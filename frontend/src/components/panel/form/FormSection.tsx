import type { ReactNode } from 'react'

import { Label } from '@/components/ui/Label'
import { cn } from '@/lib/cn'

type Props = {
  readonly title: string
  readonly eyebrow?: string
  readonly lead?: string
  /**
   * The margin note — the same device the public site uses.
   *
   * Rules for what belongs here are the site's own: a reason, an objection, or a
   * limit. Never a restatement of the field label, and never an instruction that
   * the field itself should have made obvious.
   */
  readonly note?: ReactNode
  readonly children: ReactNode
  readonly className?: string
}

/**
 * One group of fields, with its explanation in the margin.
 *
 * The annotation track is lifted straight from the public site, and not as
 * decoration: it puts the reason for a field beside the field instead of under
 * it, which is the difference between a form that teaches and a form that just
 * collects.
 */
export function FormSection({ title, eyebrow, lead, note, children, className }: Props) {
  return (
    <section className={cn('annotation', className)}>
      <div className="annotation__body">
        {eyebrow ? <Label as="p">{eyebrow}</Label> : null}
        <h2 className={cn('type-h2', eyebrow && 'mt-1')}>{title}</h2>
        {lead ? <p className="type-small text-muted mt-1">{lead}</p> : null}

        <div className="mt-4 grid gap-4">{children}</div>
      </div>

      {note ? (
        <aside className="note">
          <p className="type-label text-accent mb-2 lg:mb-1.5">Catatan</p>
          <div className="note__body type-note text-muted">{note}</div>
        </aside>
      ) : null}
    </section>
  )
}
