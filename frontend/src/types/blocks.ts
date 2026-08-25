/**
 * The block contract, as the server defines it.
 *
 * These types mirror `BlockKind` and `BlockDocumentProcessor` in the API. The
 * numbers that go with them are **not** duplicated here — they are fetched from
 * `GET /public/block-schema`, because a second hand-written copy of a limit is
 * a copy that will eventually disagree.
 */

export type BlockKind =
  | 'richText'
  | 'heading'
  | 'decision'
  | 'figure'
  | 'metrics'
  | 'note'
  | 'codeBlock'
  | 'table'

export interface RichTextBlock {
  id: string
  type: 'richText'
  /** Sanitised on the server against a closed allow-list. */
  html: string
}

export interface HeadingBlock {
  id: string
  type: 'heading'
  level: 2 | 3
  text: string
}

/**
 * The house format from blueprint 05: "I chose X because Y, even though Z."
 * `despite` is required — a decision without a rejected alternative is a
 * preference, and the form is where that stops depending on the writer.
 */
export interface DecisionBlock {
  id: string
  type: 'decision'
  step: number
  title: string
  chose: string
  because: string
  despite: string
  body: RichTextBlock[]
}

export interface FigureBlock {
  id: string
  type: 'figure'
  /** Stand-in drawing used while no real screenshot exists. */
  variant: 'system' | 'website' | 'catalog'
  mediaId?: string | null
  src?: string | null
  /** Describes the image for someone who cannot see it. Never a filename. */
  alt: string
  caption: string
}

export interface MetricsBlock {
  id: string
  type: 'metrics'
  /** Exactly three — blueprint 05, enforced by the server. */
  items: { value: string; label: string }[]
}

export interface NoteBlock {
  id: string
  type: 'note'
  html: string
}

export interface CodeBlock {
  id: string
  type: 'codeBlock'
  language: string
  code: string
}

export interface TableBlock {
  id: string
  type: 'table'
  head: string[]
  rows: string[][]
}

export type Block =
  | RichTextBlock
  | HeadingBlock
  | DecisionBlock
  | FigureBlock
  | MetricsBlock
  | NoteBlock
  | CodeBlock
  | TableBlock

/** Limits served by the API so the editor and the server agree by construction. */
export interface BlockLimits {
  maxBlocksPerDocument: number
  maxNestedBlocks: number
  richTextMaxChars: number
  headingMaxChars: number
  noteMaxChars: number
  codeMaxChars: number
  decisionTitleMaxChars: number
  decisionClauseMaxChars: number
  figureAltMinChars: number
  figureAltMaxChars: number
  figureCaptionMaxChars: number
  metricsCount: number
  metricValueMaxChars: number
  metricLabelMaxChars: number
  maxNotesPerDocument: number
  tableMaxColumns: number
  tableMaxRows: number
  tableCellMaxChars: number
}

export interface BlockSchema {
  kinds: BlockKind[]
  limits: BlockLimits
  figureVariants: FigureBlock['variant'][]
}

/**
 * Fallbacks used only until the schema arrives, so the editor can render on the
 * first paint instead of flashing empty. The server remains the authority.
 */
export const FALLBACK_LIMITS: BlockLimits = {
  maxBlocksPerDocument: 200,
  maxNestedBlocks: 40,
  richTextMaxChars: 4000,
  headingMaxChars: 90,
  noteMaxChars: 400,
  codeMaxChars: 6000,
  decisionTitleMaxChars: 140,
  decisionClauseMaxChars: 220,
  figureAltMinChars: 10,
  figureAltMaxChars: 220,
  figureCaptionMaxChars: 220,
  metricsCount: 3,
  metricValueMaxChars: 20,
  metricLabelMaxChars: 40,
  maxNotesPerDocument: 4,
  tableMaxColumns: 8,
  tableMaxRows: 60,
  tableCellMaxChars: 300,
}

const newId = (): string =>
  // Not security-sensitive: this only has to be unique inside one document, and
  // the server reissues it anyway if it is missing.
  Math.random().toString(36).slice(2, 10) + Date.now().toString(36).slice(-4)

