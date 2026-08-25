import type { ComponentPropsWithoutRef, ReactNode } from 'react'
import { forwardRef } from 'react'

import { cn } from '@/lib/cn'

type Props = Omit<ComponentPropsWithoutRef<'input'>, 'id'> & {
  readonly id: string
  readonly label: string
  readonly error?: string
  /** Absolutely-positioned control inside the field — a reveal toggle, a unit. */
  readonly adornment?: ReactNode
  readonly hint?: string
}

/**
 * One labelled input.
 *
 * The error line reserves its height whether or not it has text, so a message
 * appearing never pushes the rest of the form down under the reader's cursor.
 */
export const TextField = forwardRef<HTMLInputElement, Props>(function TextField(
  { id, label, error, adornment, hint, className, ...rest },
  ref,
) {
  const errorId = `${id}-error`
  const hintId = `${id}-hint`

  return (
    <div>
      <label htmlFor={id} className="block font-semibold">
        {label}
      </label>

      {hint ? (
        <p id={hintId} className="type-small text-muted mt-1">
          {hint}
        </p>
      ) : null}

      <div className="relative mt-2">
        <input
          id={id}
          ref={ref}
          aria-invalid={Boolean(error)}
          aria-describedby={error ? errorId : hint ? hintId : undefined}
          className={cn(
            'bg-surface w-full rounded-[var(--radius-control)] border px-4 py-3',
            'transition-colors duration-150 ease-out',
            'focus:border-accent focus:ring-accent/20 focus:ring-3 focus:outline-none',
            error ? 'border-danger' : 'border-muted',
            className,
          )}
          {...rest}
        />
        {adornment}
      </div>

      <p id={errorId} className="type-small text-danger min-h-6 pt-1" aria-live="polite">
        {error}
      </p>
    </div>
  )
})
