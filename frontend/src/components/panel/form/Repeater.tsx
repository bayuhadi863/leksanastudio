import type { ReactNode } from 'react'

import { Button } from '@/components/ui/Button'
import { Label } from '@/components/ui/Label'
import { cn } from '@/lib/cn'

type Props<T> = {
  readonly label: string
  readonly hint?: string
  readonly items: readonly T[]
  readonly onChange: (items: T[]) => void
  readonly create: () => T
  readonly renderItem: (item: T, update: (next: T) => void, index: number) => ReactNode
  /** One-line description of an item when collapsed or empty. */
  readonly itemLabel?: (item: T, index: number) => string
  readonly addLabel?: string
  /** Fixed-length lists — metrics are always three — hide add and remove entirely. */
  readonly fixedLength?: boolean
  readonly max?: number
  readonly error?: string
}

/**
 * A list of the same shape, repeated.
 *
 * Ordering uses arrows rather than dragging, for the same reason the block
 * editor does: dragging has to be built correctly for touch, keyboard, and
 * auto-scroll before it is better than two buttons, and until then it is worse.
 */
export function Repeater<T>({
  label,
  hint,
  items,
  onChange,
  create,
  renderItem,
  itemLabel,
  addLabel = 'Tambah',
  fixedLength,
  max,
  error,
}: Props<T>) {
  const update = (index: number, next: T) => {
    const copy = [...items]
    copy[index] = next
    onChange(copy)
  }

  const move = (index: number, direction: -1 | 1) => {
    const target = index + direction
    if (target < 0 || target >= items.length) return

    const copy = [...items]
    const [moved] = copy.splice(index, 1)
    copy.splice(target, 0, moved!)
    onChange(copy)
  }

  const remove = (index: number) => onChange(items.filter((_, i) => i !== index))

  const atMax = max !== undefined && items.length >= max

  return (
    <div>
      <div className="flex items-baseline justify-between gap-4">
        <span className="font-semibold">{label}</span>
        {max !== undefined ? (
          <Label as="span" className={cn(atMax && 'text-accent')}>
            {items.length}/{max}
          </Label>
        ) : null}
      </div>

      {hint ? <p className="type-small text-muted mt-1">{hint}</p> : null}

      <ul className="mt-3 grid gap-3">
        {items.map((item, index) => (
          <li
            key={index}
            className="border-line bg-surface rounded-[var(--radius-control)] border p-4"
          >
            <div className="mb-3 flex items-center justify-between gap-4">
              <Label as="span">{itemLabel?.(item, index) ?? `${index + 1}`}</Label>

              {fixedLength ? null : (
                <div className="flex items-center gap-1">
                  <IconButton
                    label="Naikkan"
                    disabled={index === 0}
                    onClick={() => move(index, -1)}
                  >
                    ↑
                  </IconButton>
                  <IconButton
                    label="Turunkan"
                    disabled={index === items.length - 1}
                    onClick={() => move(index, 1)}
                  >
                    ↓
                  </IconButton>
                  <IconButton label="Hapus" danger onClick={() => remove(index)}>
                    ✕
                  </IconButton>
                </div>
              )}
            </div>

            <div className="grid gap-4">{renderItem(item, (next) => update(index, next), index)}</div>
          </li>
        ))}
      </ul>

      {items.length === 0 ? (
        <p className="type-small text-muted border-line mt-3 border-t pt-3">Belum ada isi.</p>
      ) : null}

      {fixedLength ? null : (
        <div className="mt-4">
          <Button
            type="button"
            variant="secondary"
            disabled={atMax}
            onClick={() => onChange([...items, create()])}
          >
            + {addLabel}
          </Button>
        </div>
      )}

      <p className="type-small text-danger min-h-6 pt-1" aria-live="polite">
        {error}
      </p>
    </div>
  )
}

/**
 * A square, quiet control for row actions.
 *
 * Kept to the 44px touch target even though it reads small — these are the
 * controls most likely to be used on a phone, and a cramped arrow is a missed tap.
 */
export function IconButton({
  children,
  label,
  onClick,
  disabled,
  danger,
  className,
}: {
  readonly children: ReactNode
  readonly label: string
  readonly onClick: () => void
  readonly disabled?: boolean
  readonly danger?: boolean
  readonly className?: string
}) {
  return (
    <button
      type="button"
      onClick={onClick}
      disabled={disabled}
      title={label}
      className={cn(
        'flex h-9 min-h-9 w-9 items-center justify-center rounded-[var(--radius-control)]',
        'transition-colors duration-150 ease-out',
        'focus-visible:outline-accent focus-visible:outline focus-visible:outline-2 focus-visible:outline-offset-1',
        disabled
          ? 'text-muted cursor-not-allowed opacity-35'
          : danger
            ? 'text-muted hover:text-danger hover:bg-bg'
            : 'text-muted hover:text-accent hover:bg-bg',
        className,
      )}
    >
      <span aria-hidden="true">{children}</span>
      <span className="sr-only">{label}</span>
    </button>
  )
}

/** A repeater of plain strings — deliverables, exclusions, stack. */
export function StringRepeater({
  label,
  hint,
  items,
  onChange,
  placeholder,
  addLabel = 'Tambah baris',
  max,
}: {
  readonly label: string
  readonly hint?: string
  readonly items: readonly string[]
  readonly onChange: (items: string[]) => void
  readonly placeholder?: string
  readonly addLabel?: string
  readonly max?: number
}) {
  return (
    <Repeater
      label={label}
      hint={hint}
      items={items}
      onChange={onChange}
      create={() => ''}
      addLabel={addLabel}
      max={max}
      itemLabel={(_, index) => `Baris ${index + 1}`}
      renderItem={(item, update) => (
        <input
          type="text"
          value={item}
          placeholder={placeholder}
          onChange={(event) => update(event.target.value)}
          className="border-muted bg-bg focus:border-accent focus:ring-accent/20 w-full rounded-[var(--radius-control)] border px-3 py-2.5 transition-colors duration-150 ease-out focus:ring-3 focus:outline-none"
        />
      )}
    />
  )
}
