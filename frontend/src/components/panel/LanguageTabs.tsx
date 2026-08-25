import { cn } from '@/lib/cn'
import type { ContentStatus, LocaleDTO } from '@/types/content'

export type LanguageTabState = {
  /** False when this entry has no translation in this language yet. */
  readonly enabled: boolean
  readonly status: ContentStatus
  readonly hasError?: boolean
}

type Props = {
  readonly locales: readonly LocaleDTO[]
  readonly active: string
  readonly onChange: (localeCode: string) => void
  readonly state: (localeCode: string) => LanguageTabState
}

/**
 * Which language you are editing.
 *
 * Tabs rather than a dropdown, and every language always visible — including
 * the ones with nothing in them. A half-translated site fails quietly; the one
 * place it must be loud is the screen where the translation would be written.
 *
 * Renders even when only one language is active. The row costs a line and makes
 * the second language a fact of the interface rather than a rebuild.
 */
export function LanguageTabs({ locales, active, onChange, state }: Props) {
  return (
    <div
      role="tablist"
      aria-label="Bahasa"
      className="border-line -mb-px flex flex-wrap gap-x-1 border-b"
    >
      {locales.map((locale) => {
        const { enabled, status, hasError } = state(locale.code)
        const selected = locale.code === active

        return (
          <button
            key={locale.code}
            role="tab"
            type="button"
            aria-selected={selected}
            onClick={() => onChange(locale.code)}
            title={locale.name}
            className={cn(
              'type-label flex min-h-11 items-center gap-2 border-b-2 px-4 transition-colors duration-150 ease-out',
              selected
                ? 'border-accent text-accent'
                : 'text-muted hover:text-text border-transparent',
            )}
          >
            <span
              aria-hidden="true"
              className={cn(
                'h-1.5 w-1.5 shrink-0 rounded-full',
                hasError
                  ? 'bg-danger'
                  : !enabled
                    ? 'border-muted border'
                    : status === 'Published'
                      ? 'bg-accent'
                      : 'border-muted border',
              )}
            />

            {locale.code.toUpperCase()}

            <span className="sr-only">
              {' — '}
              {locale.nativeName}
              {hasError
                ? ', ada isian yang perlu diperbaiki'
                : !enabled
                  ? ', belum ada terjemahan'
                  : status === 'Published'
                    ? ', terbit'
                    : ', draf'}
            </span>

            {enabled ? null : (
              <span className="text-muted normal-case">belum ada</span>
            )}
          </button>
        )
      })}
    </div>
  )
}
