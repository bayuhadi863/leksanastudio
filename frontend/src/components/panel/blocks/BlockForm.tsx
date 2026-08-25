import { MediaPickerButton } from '@/components/panel/media/MediaField'
import { RichTextInput } from '@/components/panel/form/RichTextInput'
import { FieldRow } from '@/components/panel/form/FieldRow'
import { Repeater } from '@/components/panel/form/Repeater'
import {
  NumberInput,
  SegmentedInput,
  TextAreaInput,
  TextInput,
} from '@/components/panel/form/Inputs'
import { Label } from '@/components/ui/Label'
import type {
  Block,
  BlockLimits,
  CodeBlock,
  DecisionBlock,
  FigureBlock,
  HeadingBlock,
  MetricsBlock,
  NoteBlock,
  RichTextBlock,
  TableBlock,
} from '@/types/blocks'
import { createRichTextBlock } from '@/types/blocks'

type Props = {
  readonly block: Block
  readonly onChange: (block: Block) => void
  readonly limits: BlockLimits
}

/** Routes a block to the form its kind deserves. */
export function BlockForm({ block, onChange, limits }: Props) {
  switch (block.type) {
    case 'richText':
      return <RichTextForm block={block} onChange={onChange} limits={limits} />
    case 'heading':
      return <HeadingForm block={block} onChange={onChange} limits={limits} />
    case 'decision':
      return <DecisionForm block={block} onChange={onChange} limits={limits} />
    case 'figure':
      return <FigureForm block={block} onChange={onChange} limits={limits} />
    case 'metrics':
      return <MetricsForm block={block} onChange={onChange} limits={limits} />
    case 'note':
      return <NoteForm block={block} onChange={onChange} limits={limits} />
    case 'codeBlock':
      return <CodeForm block={block} onChange={onChange} limits={limits} />
    case 'table':
      return <TableForm block={block} onChange={onChange} limits={limits} />
  }
}

/* ------------------------------------------------------------------- text */

function RichTextForm({
  block,
  onChange,
  limits,
}: {
  block: RichTextBlock
  onChange: (block: Block) => void
  limits: BlockLimits
}) {
  return (
    <RichTextInput
      value={block.html}
      onChange={(html) => onChange({ ...block, html })}
      maxLength={limits.richTextMaxChars}
    />
  )
}

/* ---------------------------------------------------------------- heading */

function HeadingForm({
  block,
  onChange,
  limits,
}: {
  block: HeadingBlock
  onChange: (block: Block) => void
  limits: BlockLimits
}) {
  return (
    <div className="grid gap-4">
      <TextInput
        label="Judul bagian"
        value={block.text}
        maxLength={limits.headingMaxChars}
        onChange={(text) => onChange({ ...block, text })}
        placeholder="Masalah"
        required
      />
      <SegmentedInput
        label="Tingkat"
        hint="Tingkat 2 memecah tulisan; tingkat 3 memecah bagian di dalamnya."
        value={String(block.level) as '2' | '3'}
        onChange={(level) => onChange({ ...block, level: Number(level) as 2 | 3 })}
        options={[
          { value: '2', label: 'Bagian' },
          { value: '3', label: 'Sub-bagian' },
        ]}
      />
    </div>
  )
}

/* --------------------------------------------------------------- decision */

/**
 * The house format, as a form.
 *
 * The four fields are not styling: blueprint 05 fixes the shape as "I chose X
 * because Y, even though Z", and requires the rejected alternative. Asking for
 * it in its own labelled box is what turns a rule a writer has to remember into
 * one they cannot skip.
 */
