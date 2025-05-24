using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Identity;
using Dominio.Usuarios;
using Dominio.Gastos;
using Dominio.Documentos;
using Repositorio;


namespace FinHealthAPI.Extensiones
{
    public static class ExtensionPoblarDatos
    {
        public async static Task PoblarDatos(this IApplicationBuilder app)
        {
            using (IServiceScope scope = app.ApplicationServices.CreateScope())
            {
                var services = scope.ServiceProvider;
                var dbContext = services.GetRequiredService<ApplicationDbContext>();

                if (!dbContext.Categorias.Any())
                {
                    List<Categoria> categorias = new List<Categoria>()
                    {
                        new Categoria { Nombre = "Fijos", Descripcion= "Recurrentes o regulares"},
                        new Categoria { Nombre = "Variables", Descripcion= "Bienes y servicios,luz,agua,telefono,etc"},
                        new Categoria { Nombre = "Imprevistos", Descripcion= "Gastos imprevistos o emergentes"},
                        new Categoria { Nombre = "Ahorro y/o Inversion", Descripcion= "Gastos de Ahorro e Inversión"},
                        new Categoria { Nombre = "Familiares", Descripcion= "Gastos Relacionados con la Familia"},
                    };

                    dbContext.Categorias.AddRange(categorias);
                }

                // Poblar Categorías
                if (!dbContext.TipoDeDocumentos.Any())
                {
                    List<TipoDeDocumento> tiposDeDocumentos = new List<TipoDeDocumento>()
                    {
                        new TipoDeDocumento { Nombre = "Factura", Descripcion= "Documento oficial que detalla la compra de bienes o servicios, generalmente emitido por un proveedor.",Ejemplo = " Factura de una compra en un supermercado o de un servicio contratado."},
                        new TipoDeDocumento { Nombre = "Recibo", Descripcion= "Comprobante simple que confirma que un pago ha sido realizado, generalmente sin tanto detalle como una factura" ,Ejemplo = "Recibo por el pago de un café o un estacionamiento."},
                        new TipoDeDocumento { Nombre = "Boleta de venta", Descripcion= "Similar al recibo, utilizado en algunos países para documentar ventas menores, especialmente en comercios minoristas." ,Ejemplo = "Boleta de venta de una tienda de conveniencia."},
                        new TipoDeDocumento { Nombre = "Nota de crédito", Descripcion= "Documento emitido para anular o ajustar una factura emitida previamente, utilizado comúnmente en devoluciones o correcciones." ,Ejemplo = "Nota de crédito por un artículo devuelto."},
                        new TipoDeDocumento { Nombre = "Comprobante de pago", Descripcion= "Documento que confirma que un pago fue realizado, pudiendo incluir transferencias bancarias, pagos electrónicos, o comprobantes de tarjeta.",Ejemplo = "Comprobante de una transferencia bancaria." },
                        new TipoDeDocumento { Nombre = "Voucher", Descripcion= "Vale o cupón que representa un crédito, descuento, o pago parcial para un bien o servicio.",Ejemplo = "Voucher de un restaurante o cupón de descuento aplicado en una compra." },
                        new TipoDeDocumento { Nombre = "Contrato de servicios", Descripcion= "Documento que detalla un acuerdo entre el usuario y un proveedor de servicios, útil si el gasto está relacionado con servicios contratados a largo plazo.",Ejemplo = "Contrato de servicios de internet o mantenimiento." },
                        new TipoDeDocumento { Nombre = "Declaración aduanera", Descripcion= "Documento requerido para el pago de aranceles de aduana, común en compras internacionales.",Ejemplo = "Declaración de impuestos aduaneros para un artículo importado" },
                        new TipoDeDocumento { Nombre = "Estado de cuenta de tarjeta", Descripcion= "Documento emitido por un banco o institución financiera que resume las transacciones realizadas con una tarjeta de crédito o débito en un período específico",Ejemplo = "Estado de cuenta mensual de la tarjeta de crédito." },
                        new TipoDeDocumento { Nombre = "Recibo electrónico", Descripcion= "Documento digital que confirma una transacción realizada en línea, común en compras por internet o pagos digitales.",Ejemplo = "Recibo electrónico de una tienda online o de un servicio de streaming." },
                        new TipoDeDocumento { Nombre = "Reporte de gastos", Descripcion= "Un documento que agrupa varios gastos en un período determinado, útil para reportes o análisis.",Ejemplo = "Reporte de gastos de un viaje de negocios." },
                        new TipoDeDocumento { Nombre = "Otros documentos", Descripcion= "Categoría genérica para cualquier documento no contemplado en las anteriores categorías, como tickets de transporte, pólizas de seguro, etc.",Ejemplo = "Póliza de seguro o ticket de estacionamiento" },
                    };
                    dbContext.TipoDeDocumentos.AddRange(tiposDeDocumentos);
                }
                if (!dbContext.MetodosDePago.Any())
                {
                    List<MetodoDePago> metodosDePago = new List<MetodoDePago>()
                    {
                        new MetodoDePago { Nombre = "Efectivo", Descripcion= "El pago se realiza en billetes y monedas físicas."},
                        new MetodoDePago { Nombre = "Tarjeta", Descripcion= "El pago se realiza utilizando una tarjeta de débito o crédito, ya sea en línea o en un punto de venta físico."},
                        new MetodoDePago { Nombre = "Transferencia Bancaria", Descripcion= "El pago se realiza mediante la transferencia de dinero desde una cuenta bancaria a otra, ya sea nacional o internacional."},
                        new MetodoDePago { Nombre = "Pago con Aplicaciones de Pago Electrónico", Descripcion= "El pago se realiza utilizando aplicaciones móviles o servicios electrónicos que permiten transferencias de dinero sin necesidad de usar tarjetas bancarias físicas."},
                        new MetodoDePago { Nombre = "Cheque", Descripcion= "Un pago realizado a través de un cheque, un documento bancario que permite al titular de la cuenta emitir un pago."},
                        new MetodoDePago { Nombre = "Pago por Contra Reembolso", Descripcion= "El pago se realiza en el momento de la entrega del producto o servicio. El consumidor paga al repartidor o al mensajero en efectivo o con tarjeta."},
                        new MetodoDePago { Nombre = "Criptomonedas", Descripcion= "El pago se realiza mediante una moneda digital descentralizada, como Bitcoin, Ethereum, entre otras."},
                    };

                    dbContext.MetodosDePago.AddRange(metodosDePago);
                }
                if (!dbContext.Monedas.Any())
                {
                    List<Moneda> monedas = new List<Moneda>
                    {
                        new Moneda("UYU", "Peso Uruguayo", "$", 40.00m, "Uruguay"),
                        new Moneda("UI", "Unidad Indexada", "$", 40.00m, "Uruguay"),
                        new Moneda("ARS", "Peso Argentino", "$", 40.00m, "Argentina"),
                        new Moneda("BRL", "Real", "$", 40.00m, "Brasil"),
                        new Moneda("USD", "Dólar estadounidense", "$", 1.00m, "Estados Unidos"),
                        new Moneda("EUR", "Euro", "€", 0.85m, "Unión Europea"),
                        new Moneda("JPY", "Yen japonés", "¥", 110.00m, "Japón"),
                        new Moneda("GBP", "Libra esterlina", "£", 0.75m, "Reino Unido"),
                        new Moneda("AUD", "Dólar australiano", "A$", 1.35m, "Australia"),
                        new Moneda("CAD", "Dólar canadiense", "C$", 1.25m, "Canadá"),
                        new Moneda("CHF", "Franco suizo", "CHF", 0.92m, "Suiza"),
                        new Moneda("CNY", "Yuan chino", "¥", 6.45m, "China"),
                        new Moneda("SEK", "Corona sueca", "kr", 8.60m, "Suecia"),
                        new Moneda("NZD", "Dólar neozelandés", "NZ$", 1.40m, "Nueva Zelanda")
                    };
                    dbContext.Monedas.AddRange(monedas);
                };

                // Guardar los cambios
                await dbContext.SaveChangesAsync();

            }
        }
    }
}
