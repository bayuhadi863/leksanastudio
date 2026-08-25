import type { ReactNode } from 'react'

import { cn } from '@/lib/cn'

export type FieldShellProps = {
  readonly id: string
  readonly label: string
  readonly hint?: string
  readonly error?: string
  readonly required?: boolean
  /** Current length and its ceiling. Rendered as a counter that warns before it refuses. */
  readonly count?: { readonly value: number; readonly max: number }
  readonly children: ReactNode
}

/**
 * Label, hint, control, counter, error — in that order, every time.
 *
 * The error line reserves its height whether or not it has text, so a message
 * appearing never shifts the page under the cursor. Small, and the difference
 * between a form that feels solid and one that twitches.
 */
export function FieldShell({
  id,
  label,
  hint,
  error,
  required,
  count,
  children,
}: FieldShellProps) {
  const hintId = hint ? `${id}-hint` : undefined
  const errorId = `${id}-error`

  return (
    // Four children, always, in this order: label · hint · control · error.
    // `FieldRow` lines fields up by handing those four to a subgrid, and a
    // conditional element would shift every following row out of step — so the
    // hint keeps its place even when it has nothing to say.
    <div className="field">
      <div className="flex items-baseline justify-between gap-4">
        <label htmlFor={id} className="block font-semibold">
          {label}
          {required ? (
            <span className="text-accent ml-1" aria-hidden="true">
              *
            </span>
          ) : null}
        </label>

        {count ? <CharacterCount value={count.value} max={count.max} /> : null}
      </div>

      <p id={hintId} className={cn('type-small text-muted', hint && 'mt-0.5')}>
        {hint}
      </p>

      <div className="mt-1.5">{children}</div>

      <p id={errorId} className="type-small text-danger min-h-5 pt-0.5" aria-live="polite">
        {error}
      </p>
    </div>
  )
}

/**
 * How much room is left.
 *
 * Silent until 80%, then muted-to-accent, then danger past the limit. A counter
 * that shouts from the first character trains people to ignore it.
 */
export function CharacterCount({
  value,
  max,
}: {
  readonly value: number
  readonly max: number
}) {
  const ratio = max > 0 ? value / max : 0
  if (ratio < 0.8) {
    return (
      <span className="type-label text-muted numeric opacity-0 transition-opacity duration-150">
        {value}/{max}
      </span>
    )
  }

  return (
    <span
      className={cn(
        'type-label numeric transition-colors duration-150',
        value > max ? 'text-danger' : 'text-accent',
      )}
    >
      {value}/{max}
    </span>
  )
}