function DecisionForm({
  block,
  onChange,
  limits,
}: {
  block: DecisionBlock
  onChange: (block: Block) => void
  limits: BlockLimits
}) {
  return (
    <div className="grid gap-4">
      <FieldRow columns="7rem minmax(0, 1fr)">
        <NumberInput
          label="Langkah"
          value={block.step}
          min={1}
          onChange={(step) => onChange({ ...block, step: step || 1 })}
        />
        <TextInput
          label="Judul keputusan"
          value={block.title}
          maxLength={limits.decisionTitleMaxChars}
          onChange={(title) => onChange({ ...block, title })}
          placeholder="Menyelundupkan metode yang diblokir lewat POST"
          required
        />
      </FieldRow>

      <div className="border-line bg-bg grid gap-4 rounded-[var(--radius-control)] border p-4">
        <p className="type-small text-muted">
          Saya memilih <span className="text-accent font-semibold">X</span> karena{' '}
          <span className="font-semibold">Y</span>, walaupun <span className="italic">Z</span>.
        </p>

        <TextAreaInput
          label="Saya memilih…"
          value={block.chose}
          rows={2}
          maxLength={limits.decisionClauseMaxChars}
          onChange={(chose) => onChange({ ...block, chose })}
          placeholder="mempertahankan REST yang benar dan menambah lapisan penerjemah"
          required
        />
        <TextAreaInput
          label="…karena…"
          value={block.because}
          rows={2}
          maxLength={limits.decisionClauseMaxChars}
          onChange={(because) => onChange({ ...block, because })}
          placeholder="kendala ini milik satu lingkungan, bukan milik sistemnya"
          required
        />
        <TextAreaInput
          label="…walaupun…"
          hint="Alternatif yang ditolak, dan harganya. Tanpa ini, itu preferensi — bukan keputusan."
          value={block.despite}
          rows={2}
          maxLength={limits.decisionClauseMaxChars}
          onChange={(despite) => onChange({ ...block, despite })}
          placeholder="itu berarti dua mekanisme yang harus selalu diubah berpasangan"
          required
        />
      </div>

      <Repeater
        label="Penjelasan"
        hint="Paragraf di bawah kalimat keputusan."
        items={block.body}
        max={limits.maxNestedBlocks}
        addLabel="Tambah paragraf"
        create={createRichTextBlock}
        itemLabel={(_, index) => `Paragraf ${index + 1}`}
        onChange={(body) => onChange({ ...block, body: body as RichTextBlock[] })}
        renderItem={(item, update) => (
          <RichTextInput
            value={item.html}
            maxLength={limits.richTextMaxChars}
            compact
            onChange={(html) => update({ ...item, html })}
          />
        )}
      />
    </div>
  )
}

/* ----------------------------------------------------------------- figure */

function FigureForm({
  block,
  onChange,
  limits,
}: {
  block: FigureBlock
  onChange: (block: Block) => void
  limits: BlockLimits
}) {
  return (
    <div className="grid gap-4">
      <MediaPickerButton
        mediaId={block.mediaId ?? null}
        objectPath={block.src ?? null}
        onChange={(media) =>
          onChange({
            ...block,
            mediaId: media?.id ?? null,
            // The path is stored beside the id so rendering a page never has to
            // resolve every image one lookup at a time.
            src: media?.objectPath ?? null,
          })
        }
      />

      {block.mediaId ? null : (
        <SegmentedInput
          label="Skema"
          hint="Dipakai selama belum ada tangkapan layar asli."
          value={block.variant}
          onChange={(variant) => onChange({ ...block, variant })}
          options={[
            { value: 'system', label: 'Sistem' },
            { value: 'website', label: 'Website' },
            { value: 'catalog', label: 'Katalog' },
          ]}
        />
      )}

      <TextAreaInput
        label="Deskripsi gambar"
        hint="Yang dibaca orang yang tidak bisa melihat gambarnya. Jelaskan isinya, jangan menamainya."
        value={block.alt}
        rows={2}
        maxLength={limits.figureAltMaxChars}
        onChange={(alt) => onChange({ ...block, alt })}
        placeholder="Dasbor panel administrasi: ringkasan jumlah penelitian, diikuti daftar pengajuan"
        required
        error={
          block.alt.length > 0 && block.alt.length < limits.figureAltMinChars
            ? `Minimal ${limits.figureAltMinChars} karakter.`
            : undefined
        }
      />

      <TextInput
        label="Keterangan"
        hint="Kalimat di bawah gambar. Boleh kosong."
        value={block.caption}
        maxLength={limits.figureCaptionMaxChars}
        onChange={(caption) => onChange({ ...block, caption })}
      />
    </div>
  )
}

/* ---------------------------------------------------------------- metrics */

function MetricsForm({
  block,
  onChange,
  limits,
}: {
  block: MetricsBlock
  onChange: (block: Block) => void
  limits: BlockLimits
}) {
  return (
    <Repeater
      label={`Tiga angka utama`}
      hint="Tepat tiga — bukan dua, bukan empat. Angka dulu, keterangannya di bawah."
      items={block.items}
      fixedLength
      create={() => ({ value: '', label: '' })}
      itemLabel={(_, index) => `Angka ${index + 1}`}
      onChange={(items) => onChange({ ...block, items })}
      renderItem={(item, update) => (
        <FieldRow columns="10rem minmax(0, 1fr)">
          <TextInput
            label="Angka"
            value={item.value}
            maxLength={limits.metricValueMaxChars}
            onChange={(value) => update({ ...item, value })}
            placeholder="30"
            mono
          />
          <TextInput
            label="Keterangan"
            value={item.label}
            maxLength={limits.metricLabelMaxChars}
            onChange={(label) => update({ ...item, label })}
            placeholder="Modul"
          />
        </FieldRow>
      )}
      error={
        block.items.length !== limits.metricsCount
          ? `Harus tepat ${limits.metricsCount} angka.`
          : undefined
      }
    />
  )
}

