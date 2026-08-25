import { useCallback, useEffect, useRef, useState, type ReactNode } from 'react'

import { PreviewFrame } from '@/components/panel/preview/PreviewFrame'
import { Label } from '@/components/ui/Label'
import { cn } from '@/lib/cn'

export type PreviewWidth = 'penuh' | 'hp'

const WIDTHS: readonly { readonly value: PreviewWidth; readonly label: string }[] = [
  { value: 'penuh', label: 'Penuh' },
  { value: 'hp', label: 'HP' },
]

/** The phone this site is actually read on. */
const PHONE_WIDTH = '390px'

type Props = {
  readonly children: ReactNode
  /** Block currently being edited; the preview scrolls to it. */
  readonly focusBlockId?: string | null
  /** True while the form holds changes the site has not been told about yet. */
  readonly dirty?: boolean
  readonly className?: string
}

/**
 * The live preview surface.
 *
 * The page renders inside an iframe, and that is the whole design. A preview
 * that shares the panel's viewport cannot answer the one question a width
 * switch is asked — *does this work on a phone?* — because every media query
 * and every `vw` in the stylesheet would still be reading 1440px. The frame
 * gives the sheet a viewport of its own, so 390px means 390px.
 *
 * It also follows the cursor: editing block twenty-two and seeing block one is
 * not a preview, it is a second thing to scroll. The matching block is brought
 * into view when focus moves — and only when it is genuinely out of sight,
 * because a preview that jumps while you type is worse than one that sits still.
 */
export function PreviewPane({ children, focusBlockId, dirty, className }: Props) {
  const [width, setWidth] = useState<PreviewWidth>('penuh')
  const frameDocRef = useRef<Document | null>(null)

  const handleFrameReady = useCallback((document: Document | null) => {
    frameDocRef.current = document
  }, [])

  useEffect(() => {
    if (!focusBlockId) return

    const doc = frameDocRef.current
    const target = doc?.querySelector<HTMLElement>(`[data-preview-block="${focusBlockId}"]`)
    const scroller = doc?.scrollingElement
    if (!doc || !target || !scroller) return

    const box = target.getBoundingClientRect()
    const viewport = doc.documentElement.clientHeight
    if (box.top >= 0 && box.bottom <= viewport) return

    scroller.scrollTo({ top: scroller.scrollTop + box.top - 24, behavior: 'smooth' })
  }, [focusBlockId])

  return (
    <aside className={cn('flex min-h-0 flex-col', className)} aria-label="Pratinjau">
      <header className="border-line flex items-center justify-between gap-4 border-b pb-2">
        <div className="flex items-baseline gap-3">
          <Label as="p">Pratinjau</Label>
          <span className={cn('type-small', dirty ? 'text-accent' : 'text-muted')}>
            {dirty ? 'Belum disimpan' : 'Sama dengan yang tersimpan'}
          </span>
        </div>

        <fieldset className="flex items-center gap-2">
          <legend className="sr-only">Lebar pratinjau</legend>
          <div className="border-line flex rounded-[var(--radius-control)] border">
            {WIDTHS.map((option) => (
              <label
                key={option.value}
                className={cn(
                  'type-label flex min-h-9 cursor-pointer items-center px-3',
                  'transition-colors duration-150 ease-out',
                  'first:rounded-l-[3px] last:rounded-r-[3px]',
                  'has-[:focus-visible]:outline has-[:focus-visible]:outline-2',
                  'has-[:focus-visible]:outline-accent has-[:focus-visible]:outline-offset-2',
                  width === option.value
                    ? 'bg-accent-soft text-accent'
                    : 'text-muted hover:text-text',
                )}
              >
                <input
                  type="radio"
                  name="preview-width"
                  value={option.value}
                  checked={width === option.value}
                  onChange={() => setWidth(option.value)}
                  className="sr-only"
                />
                {option.label}
              </label>
            ))}
          </div>
        </fieldset>
      </header>

      <div className="mt-3 flex min-h-0 flex-1 justify-center">
        {/*
          The sheet is outlined, not bordered. A border — on the frame or on this
          wrapper — takes two pixels out of the frame's viewport, and 390px has
          to mean 390px or the phone check is wrong in the direction that hides
          bugs. An outline draws outside the box and costs nothing.
        */}
        <div
          className="outline-line bg-bg h-full min-h-[16rem] outline outline-1"
          style={{ width: width === 'hp' ? PHONE_WIDTH : '100%' }}
        >
          <PreviewFrame width="100%" onFrameReady={handleFrameReady} className="block h-full">
            {children}
          </PreviewFrame>
        </div>
      </div>
    </aside>
  )
}
