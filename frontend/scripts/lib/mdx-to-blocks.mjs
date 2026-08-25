/**
 * MDX to typed blocks.
 *
 * The MDX files are not the source of truth any more — the database is. This
 * converter exists to move them across once, faithfully, and to be run again
 * whenever an import needs repeating. It is deliberately strict: an element it
 * does not recognise is an error, never a silent omission, because a silent
 * omission is a paragraph that disappears from a published page and is noticed
 * months later.
 *
 * The block shapes here mirror `BlockKind` and `BlockDocumentProcessor` in the
 * API. The server validates everything again on write; this side only has to be
 * honest.
 */

import { unified } from 'unified'
import remarkParse from 'remark-parse'
import remarkGfm from 'remark-gfm'
import remarkMdx from 'remark-mdx'
import remarkFrontmatter from 'remark-frontmatter'
import { parse as parseYaml } from 'yaml'

/** Elements the studio's MDX actually uses. Anything else stops the import. */
const KNOWN_COMPONENTS = new Set(['Metrics', 'Decision', 'Figure', 'Note', 'Annotation'])

const processor = unified()
  .use(remarkParse)
  .use(remarkGfm)
  .use(remarkFrontmatter, ['yaml'])
  .use(remarkMdx)

export function parseMdx(source) {
  const tree = processor.parse(source)

  const yamlNode = tree.children.find((node) => node.type === 'yaml')
  const frontmatter = yamlNode ? parseYaml(yamlNode.value) : {}
  const body = tree.children.filter((node) => node.type !== 'yaml')

  return { frontmatter, body }
}

/* ------------------------------------------------------------------ blocks */

/**
 * Converts a list of mdast flow nodes into blocks.
 *
 * Consecutive prose (paragraphs and lists) merges into one rich-text block: the
 * editor treats a block as a unit of movement, and splitting every paragraph
 * into its own card would turn a case study into forty cards nobody can reorder
 * meaningfully.
 */
export function toBlocks(nodes, context) {
  const blocks = []
  let prose = []

  const flushProse = () => {
    if (prose.length === 0) return
    const html = prose.join('')
    prose = []
    if (html.trim().length > 0) blocks.push({ type: 'richText', html })
  }

  for (const node of nodes) {
    switch (node.type) {
      case 'paragraph':
        prose.push(`<p>${inlineHtml(node.children, context)}</p>`)
        break

      case 'list':
        prose.push(listHtml(node, context))
        break

      case 'blockquote':
        // The design has no block quote; its paragraphs read the same as prose.
        for (const child of node.children) {
          if (child.type === 'paragraph') prose.push(`<p>${inlineHtml(child.children, context)}</p>`)
          else throw fail(context, `blockquote berisi ${child.type}, belum ditangani`)
        }
        break

      case 'heading': {
        flushProse()
        const text = plainText(node.children).trim()
        if (text.length > 0) {
          blocks.push({ type: 'heading', level: node.depth <= 2 ? 2 : 3, text })
        }
        break
      }

      case 'code':
        flushProse()
        blocks.push({ type: 'codeBlock', language: node.lang ?? '', code: node.value })
        break

      case 'table':
        flushProse()
        blocks.push(tableBlock(node, context))
        break

      case 'thematicBreak':
        flushProse()
        break

      case 'mdxFlowExpression':
        // `{/* … */}` — an authoring comment, not content.
        break

      case 'mdxjsEsm':
        break

      case 'mdxJsxFlowElement': {
        flushProse()
        blocks.push(...componentBlocks(node, context))
        break
      }

      default:
        throw fail(context, `simpul mdast "${node.type}" belum ditangani`)
    }
  }

  flushProse()
  return blocks
}

