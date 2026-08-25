/**
 * Moves the MDX case studies into the CMS.
 *
 * Run it as often as you like: entries are matched by their content key, so a
 * second run updates what the first run created instead of duplicating it. The
 * same is true of the images — a file already in the library is reused rather
 * than uploaded again.
 *
 * Every import verifies itself. The words in the source file and the words in
 * the blocks that were written must match exactly; anything else fails the run
 * and says which words went missing. "Close enough" is how a paragraph
 * disappears from a published page and nobody notices for a month.
 *
 *   npm run import:content            # import and verify
 *   npm run import:content -- --dry   # convert and verify, write nothing
 */

import { readdir, readFile } from 'node:fs/promises'
import path from 'node:path'
import { fileURLToPath } from 'node:url'

import { connect, imageSize, resolveCredentials } from './lib/api-client.mjs'
import { blockText, mdastText, parseMdx, toBlocks, words } from './lib/mdx-to-blocks.mjs'

const here = path.dirname(fileURLToPath(import.meta.url))
const frontendRoot = path.join(here, '..')
const repoRoot = path.join(frontendRoot, '..')
const caseStudyDir = path.join(frontendRoot, 'src', 'content', 'studi-kasus')
const publicDir = path.join(frontendRoot, 'public')

const dryRun = process.argv.includes('--dry')
const locale = process.env.LEKSANA_IMPORT_LOCALE ?? 'id'

const LABELS = { klien: 'Client', 'produk-sendiri': 'OwnProduct' }
const FIGURES = { system: 'System', website: 'Website', catalog: 'Catalog' }

/* --------------------------------------------------------------- pipeline */

const files = (await readdir(caseStudyDir)).filter((name) => name.endsWith('.mdx')).sort()
if (files.length === 0) {
  console.log('Tidak ada berkas MDX studi kasus. Tidak ada yang diimpor.')
  process.exit(0)
}

console.log(`Menyiapkan ${files.length} studi kasus dari ${path.relative(repoRoot, caseStudyDir)}\n`)

const prepared = []

for (const file of files) {
  const source = await readFile(path.join(caseStudyDir, file), 'utf8')
  const contentKey = file.replace(/\.mdx$/, '')
  const context = { file }

  const { frontmatter, body } = parseMdx(source)
  const blocks = toBlocks(body, context)

  // The check that makes this import trustworthy: the same words, in the same
  // order, on both sides of the conversion.
  const before = words(mdastText(body))
  const after = words(blockText(blocks))
  const mismatch = firstDifference(before, after)

  if (mismatch) {
    console.error(`GAGAL  ${file} — teks hasil konversi tidak sama dengan sumbernya`)
    console.error(`       ${mismatch}`)
    process.exitCode = 1
    continue
  }

  console.log(
    `siap   ${file} — ${blocks.length} blok, ${before.length} kata, ` +
      `${countNotes(blocks)} catatan pinggir`,
  )

  prepared.push({ file, contentKey, frontmatter, blocks })
}

if (process.exitCode === 1) {
  console.error('\nAda berkas yang gagal dikonversi. Tidak ada yang ditulis ke basis data.')
  process.exit(1)
}

if (dryRun) {
  console.log('\n--dry: konversi dan verifikasi lolos, tidak ada yang ditulis.')
  process.exit(0)
}

/* ----------------------------------------------------------------- upload */

const credentials = await resolveCredentials(repoRoot)
console.log(`\nMasuk sebagai ${credentials.email} (dari ${credentials.source})`)

const api = await connect(credentials)

const library = await api.listMedia()
const byName = new Map(library.map((item) => [item.originalName, item]))

