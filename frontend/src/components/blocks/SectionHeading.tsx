import type { ReactNode } from 'react'

import { Label } from '@/components/ui/Label'
import { cn } from '@/lib/cn'

type Props = {
  readonly eyebrow?: string
  readonly title: ReactNode
  readonly lead?: ReactNode
  readonly className?: string
  readonly as?: 'h2' | 'h3'
}

export function SectionHeading({ eyebrow, title, lead, className, as: Heading = 'h2' }: Props) {
  return (
    <div className={cn('max-w-[var(--measure)]', className)}>
      {eyebrow ? <Label as="p">{eyebrow}</Label> : null}
      <Heading className={cn('type-h2', eyebrow && 'mt-3')}>{title}</Heading>
      {lead ? <p className="type-lead mt-4">{lead}</p> : null}
    </div>
  )
}
