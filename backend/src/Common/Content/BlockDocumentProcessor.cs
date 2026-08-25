using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Nodes;
using Ganss.Xss;

namespace LeksanaStudio.Common.Content
{
    /// <summary>
    /// Checks, sanitises, and normalises a body document.
    ///
    /// Three things happen here, in order, and the order matters:
    /// <list type="number">
    ///   <item>the shape is checked — unknown block kinds are rejected, not ignored;</item>
    ///   <item>rich text is put through a real sanitiser against a closed allow-list;</item>
    ///   <item>document-wide rules are counted — three metrics, at most four notes.</item>
    /// </list>
    ///
    /// The sanitiser is a library, deliberately. A hand-written one passes every
    /// test its author thought of and fails the one they did not.
    /// </summary>
    public class BlockDocumentProcessor : IBlockDocumentProcessor
    {
        private static readonly JsonSerializerOptions WriteOptions = new()
        {
            Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
            WriteIndented = false,
        };

        private readonly HtmlSanitizer _sanitizer = BuildSanitizer();

        private static HtmlSanitizer BuildSanitizer()
        {
            var sanitizer = new HtmlSanitizer();

            sanitizer.AllowedTags.Clear();
            foreach (var tag in new[] { "p", "br", "strong", "em", "a", "ul", "ol", "li", "code" })
                sanitizer.AllowedTags.Add(tag);

            // `href` only. No style, no class, no id — those are the design's to
            // decide, and a pasted `style` is how a document stops matching its page.
            sanitizer.AllowedAttributes.Clear();
            sanitizer.AllowedAttributes.Add("href");

            sanitizer.AllowedSchemes.Clear();
            sanitizer.AllowedSchemes.Add("http");
            sanitizer.AllowedSchemes.Add("https");
            sanitizer.AllowedSchemes.Add("mailto");

            sanitizer.AllowedCssProperties.Clear();
            sanitizer.KeepChildNodes = true;

            return sanitizer;
        }

        public BlockDocumentResult Process(string? json)
        {
            if (string.IsNullOrWhiteSpace(json))
                return new BlockDocumentResult(true, null, "[]");

            JsonNode? parsed;
            try
            {
                parsed = JsonNode.Parse(json);
            }
            catch (JsonException)
            {
                return Invalid("Isi tulisan tidak terbaca. Muat ulang halaman lalu coba lagi.");
            }

            if (parsed is not JsonArray blocks)
                return Invalid("Isi tulisan harus berupa daftar blok.");

            if (blocks.Count > BlockLimits.MaxBlocksPerDocument)
                return Invalid($"Terlalu banyak blok. Maksimal {BlockLimits.MaxBlocksPerDocument}.");

            var counters = new DocumentCounters();
            var normalised = new JsonArray();

            for (var index = 0; index < blocks.Count; index++)
            {
                var result = ProcessBlock(blocks[index], index + 1, counters, depth: 0);
                if (!result.IsValid)
                    return result;

                normalised.Add(JsonNode.Parse(result.Json!));
            }

            if (counters.Notes > BlockLimits.MaxNotesPerDocument)
            {
                return Invalid(
                    $"Catatan pinggir maksimal {BlockLimits.MaxNotesPerDocument} per tulisan. "
                        + $"Sekarang ada {counters.Notes}. Lebih dari itu, catatan pinggir berhenti "
                        + "terbaca sebagai catatan."
                );
            }

            return new BlockDocumentResult(true, null, normalised.ToJsonString(WriteOptions));
        }

