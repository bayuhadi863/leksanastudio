import type { ReactNode } from 'react'

import { cn } from '@/lib/cn'

type Props = {
  readonly children: ReactNode
  readonly className?: string
}

/**
 * Margin note — the site's signature element (blueprint 06b §6b.2).
 *
 * Rules that keep it from becoming decoration:
 *   · at most four per page
 *   · always first person
 *   · carries a reason, an objection, or a limit — never a feature, never a CTA
 *   · 20–45 words
 *
 * Below the note breakpoint it renders inline with an accent rule, because
 * its content matters too much to hide on small screens.
 *
 * The body is a <div>, not a <p>: authored from MDX the children arrive
 * already wrapped in their own paragraph, and a <p> inside a <p> is invalid
 * HTML that surfaces as a hydration error.
 */
export function Note({ children, className }: Props) {
  return (
    <aside className={cn('note', className)}>
      <p className="type-label text-accent mb-2 lg:mb-1.5">Catatan</p>
      <div className="note__body type-note text-muted">{children}</div>
    </aside>
  )
}
