import type { ReactNode } from 'react'
import { Link } from 'react-router-dom'

import { Label } from '@/components/ui/Label'
import { cn } from '@/lib/cn'

type Props = {
  readonly eyebrow: string
  readonly title: string
  readonly lead?: string
  /** Buttons for the page as a whole — create, publish, delete. */
  readonly actions?: ReactNode
  /** Trail back to where this page came from. Omitted on top-level screens. */
  readonly backTo?: { readonly href: string; readonly label: string }
  readonly children: ReactNode
  readonly wide?: boolean
  /** Drops the measure entirely — for the split editor, which needs the width. */
  readonly fluid?: boolean
}

/**
 * The frame every panel screen sits in.
 *
 * Deliberately the same furniture as the public site: a mono eyebrow, a serif
 * title, one line of lead. The panel should read as the back of the same sheet
 * of paper — not as a different product wearing the same logo.
 */
export function PanelPage({
  eyebrow,
  title,
  lead,
  actions,
  backTo,
  children,
  wide,
  fluid,
}: Props) {
  return (
    <div className={cn('mx-auto', fluid ? 'max-w-none' : wide ? 'max-w-6xl' : 'max-w-4xl')}>
      <header className="border-line border-b pb-4">
        {backTo ? (
          <Link
            to={backTo.href}
            className="type-small text-muted hover:text-accent mb-3 inline-flex items-center gap-2"
          >
            <span aria-hidden="true">&larr;</span>
            {backTo.label}
          </Link>
        ) : null}

        <div className="flex flex-col gap-4 sm:flex-row sm:items-end sm:justify-between">
          <div className="max-w-[var(--measure)]">
            <Label as="p">{eyebrow}</Label>
            <h1 className="type-h1 mt-1">{title}</h1>
            {lead ? <p className="text-muted mt-1.5">{lead}</p> : null}
          </div>

          {actions ? <div className="flex shrink-0 flex-wrap gap-2">{actions}</div> : null}
        </div>
      </header>

      <div className="pt-4">{children}</div>
    </div>
  )
}