        private BlockDocumentResult ProcessBlock(
            JsonNode? node,
            int position,
            DocumentCounters counters,
            int depth
        )
        {
            if (node is not JsonObject block)
                return Invalid($"Blok ke-{position} tidak terbaca.");

            var kind = block["type"]?.GetValue<string>();
            if (kind is null || !BlockKind.All.Contains(kind))
                return Invalid($"Blok ke-{position} memakai jenis yang tidak dikenal.");

            // A stable id per block: React keys today, per-block translation later.
            //
            // Unique *within the document*, and checked rather than assumed. The
            // first version took the leading 12 characters of a v7 GUID — which
            // are the millisecond timestamp, identical for every block written in
            // the same millisecond. Forty-three blocks came back sharing two ids,
            // and the editor, which keys its open/closed state by id, opened
            // twenty cards at a time.
            var id = block["id"]?.GetValue<string>();
            if (string.IsNullOrWhiteSpace(id) || !counters.Ids.Add(id))
                block["id"] = NewBlockId(counters);

            return kind switch
            {
                BlockKind.RichText => ProcessRichText(block, position),
                BlockKind.Heading => ProcessHeading(block, position),
                BlockKind.Decision => ProcessDecision(block, position, counters, depth),
                BlockKind.Figure => ProcessFigure(block, position),
                BlockKind.Metrics => ProcessMetrics(block, position),
                BlockKind.Note => ProcessNote(block, position, counters),
                BlockKind.CodeBlock => ProcessCode(block, position),
                BlockKind.Table => ProcessTable(block, position),
                _ => Invalid($"Blok ke-{position} memakai jenis yang tidak dikenal."),
            };
        }

        private BlockDocumentResult ProcessRichText(JsonObject block, int position)
        {
            var html = _sanitizer.Sanitize(block["html"]?.GetValue<string>() ?? string.Empty);

            if (html.Length > BlockLimits.RichTextMaxChars)
            {
                return Invalid(
                    $"Blok teks ke-{position} terlalu panjang "
                        + $"({html.Length} dari {BlockLimits.RichTextMaxChars} karakter). "
                        + "Pecah jadi beberapa blok."
                );
            }

            block["html"] = html;
            return Ok(block);
        }

        private static BlockDocumentResult ProcessHeading(JsonObject block, int position)
        {
            var level = block["level"]?.GetValue<int>() ?? 2;
            if (level is not (2 or 3))
                return Invalid($"Judul ke-{position} harus tingkat 2 atau 3.");

            var text = (block["text"]?.GetValue<string>() ?? string.Empty).Trim();
            if (text.Length == 0)
                return Invalid($"Judul ke-{position} masih kosong.");
            if (text.Length > BlockLimits.HeadingMaxChars)
                return TooLong("Judul", position, text.Length, BlockLimits.HeadingMaxChars);

            block["level"] = level;
            block["text"] = text;
            return Ok(block);
        }

        private BlockDocumentResult ProcessDecision(
            JsonObject block,
            int position,
            DocumentCounters counters,
            int depth
        )
        {
            if (depth > 0)
                return Invalid($"Blok keputusan ke-{position} tidak boleh berada di dalam blok lain.");

            var step = block["step"]?.GetValue<int>() ?? 0;
            if (step < 1)
                return Invalid($"Blok keputusan ke-{position} perlu nomor langkah.");

            // The house format from blueprint 05: "I chose X because Y, even though Z."
            // A decision without a rejected alternative is a preference, and the
            // form is where that stops being a matter of the writer's discipline.
            foreach (var (field, label, max) in DecisionFields)
            {
                var value = (block[field]?.GetValue<string>() ?? string.Empty).Trim();
                if (value.Length == 0)
                {
                    return Invalid(
                        $"Blok keputusan ke-{position}: \"{label}\" wajib diisi. "
                            + "Keputusan tanpa alternatif yang ditolak bukan keputusan."
                    );
                }
                if (value.Length > max)
                    return TooLong($"Keputusan ke-{position} — {label}", position, value.Length, max);

                block[field] = value;
            }

            if (block["body"] is JsonArray body)
            {
                if (body.Count > BlockLimits.MaxNestedBlocks)
                    return Invalid($"Isi blok keputusan ke-{position} terlalu panjang.");

                var normalised = new JsonArray();
                for (var i = 0; i < body.Count; i++)
                {
                    var inner = ProcessBlock(body[i], i + 1, counters, depth + 1);
                    if (!inner.IsValid)
                        return inner;
                    normalised.Add(JsonNode.Parse(inner.Json!));
                }
                block["body"] = normalised;
            }
            else
            {
                block["body"] = new JsonArray();
            }

            return Ok(block);
        }

