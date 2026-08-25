import type { ReactNode } from 'react'
import { useId } from 'react'

import { FieldShell } from '@/components/panel/form/Field'
import { controlClasses } from '@/components/panel/form/control-styles'
import { Label } from '@/components/ui/Label'
import { cn } from '@/lib/cn'

type Common = {
  readonly label: string
  readonly hint?: string
  readonly error?: string
  readonly required?: boolean
  readonly disabled?: boolean
}

/* ------------------------------------------------------------------- text */

export function TextInput({
  label,
  hint,
  error,
  required,
  disabled,
  value,
  onChange,
  placeholder,
  maxLength,
  mono,
  autoFocus,
}: Common & {
  readonly value: string
  readonly onChange: (value: string) => void
  readonly placeholder?: string
  readonly maxLength?: number
  readonly mono?: boolean
  readonly autoFocus?: boolean
}) {
  const id = useId()

  return (
    <FieldShell
      id={id}
      label={label}
      hint={hint}
      error={error}
      required={required}
      count={maxLength ? { value: value.length, max: maxLength } : undefined}
    >
      <input
        id={id}
        type="text"
        value={value}
        disabled={disabled}
        autoFocus={autoFocus}
        placeholder={placeholder}
        aria-invalid={Boolean(error)}
        onChange={(event) => onChange(event.target.value)}
        className={controlClasses(Boolean(error), cn(mono && 'font-mono text-[0.9375rem]'))}
      />
    </FieldShell>
  )
}

/* --------------------------------------------------------------- textarea */

export function TextAreaInput({
  label,
  hint,
  error,
  required,
  disabled,
  value,
  onChange,
  placeholder,
  maxLength,
  rows = 4,
}: Common & {
  readonly value: string
  readonly onChange: (value: string) => void
  readonly placeholder?: string
  readonly maxLength?: number
  readonly rows?: number
}) {
  const id = useId()

  return (
    <FieldShell
      id={id}
      label={label}
      hint={hint}
      error={error}
      required={required}
      count={maxLength ? { value: value.length, max: maxLength } : undefined}
    >
      <textarea
        id={id}
        rows={rows}
        value={value}
        disabled={disabled}
        placeholder={placeholder}
        aria-invalid={Boolean(error)}
        onChange={(event) => onChange(event.target.value)}
        className={controlClasses(Boolean(error), 'resize-y leading-relaxed')}
      />
    </FieldShell>
  )
}

/* ----------------------------------------------------------------- number */

export function NumberInput({
  label,
  hint,
  error,
  required,
  disabled,
  value,
  onChange,
  min,
  max,
  suffix,
}: Common & {
  readonly value: number
  readonly onChange: (value: number) => void
  readonly min?: number
  readonly max?: number
  readonly suffix?: string
}) {
  const id = useId()

  return (
    <FieldShell id={id} label={label} hint={hint} error={error} required={required}>
      <div className="relative">
        <input
          id={id}
          type="number"
          inputMode="numeric"
          value={Number.isFinite(value) ? value : ''}
          min={min}
          max={max}
          disabled={disabled}
          aria-invalid={Boolean(error)}
          onChange={(event) => onChange(event.target.valueAsNumber)}
          className={controlClasses(Boolean(error), cn('numeric', suffix && 'pr-20'))}
        />
        {suffix ? (
          <Label as="span" className="absolute top-1/2 right-4 -translate-y-1/2">
            {suffix}
          </Label>
        ) : null}
      </div>
    </FieldShell>
  )
}

/* ----------------------------------------------------------------- select */

export function SelectInput<T extends string>({
  label,
  hint,
  error,
  required,
  disabled,
  value,
  onChange,
  options,
}: Common & {
  readonly value: T
  readonly onChange: (value: T) => void
  readonly options: readonly { readonly value: T; readonly label: string }[]
}) {
  const id = useId()

  return (
    <FieldShell id={id} label={label} hint={hint} error={error} required={required}>
      <select
        id={id}
        value={value}
        disabled={disabled}
        aria-invalid={Boolean(error)}
        onChange={(event) => onChange(event.target.value as T)}
        className={controlClasses(Boolean(error), 'appearance-none pr-10')}
      >
        {options.map((option) => (
          <option key={option.value} value={option.value}>
            {option.label}
          </option>
        ))}
      </select>
    </FieldShell>
  )
}

