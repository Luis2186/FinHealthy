using Dominio;
using Dominio.Errores;
using Dominio.Gastos;
using HtmlAgilityPack;
using Repositorio.Repositorios.Monedas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Servicio.ServiciosExternos
{
    public class ServicioMonedas : IServicioMonedas
    {
        private readonly HttpClient _httpClient;
        private readonly IRepositorioMoneda _repoMoneda;

        public ServicioMonedas(HttpClient httpClient, IRepositorioMoneda repoMoneda)
        {
            _httpClient = httpClient;
            _repoMoneda = repoMoneda;
        }

        public async Task<Resultado<bool>> ActualizarMonedasDesdeServicio()
        {
            try
            {
                var monedas = await _repoMoneda.ObtenerTodosAsync();

                if (monedas.TieneErrores) return Resultado<bool>.Failure(monedas.Errores);

                foreach (var moneda in monedas.Valor)
                {
                    try
                    {
                        var urlServicio = $"https://v6.exchangerate-api.com/v6/c7eb5f1fd1bbd0c66a9049a4/latest/{moneda.Codigo}";
                        var response = await _httpClient.GetStringAsync(urlServicio);
                        var jsonDoc = JsonDocument.Parse(response);

                        if (jsonDoc.RootElement.TryGetProperty("conversion_rates", out JsonElement conversionRates))
                        {
                            if (conversionRates.TryGetProperty("UYU", out JsonElement uyuRate))
                            {
                                double valorEnPesosUruguayos = uyuRate.GetDouble();

                                if (valorEnPesosUruguayos > 0)
                                {
                                    moneda.TipoDeCambio = valorEnPesosUruguayos;
                                    await _repoMoneda.ActualizarAsync(moneda);
                                }
                            }                        
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error inesperado con la moneda {moneda.Codigo}: {ex.Message}");
                    }
                }
               
                return Resultado<bool>.Success(true);
            }
            catch (Exception ex)
            {
                return Resultado<bool>.Failure(ErroresCrud.ErrorDeExcepcion("ActualizarMonedasDesdeServicio", ex.Message));
            }
        }

        private async Task GetCotizacion()
        {
            // URL de la página que deseas scrapear
            string url = "https://www.brou.com.uy/web/guest/cotizaciones";

            // Usar HttpClient para realizar la solicitud HTTP
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    // Obtener el contenido HTML de la página
                    var response = await client.GetStringAsync(url);

                    // Analizar el HTML con HtmlAgilityPack
                    HtmlDocument doc = new HtmlDocument();
                    doc.LoadHtml(response);
                    // Seleccionar la tabla que contiene las cotizaciones
                    var tablaCotizaciones = doc.DocumentNode.SelectSingleNode("//table[@class='table table-striped table-condensed']");

                    if (tablaCotizaciones != null)
                    {
                        // Buscar todas las filas dentro de la tabla
                        var filas = tablaCotizaciones.SelectNodes(".//tr");

                        foreach (var fila in filas)
                        {
                            // Obtener las celdas de cada fila
                            var celdas = fila.SelectNodes(".//td");

                            if (celdas != null && celdas.Count == 3) // Asumiendo que hay 3 celdas por fila
                            {
                                // Extraer la información de las celdas
                                var moneda = celdas[0].InnerText.Trim();
                                var compra = celdas[1].InnerText.Trim();
                                var venta = celdas[2].InnerText.Trim();

                                Console.WriteLine($"Moneda: {moneda}, Compra: {compra}, Venta: {venta}");
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error al cargar la página: {ex.Message}");
                }
            }

        }


    }
}