        private static readonly (string Field, string Label, int Max)[] DecisionFields =
        [
            ("title", "Judul", BlockLimits.DecisionTitleMaxChars),
            ("chose", "Saya memilih", BlockLimits.DecisionClauseMaxChars),
            ("because", "Karena", BlockLimits.DecisionClauseMaxChars),
            ("despite", "Walaupun", BlockLimits.DecisionClauseMaxChars),
        ];

        private static BlockDocumentResult ProcessFigure(JsonObject block, int position)
        {
            var variant = block["variant"]?.GetValue<string>() ?? "system";
            if (!BlockLimits.FigureVariants.Contains(variant))
                return Invalid($"Gambar ke-{position} memakai jenis skema yang tidak dikenal.");

            var alt = (block["alt"]?.GetValue<string>() ?? string.Empty).Trim();
            if (alt.Length < BlockLimits.FigureAltMinChars)
            {
                return Invalid(
                    $"Gambar ke-{position}: deskripsi gambar perlu menjelaskan isinya, "
                        + $"minimal {BlockLimits.FigureAltMinChars} karakter. "
                        + "Ini yang dibaca orang yang tidak bisa melihat gambarnya."
                );
            }
            if (alt.Length > BlockLimits.FigureAltMaxChars)
                return TooLong($"Gambar ke-{position} — deskripsi", position, alt.Length, BlockLimits.FigureAltMaxChars);

            var caption = (block["caption"]?.GetValue<string>() ?? string.Empty).Trim();
            if (caption.Length > BlockLimits.FigureCaptionMaxChars)
                return TooLong($"Gambar ke-{position} — keterangan", position, caption.Length, BlockLimits.FigureCaptionMaxChars);

            block["variant"] = variant;
            block["alt"] = alt;
            block["caption"] = caption;
            return Ok(block);
        }

        private static BlockDocumentResult ProcessMetrics(JsonObject block, int position)
        {
            if (block["items"] is not JsonArray items || items.Count != BlockLimits.MetricsCount)
            {
                return Invalid(
                    $"Blok metrik ke-{position} harus berisi tepat {BlockLimits.MetricsCount} angka."
                );
            }

            foreach (var item in items)
            {
                if (item is not JsonObject metric)
                    return Invalid($"Blok metrik ke-{position} tidak terbaca.");

                var value = (metric["value"]?.GetValue<string>() ?? string.Empty).Trim();
                var label = (metric["label"]?.GetValue<string>() ?? string.Empty).Trim();

                if (value.Length == 0 || label.Length == 0)
                    return Invalid($"Blok metrik ke-{position}: angka dan keterangannya wajib diisi.");
                if (value.Length > BlockLimits.MetricValueMaxChars)
                    return TooLong($"Metrik ke-{position} — angka", position, value.Length, BlockLimits.MetricValueMaxChars);
                if (label.Length > BlockLimits.MetricLabelMaxChars)
                    return TooLong($"Metrik ke-{position} — keterangan", position, label.Length, BlockLimits.MetricLabelMaxChars);

                metric["value"] = value;
                metric["label"] = label;
            }

            return Ok(block);
        }

        private BlockDocumentResult ProcessNote(JsonObject block, int position, DocumentCounters counters)
        {
            counters.Notes++;

            var html = _sanitizer.Sanitize(block["html"]?.GetValue<string>() ?? string.Empty);
            if (html.Length == 0)
                return Invalid($"Catatan pinggir ke-{position} masih kosong.");
            if (html.Length > BlockLimits.NoteMaxChars)
                return TooLong($"Catatan pinggir ke-{position}", position, html.Length, BlockLimits.NoteMaxChars);

            block["html"] = html;
            return Ok(block);
        }

        private static BlockDocumentResult ProcessCode(JsonObject block, int position)
        {
            var code = block["code"]?.GetValue<string>() ?? string.Empty;
            if (code.Length == 0)
                return Invalid($"Blok kode ke-{position} masih kosong.");
            if (code.Length > BlockLimits.CodeMaxChars)
                return TooLong($"Blok kode ke-{position}", position, code.Length, BlockLimits.CodeMaxChars);

            block["code"] = code;
            block["language"] = (block["language"]?.GetValue<string>() ?? string.Empty).Trim();
            return Ok(block);
        }

