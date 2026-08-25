import type { Problem } from '@/config/services'

type Props = {
  readonly problems: readonly Problem[]
}

/**
 * The problems block always precedes the offer. A prospect who does not feel
 * understood does not read a list of deliverables.
 */
export function ProblemList({ problems }: Props) {
  return (
    <ul className="border-line bg-line grid gap-px border-y">
      {problems.map((problem) => (
        <li key={problem.title} className="bg-bg py-7">
          <h3 className="type-h3">{problem.title}</h3>
          <p className="text-muted mt-3 max-w-[var(--measure)]">{problem.body}</p>
        </li>
      ))}
    </ul>
  )
}
