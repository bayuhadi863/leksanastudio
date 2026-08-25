import { ArrowLink } from '@/components/ui/ArrowLink'
import type { ProofPillar } from '@/config/copy'

type Props = {
  readonly pillars: readonly ProofPillar[]
}

/**
 * Every pillar must link to evidence on this same page. A claim with no proof
 * link does not belong here.
 */
export function ProofPillars({ pillars }: Props) {
  return (
    <ul className="grid gap-x-10 gap-y-12 lg:grid-cols-3">
      {pillars.map((pillar) => (
        <li key={pillar.title} className="border-accent border-t-2 pt-6">
          <h3 className="type-h3">{pillar.title}</h3>
          <p className="text-muted mt-4">{pillar.body}</p>
          <ArrowLink href={pillar.proofHref} className="mt-6">
            {pillar.proofLabel}
          </ArrowLink>
        </li>
      ))}
    </ul>
  )
}
