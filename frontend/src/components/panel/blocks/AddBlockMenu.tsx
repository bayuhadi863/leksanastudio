import { useEffect, useRef, useState } from 'react'

import { Button } from '@/components/ui/Button'
import { cn } from '@/lib/cn'
import { BLOCK_META, type BlockKind } from '@/types/blocks'

type Props = {
  readonly kinds: readonly BlockKind[]
  readonly onAdd: (kind: BlockKind) => void
  readonly disabled?: boolean
}

/**
 * What can be added, and what each one is for.
 *
 * Every entry carries a one-line purpose rather than only a name. An editor
 * choosing between "Catatan pinggir" and "Teks" should not have to add one to
 * find out which is which.
 */
export function AddBlockMenu({ kinds, onAdd, disabled }: Props) {
  const [open, setOpen] = useState(false)
  const containerRef = useRef<HTMLDivElement>(null)

  useEffect(() => {
    if (!open) return

    const onPointerDown = (event: MouseEvent) => {
      if (!containerRef.current?.contains(event.target as Node)) setOpen(false)
    }
    const onKeyDown = (event: KeyboardEvent) => {
      if (event.key === 'Escape') setOpen(false)
    }

    document.addEventListener('mousedown', onPointerDown)
    document.addEventListener('keydown', onKeyDown)
    return () => {
      document.removeEventListener('mousedown', onPointerDown)
      document.removeEventListener('keydown', onKeyDown)
    }
  }, [open])

  return (
    <div ref={containerRef} className="relative">
      <Button
        type="button"
        variant="secondary"
        disabled={disabled}
        onClick={() => setOpen((value) => !value)}
        className="w-full sm:w-auto"
      >
        + Tambah blok
      </Button>

      {open ? (
        <div
          role="menu"
          className="border-line bg-bg absolute bottom-full left-0 z-30 mb-2 w-full min-w-80 overflow-hidden rounded-[var(--radius-control)] border shadow-[0_1px_2px_var(--shadow-near),0_8px_24px_var(--shadow-far)] sm:w-96"
        >
          <ul className="max-h-96 overflow-y-auto">
            {kinds.map((kind) => (
              <li key={kind}>
                <button
                  type="button"
                  role="menuitem"
                  onClick={() => {
                    onAdd(kind)
                    setOpen(false)
                  }}
                  className={cn(
                    'border-line hover:bg-surface w-full border-b px-4 py-3 text-left last:border-b-0',
                    'transition-colors duration-150 ease-out',
                    'focus-visible:bg-surface focus-visible:outline-none',
                  )}
                >
                  <span className="block font-semibold">{BLOCK_META[kind].label}</span>
                  <span className="type-small text-muted block">{BLOCK_META[kind].hint}</span>
                </button>
              </li>
            ))}
          </ul>
        </div>
      ) : null}
    </div>
  )
}
