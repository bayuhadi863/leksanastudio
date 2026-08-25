using ClosedXML.Excel;

namespace LeksanaStudio.Common.Import
{
    /// <summary>
    /// ClosedXML-backed reader/template-builder. Templates carry a clean "Data" sheet
    /// (with dropdowns for master columns), a hidden "Referensi" sheet holding the
    /// dropdown source lists, and a "Petunjuk" sheet documenting every column.
    /// </summary>
    public class TableImporter : ITableImporter
    {
        private const string HeaderHex = "#2563EB";
        private const int MaxDataRow = 500;

        public IReadOnlyList<ImportRawRow> ReadRows(Stream stream)
        {
            using var workbook = new XLWorkbook(stream);
            var ws =
                workbook.Worksheets.FirstOrDefault(w =>
                    string.Equals(w.Name, "Data", StringComparison.OrdinalIgnoreCase)
                ) ?? workbook.Worksheet(1);

            var result = new List<ImportRawRow>();

            var headerRow = ws.FirstRowUsed();
            if (headerRow == null)
                return result;

            var headers = new List<(string Header, int Col)>();
            foreach (var cell in headerRow.CellsUsed())
            {
                var header = cell.GetString().Trim();
                if (!string.IsNullOrEmpty(header))
                    headers.Add((header, cell.Address.ColumnNumber));
            }
            if (headers.Count == 0)
                return result;

            var lastRow = ws.LastRowUsed()?.RowNumber() ?? headerRow.RowNumber();

            for (var r = headerRow.RowNumber() + 1; r <= lastRow; r++)
            {
                var values = new Dictionary<string, string?>();
                var hasValue = false;

                foreach (var (header, col) in headers)
                {
                    var text = ws.Cell(r, col).GetString()?.Trim();
                    if (string.IsNullOrEmpty(text))
                    {
                        values[header] = null;
                    }
                    else
                    {
                        values[header] = text;
                        hasValue = true;
                    }
                }

                if (hasValue)
                    result.Add(new ImportRawRow { RowNumber = r, Values = values });
            }

            return result;
        }

        public byte[] BuildTemplate(string sheetTitle, IReadOnlyList<ImportColumn> columns)
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
            ws.Row(1).Height = 22;
            ws.SheetView.FreezeRows(1);

            // Dropdowns for master columns, sourced from a hidden reference sheet.
            IXLWorksheet? refSheet = null;
            var refCol = 0;
            for (var c = 0; c < columns.Count; c++)
            {
                var allowed = columns[c].AllowedValues;
                if (allowed == null || allowed.Count == 0)
                    continue;

                refSheet ??= workbook.Worksheets.Add("Referensi");
                refCol++;
                refSheet.Cell(1, refCol).Value = columns[c].Header;
                for (var i = 0; i < allowed.Count; i++)
                    refSheet.Cell(i + 2, refCol).Value = allowed[i];

                var listRange = refSheet.Range(2, refCol, allowed.Count + 1, refCol);
                var dataRange = ws.Range(2, c + 1, MaxDataRow, c + 1);
                var validation = dataRange.CreateDataValidation();
                validation.List(listRange, true); // in-cell dropdown stays visible
                validation.IgnoreBlanks = true;
                // Non-restrictive: master values are only a suggestion — users may
                // type foreign values not yet in master (handled at preview: add/skip).
                validation.ShowErrorMessage = false;
            }
            refSheet?.Hide();

            ws.Columns().AdjustToContents();
            foreach (var col in ws.ColumnsUsed())
            {
                if (col.Width < 18)
                    col.Width = 18;
                if (col.Width > 60)
                    col.Width = 60;
            }

            BuildGuideSheet(workbook, sheetTitle, columns);

            using var ms = new MemoryStream();
            workbook.SaveAs(ms);
            return ms.ToArray();
        }

        private static void BuildGuideSheet(
            XLWorkbook workbook,
            string sheetTitle,
            IReadOnlyList<ImportColumn> columns
        )
        {
            var guide = workbook.Worksheets.Add("Petunjuk");

            guide.Cell(1, 1).Value = sheetTitle;
            guide.Cell(1, 1).Style.Font.Bold = true;
            guide.Cell(1, 1).Style.Font.FontSize = 14;

            guide.Cell(2, 1).Value =
                "Isi data pada sheet \"Data\". Baris pertama (judul kolom) jangan diubah atau dihapus. "
                + "Kolom bertanda Wajib = Ya harus diisi. Untuk kolom dengan pilihan, gunakan dropdown pada sel.";
            guide.Cell(2, 1).Style.Font.FontColor = XLColor.FromHtml("#6B7280");

            var header = new[] { "Kolom", "Wajib", "Keterangan", "Contoh" };
            for (var i = 0; i < header.Length; i++)
            {
                var cell = guide.Cell(4, i + 1);
                cell.Value = header[i];
                cell.Style.Font.Bold = true;
                cell.Style.Font.FontColor = XLColor.White;
                cell.Style.Fill.BackgroundColor = XLColor.FromHtml(HeaderHex);
            }

            var row = 5;
            foreach (var col in columns)
            {
                guide.Cell(row, 1).Value = col.Header;
                guide.Cell(row, 2).Value = col.Required ? "Ya" : "Tidak";
                guide.Cell(row, 3).Value = col.Description ?? string.Empty;
                guide.Cell(row, 4).Value = col.Example ?? string.Empty;
                row++;
            }

            guide.Columns().AdjustToContents();
            guide.Column(3).Width = 64;
            if (row > 5)
                guide.Range(5, 3, row - 1, 3).Style.Alignment.WrapText = true;
            foreach (var col in guide.ColumnsUsed())
                if (col.Width > 80)
                    col.Width = 80;
        }
    }
}