/* --------------------------------------------------------------- segmented */

/**
 * A small closed set, shown all at once.
 *
 * Preferred over a dropdown when there are two or three options and the choice
 * carries meaning — an editor should be able to see what the alternatives are
 * without opening anything.
 */
export function SegmentedInput<T extends string>({
  label,
  hint,
  error,
  value,
  onChange,
  options,
}: Common & {
  readonly value: T
  readonly onChange: (value: T) => void
  readonly options: readonly { readonly value: T; readonly label: string }[]
}) {
  const name = useId()

  return (
    // Same four-row shape as FieldShell, so a segmented control can share a
    // `FieldRow` with an ordinary input and still line up.
    <fieldset className="field">
      <legend className="font-semibold">{label}</legend>
      <p className={cn('type-small text-muted', hint && 'mt-1')}>{hint}</p>

      <div className="border-line mt-1.5 flex w-fit rounded-[var(--radius-control)] border">
        {options.map((option) => {
          const active = option.value === value
          return (
            <label
              key={option.value}
              className={cn(
                'type-label flex min-h-9 cursor-pointer items-center px-3',
                'transition-colors duration-150 ease-out',
                'first:rounded-l-[3px] last:rounded-r-[3px]',
                'has-[:focus-visible]:outline has-[:focus-visible]:outline-2',
                'has-[:focus-visible]:outline-accent has-[:focus-visible]:outline-offset-2',
                active ? 'bg-accent-soft text-accent' : 'text-muted hover:text-text',
              )}
            >
              <input
                type="radio"
                name={name}
                value={option.value}
                checked={active}
                onChange={() => onChange(option.value)}
                className="sr-only"
              />
              {option.label}
            </label>
          )
        })}
      </div>

      <p className="type-small text-danger min-h-5 pt-0.5" aria-live="polite">
        {error}
      </p>
    </fieldset>
  )
}

/* ---------------------------------------------------------------- switch */

export function SwitchInput({
  label,
  hint,
  value,
  onChange,
  disabled,
}: {
  readonly label: string
  readonly hint?: string
  readonly value: boolean
  readonly onChange: (value: boolean) => void
  readonly disabled?: boolean
}) {
  const id = useId()

  return (
    <div className="flex items-start gap-4">
      <button
        id={id}
        type="button"
        role="switch"
        aria-checked={value}
        disabled={disabled}
        onClick={() => onChange(!value)}
        className={cn(
          'mt-0.5 h-6 w-11 shrink-0 rounded-full border transition-colors duration-150 ease-out',
          'focus-visible:outline-accent focus-visible:outline focus-visible:outline-2 focus-visible:outline-offset-2',
          value ? 'border-accent bg-accent' : 'border-muted bg-surface',
        )}
      >
        <span
          aria-hidden="true"
          className={cn(
            'block h-4 w-4 rounded-full transition-transform duration-150 ease-out',
            value ? 'bg-accent-fg translate-x-[1.4rem]' : 'bg-muted translate-x-[0.2rem]',
          )}
        />
      </button>

      <label htmlFor={id} className="cursor-pointer">
        <span className="block font-semibold">{label}</span>
        {hint ? <span className="type-small text-muted block">{hint}</span> : null}
      </label>
    </div>
  )
}

/* ------------------------------------------------------------------ misc */

/** A read-only value with a label — for things the panel shows but never edits. */
export function ReadOnlyField({
  label,
  children,
}: {
  readonly label: string
  readonly children: ReactNode
}) {
  return (
    <div className="border-line border-t pt-3">
      <Label as="p">{label}</Label>
      <div className="type-small mt-1.5">{children}</div>
    </div>
  )
}
