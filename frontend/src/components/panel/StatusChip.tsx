import { cn } from '@/lib/cn'
import type { ContentStatus, TranslationSummary } from '@/types/content'

/**
 * Whether one language of one entry is live.
 *
 * A filled mark for published, a hollow one for draft — readable without colour,
 * which matters because "is this live?" is the question an editor asks most and
 * must never have to guess at.
 */
export function StatusChip({
  status,
  label,
  className,
}: {
  readonly status: ContentStatus
  readonly label: string
  readonly className?: string
}) {
  const published = status === 'Published'

  return (
    <span
      className={cn(
        'type-label inline-flex items-center gap-1.5 whitespace-nowrap',
        published ? 'text-accent' : 'text-muted',
        className,
      )}
      title={published ? 'Terbit' : 'Draf — belum tampil di situs'}
    >
      <span
        aria-hidden="true"
        className={cn(
          'h-1.5 w-1.5 shrink-0 rounded-full',
          published ? 'bg-accent' : 'border-muted border',
        )}
      />
      {label}
      <span className="sr-only">{published ? ' — terbit' : ' — draf'}</span>
    </span>
  )
}

/**
 * The whole language row for one entry.
 *
 * Shown in every list on purpose: a half-translated site is the most common way
 * a multilingual project fails, and it fails quietly. Here it cannot.
 */
export function TranslationChips({
  translations,
  className,
}: {
  readonly translations: readonly TranslationSummary[]
  readonly className?: string
}) {
  if (translations.length === 0) {
    return (
      <span className={cn('type-label text-muted', className)}>Belum ada bahasa</span>
    )
  }

  return (
    <span className={cn('inline-flex flex-wrap items-center gap-x-4 gap-y-1', className)}>
      {translations.map((translation) => (
        <StatusChip
          key={translation.localeCode}
          status={translation.status}
          label={translation.localeCode.toUpperCase()}
        />
      ))}
    </span>
  )
}
