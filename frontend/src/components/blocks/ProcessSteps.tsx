import { Label } from '@/components/ui/Label'
import type { ProcessStep } from '@/config/process'

type Props = {
  readonly steps: readonly ProcessStep[]
  readonly detailed?: boolean
}

export function ProcessSteps({ steps, detailed = false }: Props) {
  return (
    <ol className="grid">
      {steps.map((step) => (
        <li
          key={step.step}
          className="border-line grid gap-x-8 gap-y-4 border-t py-8 last:border-b sm:grid-cols-[3rem_1fr]"
        >
          <span
            aria-hidden="true"
            className="numeric font-display text-accent text-[1.75rem] leading-none font-semibold"
          >
            {step.step}
          </span>

          <div>
            <div className="flex flex-wrap items-baseline justify-between gap-x-6 gap-y-1">
              <h3 className="type-h3">
                <span className="sr-only">Langkah {step.step}: </span>
                {step.title}
              </h3>
              <Label as="p">{step.duration}</Label>
            </div>

            <p className="text-muted mt-3 max-w-[var(--measure)]">{step.summary}</p>

            {detailed ? (
              <>
                <ul className="mt-5 grid gap-2.5">
                  {step.details.map((detail) => (
                    <li key={detail} className="type-small text-muted relative pl-5">
                      <span
                        aria-hidden="true"
                        className="bg-accent absolute top-[0.7em] left-0 h-px w-1.5"
                      />
                      {detail}
                    </li>
                  ))}
                </ul>

                <p className="type-small border-line mt-5 border-t pt-4">
                  <span className="text-muted">Yang saya butuhkan dari Anda: </span>
                  {step.clientInput}
                </p>
              </>
            ) : null}
          </div>
        </li>
      ))}
    </ol>
  )
}
