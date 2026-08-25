import { cn } from '@/lib/cn'

export type Metric = {
  readonly value: string
  readonly label: string
}

type Props = {
  readonly metrics: readonly Metric[]
  readonly className?: string
}

/**
 * No box, no icon, no gradient. This block is reused more than any other on
 * the site, so it is deliberately quiet.
 *
 * The term precedes the description in the DOM because that is what a
 * definition list requires; CSS order puts the number on top, where it reads
 * better. The accent rule is a pseudo-element for the same reason — a bare
 * <span> is not valid between <dt> and <dd>.
 */
export function MetricBlock({ metrics, className }: Props) {
  return (
    <dl className={cn('grid grid-cols-2 gap-x-8 gap-y-8 sm:grid-cols-3', className)}>
      {metrics.map((metric) => (
        <div key={metric.label} className="flex flex-col">
          <dt className="type-label text-muted before:bg-accent order-2 mt-3 before:mb-3 before:block before:h-0.5 before:w-6 before:content-['']">
            {metric.label}
          </dt>
          <dd className="numeric font-display order-1 text-[clamp(2rem,1.6rem+1.8vw,2.75rem)] leading-none font-semibold">
            {metric.value}
          </dd>
        </div>
      ))}
    </dl>
  )
}