function componentBlocks(node, context) {
  const name = node.name ?? ''
  if (!KNOWN_COMPONENTS.has(name)) {
    throw fail(context, `komponen <${name}> tidak dikenal — impor dihentikan agar tidak ada isi yang hilang diam-diam`)
  }

  const attributes = readAttributes(node, context)

  switch (name) {
    case 'Metrics': {
      const items = attributes.items
      if (!Array.isArray(items)) throw fail(context, '<Metrics> tanpa daftar items')
      return [
        {
          type: 'metrics',
          items: items.map((item) => ({
            value: String(item.value ?? ''),
            label: String(item.label ?? ''),
          })),
        },
      ]
    }

    case 'Figure':
      return [
        {
          type: 'figure',
          variant: String(attributes.variant ?? 'system'),
          alt: String(attributes.alt ?? ''),
          caption: String(attributes.caption ?? ''),
          // Resolved to an uploaded file by the importer; kept as the original
          // public path here so the converter stays free of network concerns.
          sourcePath: attributes.src ? String(attributes.src) : null,
        },
      ]

    case 'Note':
      return [{ type: 'note', html: childrenHtml(node.children, context) }]

    case 'Decision': {
      const body = toBlocks(node.children, context)
      return [
        {
          type: 'decision',
          step: Number(attributes.step ?? 0),
          title: String(attributes.title ?? ''),
          chose: String(attributes.chose ?? ''),
          because: String(attributes.because ?? ''),
          despite: String(attributes.despite ?? ''),
          body,
        },
      ]
    }

    case 'Annotation': {
      // The margin note is an attribute in MDX and a block of its own here. It
      // follows the passage it annotates, which is the closest the block model
      // gets to "beside it".
      const inner = toBlocks(node.children, context)
      const note = String(attributes.note ?? '').trim()
      return note.length > 0 ? [...inner, { type: 'note', html: `<p>${escapeHtml(note)}</p>` }] : inner
    }

    default:
      throw fail(context, `komponen <${name}> tidak ditangani`)
  }
}

function tableBlock(node, context) {
  const rows = node.children.map((row) =>
    row.children.map((cell) => plainText(cell.children).trim()),
  )

  if (rows.length === 0) throw fail(context, 'tabel kosong')

  const [head, ...body] = rows
  return { type: 'table', head, rows: body }
}

/* -------------------------------------------------------------------- html */

/** Block children rendered as the small HTML the server's allow-list accepts. */
function childrenHtml(children, context) {
  const parts = []

  for (const child of children) {
    if (child.type === 'paragraph') parts.push(`<p>${inlineHtml(child.children, context)}</p>`)
    else if (child.type === 'list') parts.push(listHtml(child, context))
    else if (child.type === 'text' && child.value.trim().length === 0) continue
    else throw fail(context, `isi "${child.type}" tidak bisa dijadikan HTML sederhana`)
  }

  return parts.join('')
}

function listHtml(node, context) {
  const tag = node.ordered ? 'ol' : 'ul'
  const items = node.children
    .map((item) => {
      const inner = item.children
        .map((child) => {
          if (child.type === 'paragraph') return inlineHtml(child.children, context)
          if (child.type === 'list') return listHtml(child, context)
          throw fail(context, `butir daftar berisi ${child.type}, belum ditangani`)
        })
        .join(' ')
      return `<li>${inner}</li>`
    })
    .join('')

  return `<${tag}>${items}</${tag}>`
}

function inlineHtml(children, context) {
  return children
    .map((node) => {
      switch (node.type) {
        case 'text':
          return escapeHtml(collapseSoftBreaks(node.value))
        case 'strong':
          return `<strong>${inlineHtml(node.children, context)}</strong>`
        case 'emphasis':
          return `<em>${inlineHtml(node.children, context)}</em>`
        case 'inlineCode':
          return `<code>${escapeHtml(node.value)}</code>`
        case 'link':
          return `<a href="${escapeAttribute(node.url)}">${inlineHtml(node.children, context)}</a>`
        case 'break':
          return '<br />'
        case 'delete':
          // No strikethrough in the design system; the words still matter.
          return inlineHtml(node.children, context)
        case 'mdxJsxTextElement':
          return inlineHtml(node.children ?? [], context)
        case 'image':
          return ''
        default:
          throw fail(context, `simpul inline "${node.type}" belum ditangani`)
      }
    })
    .join('')
}

/**
 * A newline inside a paragraph is a line wrap in the source file, not a line
 * break on the page. Keeping it would put ragged breaks into published prose.
 */
const collapseSoftBreaks = (value) => value.replace(/\s*\n\s*/g, ' ')

const escapeHtml = (value) =>
  value.replace(/&/g, '&amp;').replace(/</g, '&lt;').replace(/>/g, '&gt;')