/** Uploads a public image once; a file already in the library is reused. */
async function ensureMedia(publicPath) {
  if (!publicPath) return null

  const name = path.basename(publicPath)
  const existing = byName.get(name)
  if (existing) return existing

  const filePath = path.join(publicDir, publicPath.replace(/^\//, ''))

  let bytes
  try {
    bytes = await readFile(filePath)
  } catch {
    throw new Error(`Berkas gambar tidak ditemukan: ${publicPath}`)
  }

  const size = imageSize(bytes)
  const media = await api.uploadMedia(filePath, {
    label: name.replace(/\.[^.]+$/, '').replace(/[-_]/g, ' '),
    width: size?.width,
    height: size?.height,
  })

  byName.set(name, media)
  console.log(`unggah ${name} — ${size ? `${size.width}×${size.height}` : 'ukuran tidak terbaca'}`)
  return media
}

/* ------------------------------------------------------------------ write */

// Matching on the content key rather than the address: an address is allowed to
// change, and an importer that matched on it would create a second copy the
// first time an editor tidied a title.
const existing = await api.listCaseStudies()
const keyed = new Map()
for (const row of existing) {
  const detail = await api.getCaseStudy(row.id)
  if (detail?.contentKey) keyed.set(detail.contentKey, detail)
}

console.log()
let createdCount = 0
let updatedCount = 0

for (const entry of prepared) {
  const { frontmatter: fm, blocks, contentKey, file } = entry

  const cover = await ensureMedia(fm.cover?.src)

  const resolvedBlocks = []
  for (const block of blocks) {
    resolvedBlocks.push(await resolveFigure(block))
  }

  const param = {
    contentKey,
    label: LABELS[fm.label] ?? 'Client',
    figure: FIGURES[fm.figure] ?? 'System',
    coverMediaId: cover?.id ?? null,
    year: Number(fm.year),
    stack: Array.isArray(fm.stack) ? fm.stack : [],
    order: Number(fm.order ?? 0),
    translations: [
      {
        localeCode: locale,
        slug: contentKey,
        status: 'Published',
        title: fm.title ?? null,
        summary: fm.summary ?? null,
        problem: fm.problem ?? null,
        client: fm.client ?? null,
        kind: fm.kind ?? null,
        duration: fm.duration ?? null,
        role: fm.role ?? null,
        coverAlt: fm.cover?.alt ?? null,
        metrics: (fm.metrics ?? []).map((metric) => ({
          value: String(metric.value),
          label: String(metric.label),
        })),
        body: resolvedBlocks,
      },
    ],
  }

  const found = keyed.get(contentKey)

  if (found) {
    await api.updateCaseStudy(found.id, param)
    updatedCount += 1
    console.log(`perbarui  ${file} → ${contentKey}`)
  } else {
    const id = await api.createCaseStudy(param)
    createdCount += 1
    console.log(`buat      ${file} → ${contentKey} (${id})`)
  }
}

/* ----------------------------------------------------------------- verify */

console.log('\nMemeriksa hasil yang tersimpan…')

let verified = 0

for (const entry of prepared) {
  const rows = await api.listCaseStudies()
  const match = []

  for (const row of rows) {
    const detail = await api.getCaseStudy(row.id)
    if (detail?.contentKey === entry.contentKey) match.push(detail)
  }

  if (match.length !== 1) {
    console.error(`GAGAL  ${entry.contentKey} — ditemukan ${match.length} entri, seharusnya 1`)
    process.exitCode = 1
    continue
  }

  const translation = match[0].translations.find((t) => t.localeCode === locale)
  if (!translation) {
    console.error(`GAGAL  ${entry.contentKey} — terjemahan ${locale} tidak tersimpan`)
    process.exitCode = 1
    continue
  }

  const storedWords = words(blockText(translation.body ?? []))
  const sourceWords = words(blockText(entry.blocks))
  const mismatch = firstDifference(sourceWords, storedWords)

  if (mismatch) {
    console.error(`GAGAL  ${entry.contentKey} — teks tersimpan berbeda: ${mismatch}`)
    process.exitCode = 1
    continue
  }

  verified += 1
  console.log(
    `sesuai ${entry.contentKey} — ${translation.body.length} blok tersimpan, ` +
      `${storedWords.length} kata, /portofolio/${translation.slug}`,
  )
}

console.log(
  `\n${createdCount} dibuat · ${updatedCount} diperbarui · ${verified}/${prepared.length} terverifikasi`,
)

if (process.exitCode === 1) {
  console.error('Impor selesai dengan kesalahan.')
} else {
  console.log('Impor selesai.')
}

/* ---------------------------------------------------------------- helpers */

/** Points a figure block at an uploaded file, keeping the schematic as fallback. */
async function resolveFigure(block) {
  if (block.type === 'decision') {
    const body = []
    for (const inner of block.body ?? []) body.push(await resolveFigure(inner))
    return { ...block, body }
  }

  if (block.type !== 'figure') return block

  const { sourcePath, ...rest } = block
  if (!sourcePath) return { ...rest, mediaId: null, src: null }

  const media = await ensureMedia(sourcePath)
  return { ...rest, mediaId: media?.id ?? null, src: media?.objectPath ?? null }
}

/** The first word that differs, with enough context to find it in the file. */
function firstDifference(expected, actual) {
  const length = Math.max(expected.length, actual.length)

  for (let index = 0; index < length; index += 1) {
    if (expected[index] === actual[index]) continue

    const context = expected.slice(Math.max(0, index - 6), index).join(' ')
    return (
      `kata ke-${index + 1} berbeda setelah "…${context}": ` +
      `sumber "${expected[index] ?? '(habis)'}" vs hasil "${actual[index] ?? '(habis)'}"`
    )
  }

  return null
}

function countNotes(blocks) {
  return blocks.reduce((total, block) => {
    if (block.type === 'note') return total + 1
    if (block.type === 'decision') return total + countNotes(block.body ?? [])
    return total
  }, 0)
}