        private static BlockDocumentResult ProcessTable(JsonObject block, int position)
        {
            if (block["head"] is not JsonArray head || head.Count == 0)
                return Invalid($"Tabel ke-{position} perlu baris judul.");
            if (head.Count > BlockLimits.TableMaxColumns)
                return Invalid($"Tabel ke-{position} maksimal {BlockLimits.TableMaxColumns} kolom.");

            if (block["rows"] is not JsonArray rows)
                return Invalid($"Tabel ke-{position} tidak terbaca.");
            if (rows.Count > BlockLimits.TableMaxRows)
                return Invalid($"Tabel ke-{position} maksimal {BlockLimits.TableMaxRows} baris.");

            foreach (var cell in head)
            {
                if ((cell?.GetValue<string>() ?? string.Empty).Length > BlockLimits.TableCellMaxChars)
                    return Invalid($"Tabel ke-{position}: isi sel terlalu panjang.");
            }

            foreach (var row in rows)
            {
                if (row is not JsonArray cells || cells.Count != head.Count)
                    return Invalid($"Tabel ke-{position}: jumlah sel tiap baris harus sama dengan jumlah kolom.");

                foreach (var cell in cells)
                {
                    if ((cell?.GetValue<string>() ?? string.Empty).Length > BlockLimits.TableCellMaxChars)
                        return Invalid($"Tabel ke-{position}: isi sel terlalu panjang.");
                }
            }

            return Ok(block);
        }

        public string ToPlainText(string? json)
        {
            if (string.IsNullOrWhiteSpace(json))
                return string.Empty;

            JsonNode? parsed;
            try
            {
                parsed = JsonNode.Parse(json);
            }
            catch (JsonException)
            {
                return string.Empty;
            }

            var text = new StringBuilder();
            Collect(parsed as JsonArray, text);
            return text.ToString().Trim();
        }

        private static void Collect(JsonArray? blocks, StringBuilder text)
        {
            if (blocks is null)
                return;

            foreach (var node in blocks)
            {
                if (node is not JsonObject block)
                    continue;

                Append(text, block["text"]?.GetValue<string>());
                Append(text, StripTags(block["html"]?.GetValue<string>()));
                Append(text, block["title"]?.GetValue<string>());
                Append(text, block["caption"]?.GetValue<string>());

                Collect(block["body"] as JsonArray, text);
            }
        }

        private static void Append(StringBuilder text, string? value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return;
            if (text.Length > 0)
                text.Append(' ');
            text.Append(value.Trim());
        }

        private static string StripTags(string? html)
        {
            if (string.IsNullOrWhiteSpace(html))
                return string.Empty;

            var text = new StringBuilder(html.Length);
            var inTag = false;
            foreach (var character in html)
            {
                if (character == '<')
                    inTag = true;
                else if (character == '>')
                    inTag = false;
                else if (!inTag)
                    text.Append(character);
            }
            return text.ToString();
        }

        private static BlockDocumentResult Ok(JsonObject block) =>
            new(true, null, block.ToJsonString(WriteOptions));

        private static BlockDocumentResult Invalid(string message) => new(false, message, null);

        private static BlockDocumentResult TooLong(string what, int position, int actual, int max) =>
            Invalid($"{what} terlalu panjang ({actual} dari {max} karakter).");

        /// <summary>
        /// A fresh id, taken from randomness rather than from the clock, and
        /// retried until it is genuinely unused in this document.
        /// </summary>
        private static string NewBlockId(DocumentCounters counters)
        {
            while (true)
            {
                var candidate = Guid.NewGuid().ToString("n")[..12];
                if (counters.Ids.Add(candidate))
                    return candidate;
            }
        }

        private sealed class DocumentCounters
        {
            public int Notes { get; set; }

            /// <summary>Ids already handed out in this document, nested blocks included.</summary>
            public HashSet<string> Ids { get; } = new(StringComparer.Ordinal);
        }
    }
}
