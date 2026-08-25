import type { ReactNode } from 'react'

import { cn } from '@/lib/cn'

type Props = {
  readonly children: ReactNode
  /**
   * Column template for the row, in CSS. Defaults to equal columns — pass a
   * template when one field is genuinely narrower than the other, e.g. a step
   * number beside a title: `"7rem minmax(0, 1fr)"`.
   */
  readonly columns?: string
  readonly className?: string
}

/**
 * Fields that belong on one line.
 *
 * The alignment is the whole point. Each field inside hands its four rows —
 * label, hint, control, error — to a subgrid, so a hint that wraps to two lines
 * lifts *both* hints' track and the two inputs still start at the same pixel.
 * Left to themselves, two fields side by side are two independent stacks, and
 * the longer hint drops its own input half a line below its neighbour.
 *
 * Below 40rem the row stacks and the question does not arise.
 */
export function FieldRow({ children, columns, className }: Props) {
  return (
    <div
      className={cn('field-row', className)}
      style={columns ? ({ '--field-row-columns': columns } as React.CSSProperties) : undefined}
    >
      {children}
    </div>
  )
}
