using Dominio;
using Dominio.Errores;
using Dominio.Gastos;
using HtmlAgilityPack;
using Repositorio.Repositorios.R_Gastos.R_Monedas;
using Servicio.ServiciosExternos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading;
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

        public async Task<Resultado<bool>> ActualizarMonedasDesdeServicio(CancellationToken cancellationToken)
        {
            try
            {
                var monedas = await _repoMoneda.ObtenerTodosAsync(cancellationToken);
                if (monedas.TieneErrores) return Resultado<bool>.Failure(monedas.Errores);
                foreach (var moneda in monedas.Valor)
                {
                    try
                    {
                        var urlServicio = $"https://v6.exchangerate-api.com/v6/c7eb5f1fd1bbd0c66a9049a4/latest/{moneda.Codigo}";
                        var response = await _httpClient.GetStringAsync(urlServicio, cancellationToken);
                        var jsonDoc = JsonDocument.Parse(response);
                        if (jsonDoc.RootElement.TryGetProperty("conversion_rates", out JsonElement conversionRates))
                        {
                            if (conversionRates.TryGetProperty("UYU", out JsonElement uyuRate))
                            {
                                decimal valorEnPesosUruguayos = uyuRate.GetDecimal();
                                if (valorEnPesosUruguayos > 0)
                                {
                                    moneda.TipoDeCambio = valorEnPesosUruguayos;
                                    await _repoMoneda.ActualizarAsync(moneda, cancellationToken);
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

        private async Task GetCotizacion(CancellationToken cancellationToken)
        {
            string url = "https://www.brou.com.uy/web/guest/cotizaciones";
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    var response = await client.GetStringAsync(url, cancellationToken);
                    HtmlDocument doc = new HtmlDocument();
                    doc.LoadHtml(response);
                    var tablaCotizaciones = doc.DocumentNode.SelectSingleNode("//table[@class='table table-striped table-condensed']");
                    if (tablaCotizaciones != null)
                    {
                        var filas = tablaCotizaciones.SelectNodes(".//tr");
                        foreach (var fila in filas)
                        {
                            var celdas = fila.SelectNodes(".//td");
                            if (celdas != null && celdas.Count == 3)
                            {
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