const escapeAttribute = (value) => escapeHtml(value).replace(/"/g, '&quot;')

/* ------------------------------------------------------------- attributes */

function readAttributes(node, context) {
  const result = {}

  for (const attribute of node.attributes ?? []) {
    if (attribute.type !== 'mdxJsxAttribute') {
      throw fail(context, `atribut sebar (spread) pada <${node.name}> tidak didukung`)
    }

    const value = attribute.value

    if (value === null || typeof value === 'string') {
      result[attribute.name] = value ?? true
      continue
    }

    if (value?.type === 'mdxJsxAttributeValueExpression') {
      result[attribute.name] = evaluateExpression(value.value, context, node.name, attribute.name)
      continue
    }

    throw fail(context, `nilai atribut ${attribute.name} pada <${node.name}> tidak dikenal`)
  }

  return result
}

/**
 * Evaluates a JSX attribute expression — `{3}`, `{[{ value: '27' }]}`.
 *
 * These are literals written by hand in this repository's own content files, so
 * evaluating them is reading data, not running someone else's code. The import
 * is a local, one-off migration of files the author wrote.
 */
function evaluateExpression(expression, context, component, attribute) {
  try {
    return Function(`"use strict"; return (${expression});`)()
  } catch (error) {
    throw fail(context, `nilai ${attribute} pada <${component}> tidak bisa dibaca: ${error.message}`)
  }
}

/* --------------------------------------------------------- text extraction */

/**
 * Every piece of prose in the source, in the order the blocks will hold it.
 *
 * Component-aware on purpose: it reads the same attributes the converter turns
 * into content and ignores the ones that are structure (`src`, `variant`,
 * `step`). The margin note of an `<Annotation>` is emitted after its children,
 * because that is where the converter puts it — encoding the one deliberate
 * reordering rather than weakening the comparison to tolerate any reordering.
 */
export function mdastText(nodes) {
  const out = []
  for (const node of nodes) walkForText(node, out)
  return out
}

/** Which attributes of a component carry prose, and in what order. */
const PROSE_ATTRIBUTES = {
  Decision: ['title', 'chose', 'because', 'despite'],
  Figure: ['alt', 'caption'],
  Metrics: ['items'],
  Annotation: [],
  Note: [],
}

function walkForText(node, out) {
  if (!node || typeof node !== 'object') return

  if (node.type === 'text' || node.type === 'inlineCode' || node.type === 'code') {
    out.push(node.value)
    return
  }

  if (node.type === 'mdxJsxFlowElement' || node.type === 'mdxJsxTextElement') {
    const name = node.name ?? ''
    const attributes = new Map(
      (node.attributes ?? [])
        .filter((attribute) => attribute.type === 'mdxJsxAttribute')
        .map((attribute) => [attribute.name, attribute.value]),
    )

    for (const key of PROSE_ATTRIBUTES[name] ?? []) {
      if (attributes.has(key)) collectAttributeStrings(attributes.get(key), out)
    }

    for (const child of node.children ?? []) walkForText(child, out)

    // The one deliberate move: an annotation's note follows what it annotates.
    if (name === 'Annotation' && attributes.has('note')) {
      collectAttributeStrings(attributes.get('note'), out)
    }

    return
  }

  for (const child of node.children ?? []) walkForText(child, out)
}

function collectAttributeStrings(value, out) {
  if (typeof value === 'string') {
    out.push(value)
    return
  }

  if (value?.type === 'mdxJsxAttributeValueExpression') {
    collectValueStrings(Function(`"use strict"; return (${value.value});`)(), out)
  }
}

function collectValueStrings(value, out) {
  if (typeof value === 'string') out.push(value)
  else if (Array.isArray(value)) value.forEach((item) => collectValueStrings(item, out))
  else if (value && typeof value === 'object') Object.values(value).forEach((item) => collectValueStrings(item, out))
}

/** The same prose, read back out of the blocks that were produced. */
export function blockText(blocks) {
  const out = []

  for (const block of blocks) {
    switch (block.type) {
      case 'richText':
      case 'note':
        out.push(stripTags(block.html))
        break
      case 'heading':
        out.push(block.text)
        break
      case 'codeBlock':
        out.push(block.code)
        break
      case 'figure':
        out.push(block.alt, block.caption)
        break
      case 'metrics':
        block.items.forEach((item) => out.push(item.value, item.label))
        break
      case 'table':
        out.push(...block.head)
        block.rows.forEach((row) => out.push(...row))
        break
      case 'decision':
        out.push(block.title, block.chose, block.because, block.despite)
        out.push(...blockText(block.body ?? []))
        break
      default:
        break
    }
  }

  return out
}

const stripTags = (html) =>
  html
    .replace(/<[^>]*>/g, ' ')
    .replace(/&amp;/g, '&')
    .replace(/&lt;/g, '<')
    .replace(/&gt;/g, '>')
    .replace(/&quot;/g, '"')

/** Words, in order, with layout differences flattened away. */
export function words(fragments) {
  return fragments
    .join(' ')
    .replace(/\s+/g, ' ')
    .trim()
    .split(' ')
    .filter((word) => word.length > 0)
}

/* -------------------------------------------------------------- utilities */

function fail(context, message) {
  return new Error(`${context.file}: ${message}`)
}

export function plainText(nodes) {
  return nodes
    .map((node) => {
      if (node.type === 'text' || node.type === 'inlineCode') return node.value
      if (node.children) return plainText(node.children)
      return ''
    })
    .join('')
}
