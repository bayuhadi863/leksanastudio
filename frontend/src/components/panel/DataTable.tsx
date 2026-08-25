import type { ReactNode } from 'react'

import { ErrorState } from '@/components/panel/ErrorState'
import { Label } from '@/components/ui/Label'
import { cn } from '@/lib/cn'

export type Column<T> = {
  readonly key: string
  readonly header: string
  readonly render: (row: T) => ReactNode
  /** Hidden below the note breakpoint — for columns that are useful, not essential. */
  readonly secondary?: boolean
  readonly align?: 'left' | 'right'
  readonly width?: string
}

type Props<T> = {
  readonly columns: readonly Column<T>[]
  readonly rows: readonly T[]
  readonly rowKey: (row: T) => string
  readonly onRowClick?: (row: T) => void
  readonly isLoading?: boolean
  readonly error?: unknown
  readonly onRetry?: () => void
  /** Shown when there is genuinely nothing — not while loading, and not on error. */
  readonly empty?: ReactNode
  readonly caption: string
}

/**
 * A document table, not a data grid.
 *
 * Hairline rules and generous rows rather than borders and zebra stripes: the
 * panel is meant to read like the site it edits. Rows are clickable in full,
 * because hunting for a small "edit" link is the tax most admin tables charge.
 */
export function DataTable<T>({
  columns,
  rows,
  rowKey,
  onRowClick,
  isLoading,
  error,
  onRetry,
  empty,
  caption,
}: Props<T>) {
  if (error) {
    return <ErrorState error={error} title="Gagal memuat daftar" onRetry={onRetry} />
  }

  if (isLoading) {
    return <TableSkeleton columns={columns.length} />
  }

  if (rows.length === 0) {
    return <>{empty}</>
  }

  return (
    <div className="scroll-x">
      <table className="w-full min-w-2xl border-collapse text-left">
        <caption className="sr-only">{caption}</caption>
        <thead>
          <tr className="border-line border-b">
            {columns.map((column) => (
              <th
                key={column.key}
                scope="col"
                style={column.width ? { width: column.width } : undefined}
                className={cn(
                  'pb-2',
                  column.align === 'right' && 'text-right',
                  column.secondary && 'hidden lg:table-cell',
                )}
              >
                <Label as="span">{column.header}</Label>
              </th>
            ))}
          </tr>
        </thead>

        <tbody>
          {rows.map((row) => (
            <tr
              key={rowKey(row)}
              onClick={onRowClick ? () => onRowClick(row) : undefined}
              className={cn(
                'border-line border-b transition-colors duration-150 ease-out',
                onRowClick && 'hover:bg-surface cursor-pointer',
              )}
            >
              {columns.map((column) => (
                <td
                  key={column.key}
                  className={cn(
                    'py-2.5 pr-5 align-top',
                    column.align === 'right' && 'pr-0 text-right',
                    column.secondary && 'hidden lg:table-cell',
                  )}
                >
                  {column.render(row)}
                </td>
              ))}
            </tr>
          ))}
        </tbody>
      </table>
    </div>
  )
}

/**
 * Rules that stand in for rows while loading.
 *
 * Shaped like the table it replaces so the page does not jump when the data
 * lands — a spinner would be less work and more disruption.
 */
function TableSkeleton({ columns }: { readonly columns: number }) {
  return (
    <div aria-busy="true" aria-live="polite">
      <span className="sr-only">Memuat daftar…</span>
      <div className="border-line border-b pb-3">
        <span className="bg-line block h-2 w-24" />
      </div>
      {Array.from({ length: 5 }).map((_, row) => (
        <div key={row} className="border-line flex gap-6 border-b py-5">
          {Array.from({ length: columns }).map((__, cell) => (
            <span
              key={cell}
              className="bg-line block h-3 animate-pulse"
              style={{
                width: cell === 0 ? '38%' : `${12 + ((row + cell) % 3) * 6}%`,
                animationDelay: `${(row * columns + cell) * 40}ms`,
              }}
            />
          ))}
        </div>
      ))}
    </div>
  )
}

/**
 * What an empty list says.
 *
 * Given a job to do rather than a shrug: "Belum ada apa-apa" tells an editor
 * nothing they did not already know.
 */
export function EmptyState({
  title,
  body,
  action,
}: {
  readonly title: string
  readonly body: string
  readonly action?: ReactNode
}) {
  return (
    <div className="border-line max-w-[var(--measure)] border-t pt-8">
      <Label as="p">Masih kosong</Label>
      <h2 className="type-h3 mt-3">{title}</h2>
      <p className="text-muted mt-3">{body}</p>
      {action ? <div className="mt-6">{action}</div> : null}
    </div>
  )
}
