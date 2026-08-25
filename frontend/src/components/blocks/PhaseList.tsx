import { Label } from '@/components/ui/Label'
import type { ProjectPhase } from '@/config/packages'

type Props = {
  readonly phases: readonly ProjectPhase[]
}

/**
 * System work is not sold as a package list. It is sold as a sequence, which
 * is also why the numbering here is legitimate.
 */
export function PhaseList({ phases }: Props) {
  return (
    <ol className="grid">
      {phases.map((phase) => (
        <li
          key={phase.step}
          className="border-line grid gap-x-8 gap-y-3 border-t py-8 last:border-b sm:grid-cols-[3rem_1fr_auto]"
        >
          <span
            aria-hidden="true"
            className="numeric font-display text-accent text-[1.75rem] leading-none font-semibold"
          >
            {phase.step}
          </span>

          <div className="sm:col-start-2">
            <h3 className="type-h3">
              <span className="sr-only">Tahap {phase.step}: </span>
              {phase.name}
            </h3>
            <p className="text-muted mt-2 max-w-[var(--measure)]">{phase.scope}</p>
            {phase.note ? <p className="type-small text-accent mt-3">{phase.note}</p> : null}
          </div>

          <div className="sm:col-start-3 sm:text-right">
            <p className="numeric font-semibold">{phase.price}</p>
            <Label as="p" className="mt-1.5">
              {phase.duration}
            </Label>
          </div>
        </li>
      ))}
    </ol>
  )
}
