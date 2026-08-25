import { useEffect, useId, useState } from 'react'

import { cn } from '@/lib/cn'
import {
  applyTheme,
  readStoredTheme,
  storeTheme,
  THEME_LABEL,
  THEMES,
  type Theme,
} from '@/lib/theme'

/**
 * Reading-mode control.
 *
 * Radio inputs, not buttons: three mutually exclusive options is exactly what
 * a radio group is for, and it brings arrow-key navigation and the right
 * announcement for free.
 *
 * State starts as `null` so the server-rendered markup and the first client
 * render agree — the inline script in <head> has already painted the correct
 * theme by then, and the effect below only catches the control up to it.
 */
export function ThemeSwitch({
  className,
  layout = 'inline',
}: {
  readonly className?: string
  /**
   * `inline` sits in a footer row, label beside the control. `stacked` fills a
   * narrow column — the panel rail — with the label above it, because at 18rem
   * the three options need every pixel of the width they can get.
   */
  readonly layout?: 'inline' | 'stacked'
}) {
  const groupName = useId()
  const [theme, setTheme] = useState<Theme | null>(null)
  const stacked = layout === 'stacked'

  useEffect(() => {
    setTheme(readStoredTheme())
  }, [])

  const choose = (next: Theme) => {
    setTheme(next)
    applyTheme(next)
    storeTheme(next)
  }

  return (
    <fieldset className={cn(stacked ? 'block' : 'flex items-center gap-3', className)}>
      <legend className="sr-only">Tampilan</legend>

      <span
        aria-hidden="true"
        className={cn(
          'type-label text-muted',
          stacked ? 'mb-2 block' : 'hidden sm:inline',
        )}
      >
        Tampilan
      </span>

      <div
        className={cn(
          'border-line flex rounded-[var(--radius-control)] border',
          stacked && 'w-full',
        )}
      >
        {THEMES.map((option) => (
          <label
            key={option}
            className={cn(
              'type-label flex min-h-11 cursor-pointer items-center',
              stacked ? 'flex-1 justify-center px-1.5' : 'px-3.5',
              'transition-colors duration-150 ease-out',
              'first:rounded-l-[3px] last:rounded-r-[3px]',
              'has-[:focus-visible]:outline has-[:focus-visible]:outline-2',
              'has-[:focus-visible]:outline-accent has-[:focus-visible]:outline-offset-2',
              theme === option ? 'bg-accent-soft text-accent' : 'text-muted hover:text-text',
            )}
          >
            <input
              type="radio"
              name={groupName}
              value={option}
              checked={theme === option}
              onChange={() => choose(option)}
              className="sr-only"
            />
            {THEME_LABEL[option]}
          </label>
        ))}
      </div>
    </fieldset>
  )
}
