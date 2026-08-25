import type { ReactNode } from 'react'

import { Note } from '@/components/ui/Note'
import { cn } from '@/lib/cn'

type Props = {
  readonly note: ReactNode
  readonly children: ReactNode
  readonly className?: string
}

/**
 * Pairs a block of content with its margin note in a single grid row, so the
 * note aligns with the paragraph it annotates instead of drifting below it.
 *
 * Content comes first in the DOM; the note follows as an <aside>. Reading the
 * page in sequence therefore still makes sense.
 */
export function Annotation({ note, children, className }: Props) {
  return (
    <div className={cn('annotation', className)}>
      <div className="annotation__body">{children}</div>
      <Note>{note}</Note>
    </div>
  )
}
