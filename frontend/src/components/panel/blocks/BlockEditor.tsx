import { useMemo, useState } from 'react'

import { AddBlockMenu } from '@/components/panel/blocks/AddBlockMenu'
import { BlockForm } from '@/components/panel/blocks/BlockForm'
import { IconButton } from '@/components/panel/form/Repeater'
import { Label } from '@/components/ui/Label'
import { cn } from '@/lib/cn'
import {
  BLOCK_META,
  blockSummary,
  createBlock,
  type Block,
  type BlockKind,
  type BlockLimits,
} from '@/types/blocks'

type Props = {
  readonly value: readonly Block[]
  readonly onChange: (blocks: Block[]) => void
  readonly kinds: readonly BlockKind[]
  readonly limits: BlockLimits
}

/**
 * The body editor.
 *
 * A vertical stack of typed cards, ordered with arrows rather than dragging —
 * a deliberate v1 choice. Dragging has to be correct for touch, for the
 * keyboard, and while auto-scrolling before it beats two buttons; until then it
 * is slower and less reliable, and it would have cost the week this editor
 * needed for everything else.
 *
 * What long documents actually need is not dragging but collapsing, so that is
 * what is here: every card folds to one line, and the whole document folds at
 * once.
 */
/**
 * Above this, a document opens folded.
 *
 * Chosen against the real content: an imported case study runs to thirty-six
 * blocks, and opening it expanded puts the save bar thirty thousand pixels below
 * the title. A short document still opens ready to type in.
 */
const COLLAPSE_ON_OPEN_ABOVE = 8