/** A fresh block of the requested kind, filled with the emptiest valid shape. */
export const createBlock = (kind: BlockKind, metricsCount = 3): Block => {
  switch (kind) {
    case 'richText':
      return { id: newId(), type: 'richText', html: '' }
    case 'heading':
      return { id: newId(), type: 'heading', level: 2, text: '' }
    case 'decision':
      return {
        id: newId(),
        type: 'decision',
        step: 1,
        title: '',
        chose: '',
        because: '',
        despite: '',
        body: [],
      }
    case 'figure':
      return { id: newId(), type: 'figure', variant: 'system', alt: '', caption: '' }
    case 'metrics':
      return {
        id: newId(),
        type: 'metrics',
        items: Array.from({ length: metricsCount }, () => ({ value: '', label: '' })),
      }
    case 'note':
      return { id: newId(), type: 'note', html: '' }
    case 'codeBlock':
      return { id: newId(), type: 'codeBlock', language: '', code: '' }
    case 'table':
      return { id: newId(), type: 'table', head: ['', ''], rows: [['', '']] }
  }
}

export const createRichTextBlock = (): RichTextBlock => ({
  id: newId(),
  type: 'richText',
  html: '',
})

/**
 * Repairs a document whose blocks share ids.
 *
 * The editor keys a block's open/closed state — and its React key — by id, so a
 * repeated id makes two cards behave as one. The server guarantees uniqueness on
 * write, but writing is not the only way a document arrives: anything stored
 * before that guarantee existed is still out there, and it should behave
 * correctly the moment it is opened rather than the moment it is next saved.
 *
 * Only the duplicates are touched; the first block to claim an id keeps it.
 */
export const withUniqueBlockIds = (blocks: readonly Block[]): Block[] => {
  const seen = new Set<string>()

  const repair = (list: readonly Block[]): Block[] =>
    list.map((block) => {
      const id = block.id && !seen.has(block.id) ? block.id : newId()
      seen.add(id)

      if (block.type === 'decision') {
        return { ...block, id, body: repair(block.body ?? []) as RichTextBlock[] }
      }

      return { ...block, id }
    })

  return repair(blocks)
}

/** Label and one-line purpose for each kind, as shown in the add menu. */
export const BLOCK_META: Record<BlockKind, { label: string; hint: string }> = {
  richText: { label: 'Teks', hint: 'Paragraf biasa. Ini yang paling sering dipakai.' },
  heading: { label: 'Judul bagian', hint: 'Memecah tulisan panjang jadi bagian.' },
  decision: {
    label: 'Keputusan',
    hint: 'Format rumah: saya memilih X karena Y, walaupun Z.',
  },
  figure: { label: 'Gambar', hint: 'Tangkapan layar, atau skema pengganti sementara.' },
  metrics: { label: 'Metrik', hint: 'Tiga angka utama. Dipakai sekali, di dekat atas.' },
  note: { label: 'Catatan pinggir', hint: 'Alasan, keberatan, atau batas. Bukan fitur.' },
  codeBlock: { label: 'Kode', hint: 'Potongan kode atau keluaran terminal.' },
  table: { label: 'Tabel', hint: 'Perbandingan. Menggulir sendiri di layar kecil.' },
}

/** One-line summary shown when a block is collapsed. */
export const blockSummary = (block: Block): string => {
  const strip = (html: string) => html.replace(/<[^>]*>/g, ' ').replace(/\s+/g, ' ').trim()

  switch (block.type) {
    case 'richText':
      return strip(block.html) || 'Belum ada isi'
    case 'heading':
      return block.text || 'Judul belum diisi'
    case 'decision':
      return block.title || 'Keputusan belum diberi judul'
    case 'figure':
      return block.caption || block.alt || 'Gambar belum dijelaskan'
    case 'metrics':
      return block.items.map((item) => `${item.value} ${item.label}`.trim()).join(' · ') || 'Angka belum diisi'
    case 'note':
      return strip(block.html) || 'Catatan belum diisi'
    case 'codeBlock':
      return block.code.split('\n')[0] || 'Kode belum diisi'
    case 'table':
      return block.head.filter(Boolean).join(' · ') || 'Tabel belum diisi'
  }
}