/* ------------------------------------------------------------------- note */

function NoteForm({
  block,
  onChange,
  limits,
}: {
  block: NoteBlock
  onChange: (block: Block) => void
  limits: BlockLimits
}) {
  return (
    <RichTextInput
      value={block.html}
      onChange={(html) => onChange({ ...block, html })}
      maxLength={limits.noteMaxChars}
      compact
      hint="Orang pertama. Berisi alasan, keberatan, atau batas — tidak pernah fitur, tidak pernah ajakan. 20–45 kata."
    />
  )
}

/* ------------------------------------------------------------------- code */

function CodeForm({
  block,
  onChange,
  limits,
}: {
  block: CodeBlock
  onChange: (block: Block) => void
  limits: BlockLimits
}) {
  return (
    <div className="grid gap-4">
      <TextInput
        label="Bahasa"
        hint="Untuk pelabelan saja. Boleh kosong."
        value={block.language}
        onChange={(language) => onChange({ ...block, language })}
        placeholder="bash"
        mono
      />
      <TextAreaInput
        label="Kode"
        value={block.code}
        rows={8}
        maxLength={limits.codeMaxChars}
        onChange={(code) => onChange({ ...block, code })}
        required
      />
    </div>
  )
}

/* ------------------------------------------------------------------ table */

function TableForm({
  block,
  onChange,
  limits,
}: {
  block: TableBlock
  onChange: (block: Block) => void
  limits: BlockLimits
}) {
  const setColumnCount = (count: number) => {
    const head = Array.from({ length: count }, (_, i) => block.head[i] ?? '')
    const rows = block.rows.map((row) => Array.from({ length: count }, (_, i) => row[i] ?? ''))
    onChange({ ...block, head, rows })
  }

  const updateHead = (index: number, value: string) => {
    const head = [...block.head]
    head[index] = value
    onChange({ ...block, head })
  }

  const updateCell = (rowIndex: number, cellIndex: number, value: string) => {
    const rows = block.rows.map((row, i) =>
      i === rowIndex ? row.map((cell, j) => (j === cellIndex ? value : cell)) : row,
    )
    onChange({ ...block, rows })
  }

  const addRow = () =>
    onChange({ ...block, rows: [...block.rows, block.head.map(() => '')] })

  const removeRow = (index: number) =>
    onChange({ ...block, rows: block.rows.filter((_, i) => i !== index) })

  const cellClass =
    'border-muted bg-bg focus:border-accent w-full rounded-[3px] border px-2.5 py-2 text-[0.9375rem] transition-colors duration-150 ease-out focus:outline-none'

  return (
    <div className="grid gap-4">
      <NumberInput
        label="Jumlah kolom"
        value={block.head.length}
        min={1}
        max={limits.tableMaxColumns}
        onChange={(count) =>
          setColumnCount(Math.min(Math.max(count || 1, 1), limits.tableMaxColumns))
        }
      />

      <div className="min-w-0">
        <Label as="p">Baris judul</Label>
        <div className="scroll-x mt-2">
          <div className="flex min-w-fit gap-2">
            {block.head.map((cell, index) => (
              <input
                key={index}
                type="text"
                value={cell}
                onChange={(event) => updateHead(index, event.target.value)}
                placeholder={`Kolom ${index + 1}`}
                className={`${cellClass} min-w-40 font-semibold`}
              />
            ))}
          </div>
        </div>
      </div>

      <div className="min-w-0">
        <Label as="p">Isi</Label>
        <div className="scroll-x mt-2">
          <div className="grid min-w-fit gap-2">
            {block.rows.map((row, rowIndex) => (
              <div key={rowIndex} className="flex items-center gap-2">
                {row.map((cell, cellIndex) => (
                  <input
                    key={cellIndex}
                    type="text"
                    value={cell}
                    onChange={(event) => updateCell(rowIndex, cellIndex, event.target.value)}
                    className={`${cellClass} min-w-40`}
                  />
                ))}
                <button
                  type="button"
                  onClick={() => removeRow(rowIndex)}
                  title="Hapus baris"
                  className="text-muted hover:text-danger flex h-9 w-9 shrink-0 items-center justify-center"
                >
                  <span aria-hidden="true">✕</span>
                  <span className="sr-only">Hapus baris {rowIndex + 1}</span>
                </button>
              </div>
            ))}
          </div>
        </div>

        <button
          type="button"
          onClick={addRow}
          disabled={block.rows.length >= limits.tableMaxRows}
          className="type-small text-accent mt-3 font-semibold disabled:opacity-45"
        >
          + Tambah baris
        </button>
      </div>
    </div>
  )
}
