import { Label } from '@/components/ui/Label'
import { cn } from '@/lib/cn'
import { formatBytes, mediaUrl } from '@/lib/media'
import type { MediaPaginationDTO } from '@/types/content'

type Props = {
  readonly items: readonly MediaPaginationDTO[]
  readonly selectedId?: string | null
  readonly onPick: (item: MediaPaginationDTO) => void
  readonly columnsClassName?: string
}

/**
 * The library, as pictures.
 *
 * Filenames are shown under the image rather than instead of it: an editor
 * looking for "the dashboard screenshot" recognises it in a fraction of the
 * time it takes to read `IMG_20240817_2.png`. The name is still there for the
 * moment recognition fails.
 */
export function MediaGrid({ items, selectedId, onPick, columnsClassName }: Props) {
  return (
    <ul
      className={cn(
        'grid gap-4',
        columnsClassName ?? 'grid-cols-2 sm:grid-cols-3 lg:grid-cols-4',
      )}
    >
      {items.map((item) => {
        const selected = item.id === selectedId
        const url = mediaUrl(item.objectPath)

        return (
          <li key={item.id}>
            <button
              type="button"
              onClick={() => onPick(item)}
              aria-pressed={selected}
              className={cn(
                'group block w-full text-left',
                'focus-visible:outline-accent focus-visible:outline focus-visible:outline-2 focus-visible:outline-offset-2',
              )}
            >
              <span
                className={cn(
                  'bg-surface block overflow-hidden rounded-[var(--radius-control)] border transition-colors duration-150 ease-out',
                  selected ? 'border-accent ring-accent/25 ring-3' : 'border-line group-hover:border-accent',
                )}
              >
                <img
                  src={url ?? ''}
                  alt=""
                  loading="lazy"
                  className="aspect-[4/3] w-full object-cover"
                />
              </span>

              <span className="mt-2 block truncate font-semibold" title={item.originalName}>
                {item.label || item.originalName}
              </span>

              <Label as="span" className="text-muted">
                {item.width && item.height ? `${item.width}×${item.height} · ` : ''}
                {formatBytes(item.sizeBytes)}
              </Label>
            </button>
          </li>
        )
      })}
    </ul>
  )
}
