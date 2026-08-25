using System.Text;
using ClosedXML.Excel;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace LeksanaStudio.Common.Export
{
    public class TableExporter : ITableExporter
    {
        private const string HeaderHex = "#2563EB";
        private const string ZebraHex = "#F3F6FC";

        public (byte[] Bytes, string ContentType, string FileName) Generate<T>(
            ExportFormat format,
            IReadOnlyList<T> rows,
            IReadOnlyList<ExportColumn<T>> columns,
            string title,
            string baseFileName
        )
        {
            var stamp = DateTimeOffset.UtcNow.ToString("yyyyMMdd");

            return format switch
            {
                ExportFormat.Csv => (
                    BuildCsv(rows, columns),
                    "text/csv",
                    $"{baseFileName}-{stamp}.csv"
                ),
                ExportFormat.Excel => (
                    BuildExcel(rows, columns),
                    "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                    $"{baseFileName}-{stamp}.xlsx"
                ),
                ExportFormat.Pdf => (
                    BuildPdf(rows, columns, title),
                    "application/pdf",
                    $"{baseFileName}-{stamp}.pdf"
                ),
                _ => throw new ArgumentOutOfRangeException(nameof(format)),
            };
        }

        // ---------- CSV ----------

        private static byte[] BuildCsv<T>(
            IReadOnlyList<T> rows,
            IReadOnlyList<ExportColumn<T>> columns
        )
        {
            var sb = new StringBuilder();
            sb.Append('﻿'); // BOM so Excel reads UTF-8 correctly
            sb.AppendLine(string.Join(",", columns.Select(c => Escape(c.Header))));

            foreach (var row in rows)
                sb.AppendLine(string.Join(",", columns.Select(c => Escape(c.Value(row)))));

            return Encoding.UTF8.GetBytes(sb.ToString());
        }

        private static string Escape(string? value)
        {
            var v = value ?? string.Empty;
            return v.Contains('"') || v.Contains(',') || v.Contains('\n') || v.Contains('\r')
                ? $"\"{v.Replace("\"", "\"\"")}\""
                : v;
        }

        // ---------- Excel ----------

        private static byte[] BuildExcel<T>(
            IReadOnlyList<T> rows,
            IReadOnlyList<ExportColumn<T>> columns
        )
        {
            using var workbook = new XLWorkbook();
            var ws = workbook.Worksheets.Add("Data");

            for (var c = 0; c < columns.Count; c++)
            {
                var cell = ws.Cell(1, c + 1);
                cell.Value = columns[c].Header;
                cell.Style.Font.Bold = true;
                cell.Style.Font.FontColor = XLColor.White;
                cell.Style.Fill.BackgroundColor = XLColor.FromHtml(HeaderHex);
                cell.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
            }

            for (var r = 0; r < rows.Count; r++)
            for (var c = 0; c < columns.Count; c++)
                ws.Cell(r + 2, c + 1).Value = columns[c].Value(rows[r]) ?? string.Empty;

            ws.Row(1).Height = 20;
            ws.Columns().AdjustToContents();
            foreach (var col in ws.ColumnsUsed())
                if (col.Width > 60)
                    col.Width = 60;

            using var ms = new MemoryStream();
            workbook.SaveAs(ms);
            return ms.ToArray();
        }

        // ---------- PDF ----------

        private static byte[] BuildPdf<T>(
            IReadOnlyList<T> rows,
            IReadOnlyList<ExportColumn<T>> columns,
            string title
        )
        {
            var landscape = columns.Count > 4;
            var exportedAt = DateTimeOffset.UtcNow.ToString("dd-MM-yyyy");

            var document = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(landscape ? PageSizes.A4.Landscape() : PageSizes.A4);
                    page.Margin(28);
                    page.DefaultTextStyle(x => x.FontSize(9));

                    page.Header()
                        .Column(col =>
                        {
                            col.Item().Text(title).FontSize(15).Bold();
                            col.Item()
                                .Text($"Diekspor {exportedAt} · {rows.Count} data")
                                .FontSize(9)
                                .FontColor(Colors.Grey.Medium);
                        });

                    page.Content()
                        .PaddingTop(10)
                        .Table(table =>
                        {
                            table.ColumnsDefinition(cols =>
                            {
                                foreach (var _ in columns)
                                    cols.RelativeColumn();
                            });

                            table.Header(header =>
                            {
                                foreach (var c in columns)
                                    header
                                        .Cell()
                                        .Background(HeaderHex)
                                        .Padding(5)
                                        .Text(c.Header)
                                        .FontColor(Colors.White)
                                        .Bold();
                            });

                            for (var r = 0; r < rows.Count; r++)
                            {
                                var background = r % 2 == 0 ? "#FFFFFF" : ZebraHex;
                                foreach (var c in columns)
                                    table
                                        .Cell()
                                        .Background(background)
                                        .Padding(5)
                                        .Text(c.Value(rows[r]) ?? string.Empty);
                            }
                        });

                    page.Footer()
                        .AlignCenter()
                        .Text(text =>
                        {
                            text.Span("Halaman ");
                            text.CurrentPageNumber();
                            text.Span(" / ");
                            text.TotalPages();
                        });
                });
            });

            return document.GeneratePdf();
        }
    }
}