export function BlockEditor({ value, onChange, kinds, limits }: Props) {
  const [collapsed, setCollapsed] = useState<ReadonlySet<string>>(
    () =>
      new Set(value.length > COLLAPSE_ON_OPEN_ABOVE ? value.map((block) => block.id) : []),
  )

  const noteCount = useMemo(
    () => value.filter((block) => block.type === 'note').length,
    [value],
  )

  const atLimit = value.length >= limits.maxBlocksPerDocument
  const notesOverBudget = noteCount > limits.maxNotesPerDocument

  const toggle = (id: string) =>
    setCollapsed((current) => {
      const next = new Set(current)
      if (next.has(id)) next.delete(id)
      else next.add(id)
      return next
    })

  const collapseAll = () => setCollapsed(new Set(value.map((block) => block.id)))
  const expandAll = () => setCollapsed(new Set())

  const update = (index: number, block: Block) => {
    const next = [...value]
    next[index] = block
    onChange(next)
  }

  const move = (index: number, direction: -1 | 1) => {
    const target = index + direction
    if (target < 0 || target >= value.length) return
    const next = [...value]
    const [moved] = next.splice(index, 1)
    next.splice(target, 0, moved!)
    onChange(next)
  }

  const remove = (index: number) => onChange(value.filter((_, i) => i !== index))

  const add = (kind: BlockKind) => {
    const block = createBlock(kind, limits.metricsCount)
    onChange([...value, block])
    // A new block opens: adding something and having it appear folded is the
    // kind of surprise that makes people click twice for everything afterwards.
    setCollapsed((current) => {
      const next = new Set(current)
      next.delete(block.id)
      return next
    })
  }

  return (
    <div>
      <div className="border-line flex flex-wrap items-baseline justify-between gap-x-6 gap-y-2 border-b pb-2">
        <div className="flex items-baseline gap-5">
          <Label as="span">
            {value.length} blok
            {atLimit ? ` — batas ${limits.maxBlocksPerDocument}` : ''}
          </Label>

          <Label as="span" className={cn(notesOverBudget && 'text-danger')}>
            Catatan pinggir {noteCount}/{limits.maxNotesPerDocument}
          </Label>
        </div>

        {value.length > 1 ? (
          <div className="flex items-center gap-4">
            <button
              type="button"
              onClick={collapseAll}
              className="type-label text-muted hover:text-accent"
            >
              Lipat semua
            </button>
            <button
              type="button"
              onClick={expandAll}
              className="type-label text-muted hover:text-accent"
            >
              Buka semua
            </button>
          </div>
        ) : null}
      </div>

      {notesOverBudget ? (
        <p className="type-small text-danger border-danger mt-4 border-l-2 pl-3">
          Lebih dari {limits.maxNotesPerDocument} catatan pinggir. Di atas itu catatan pinggir
          berhenti terbaca sebagai catatan, dan tulisan ini akan ditolak saat disimpan.
        </p>
      ) : null}

      {value.length === 0 ? (
        <div className="border-line mt-6 border-b border-dashed py-10 text-center">
          <p className="text-muted max-w-md mx-auto">
            Tulisan dibangun dari blok. Mulai dari <strong className="text-text">Teks</strong> untuk
            paragraf biasa, lalu tambahkan <strong className="text-text">Keputusan</strong>,{' '}
            <strong className="text-text">Metrik</strong>, atau{' '}
            <strong className="text-text">Gambar</strong> di tempat yang tepat.
          </p>
        </div>
      ) : (
        // `grid-cols-1` here and `min-w-0` on the card are load-bearing, not
        // decoration: a grid track and a grid item both take their automatic
        // minimum from their content, and the collapsed summary below is
        // `whitespace-nowrap`. Without them one long paragraph widens the card,
        // the card widens the track, and the whole page grows a scrollbar.
        <ol className="mt-4 grid grid-cols-1 gap-2">
          {value.map((block, index) => {
            const isCollapsed = collapsed.has(block.id)

            return (
              <li
                key={block.id}
                data-block-id={block.id}
                className="border-line bg-surface min-w-0 rounded-[var(--radius-control)] border"
              >
                <div className="flex items-start gap-2.5 px-3 py-2">
                  <button
                    type="button"
                    onClick={() => toggle(block.id)}
                    aria-expanded={!isCollapsed}
                    className="flex min-w-0 flex-1 items-start gap-3 text-left"
                  >
                    <span
                      aria-hidden="true"
                      className={cn(
                        'text-muted mt-0.5 shrink-0 transition-transform duration-150 ease-out',
                        isCollapsed ? '' : 'rotate-90',
                      )}
                    >
                      ›
                    </span>

                    <span className="min-w-0 flex-1">
                      <Label as="span">
                        {BLOCK_META[block.type].label}
                        {block.type === 'decision' ? ` ${block.step}` : ''}
                      </Label>
                      {isCollapsed ? (
                        <span className="type-small text-muted mt-0.5 block truncate">
                          {blockSummary(block)}
                        </span>
                      ) : null}
                    </span>
                  </button>

                  <div className="flex shrink-0 items-center gap-0.5">
                    <IconButton
                      label={`Naikkan blok ${index + 1}`}
                      disabled={index === 0}
                      onClick={() => move(index, -1)}
                    >
                      ↑
                    </IconButton>
                    <IconButton
                      label={`Turunkan blok ${index + 1}`}
                      disabled={index === value.length - 1}
                      onClick={() => move(index, 1)}
                    >
                      ↓
                    </IconButton>
                    <IconButton
                      label={`Hapus blok ${index + 1}`}
                      danger
                      onClick={() => remove(index)}
                    >
                      ✕
                    </IconButton>
                  </div>
                </div>

                {isCollapsed ? null : (
                  <div className="border-line bg-bg border-t px-3 py-3">
                    <BlockForm
                      block={block}
                      limits={limits}
                      onChange={(next) => update(index, next)}
                    />
                  </div>
                )}
              </li>
            )
          })}
        </ol>
      )}

      <div className="mt-4">
        <AddBlockMenu kinds={kinds} onAdd={add} disabled={atLimit} />
      </div>
    </div>
  )
}
