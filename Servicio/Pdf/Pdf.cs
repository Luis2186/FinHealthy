using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Servicio.Pdf
{
    public class Pdf<T> : IDocument
    {
        public T Valor { get; set; }
        private readonly List<T> _data;
        private readonly string _title;

        public Pdf(List<T> data, string title = "Reporte")
        {
            _data = data;
            _title = title;
        }
        public DocumentMetadata GetMetadata() => DocumentMetadata.Default;

        public void Compose(IDocumentContainer container)
        {

            container.Page(page =>
            {
                page.Size(PageSizes.A4.Landscape());
                page.Margin(30);
                page.PageColor(Colors.White);
                page.DefaultTextStyle(x => x.FontSize(12));
                
                page.Header().Container().PaddingBottom(40)
                    .Text(_title)
                    .FontSize(18)
                    .Bold()
                    .FontColor(Colors.Blue.Medium)
                    .AlignCenter();

                page.Content().Element(ComposeContent);

                page.Footer()
                    .AlignCenter()
                    .Text(x =>
                    {
                        x.Span("Página ");
                        x.CurrentPageNumber();
                        x.Span(" de ");
                        x.TotalPages();
                    });
            });
        }

        private void ComposeContent(IContainer container)
        {
            if (_data.Count == 0)
            {
                container.AlignCenter().Text("No hay datos para mostrar").FontSize(14).Italic();
                return;
            }

            container.Table(table =>
            {

                // Configurar columnas dinámicamente basado en las propiedades del tipo T
                table.ColumnsDefinition(columns =>
                {
                    columns.ConstantColumn(100);
                    columns.ConstantColumn(100);
                    columns.ConstantColumn(100);
                    columns.ConstantColumn(50);
                    columns.RelativeColumn();
                    columns.RelativeColumn();
                    columns.RelativeColumn();




                    //foreach (var property in typeof(T).GetProperties())
                    //{
                    //    columns.RelativeColumn(10); // Definir columnas iguales
                    //}
                });

                // Encabezados
                table.Header(header =>
                {
                    
                    foreach (var property in typeof(T).GetProperties())
                    {
                        header.Cell().Element(CellStyle).Text(property.Name).Bold();
                    }
                });

                // Filas
                foreach (var item in _data)
                {
                    foreach (var property in typeof(T).GetProperties())
                    {
                        var value = property.GetValue(item)?.ToString() ?? "N/A";
                        table.Cell().Element(CellStyle).Text(value);
                    }
                }
            });
        }

        private static IContainer CellStyle(IContainer container)
        {
            return container.Padding(5).BorderBottom(1).BorderColor(Colors.Grey.Lighten2).AlignCenter().AlignMiddle();
        }
    }
}
