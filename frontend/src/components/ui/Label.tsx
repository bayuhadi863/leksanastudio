import type { ReactNode } from 'react'

import { cn } from '@/lib/cn'

type Props = {
  readonly children: ReactNode
  readonly className?: string
  readonly as?: 'span' | 'p' | 'div' | 'dt'
}

/** Mono label. Categories, metric units, table heads. The only place mono appears. */
export function Label({ children, className, as: Tag = 'span' }: Props) {
  return <Tag className={cn('type-label text-muted', className)}>{children}</Tag>
}
