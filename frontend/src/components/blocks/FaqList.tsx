import type { QA } from '@/config/services'

type Props = {
  readonly items: readonly QA[]
}

/**
 * Native <details>. Zero JavaScript, keyboard accessible by default, and
 * findable by in-page search in browsers that support it.
 */
export function FaqList({ items }: Props) {
  return (
    <div className="max-w-[var(--measure)]">
      {items.map((item) => (
        <details key={item.question} className="group/faq border-line border-b first:border-t">
          <summary className="hover:text-accent flex cursor-pointer list-none items-start justify-between gap-6 py-5 font-semibold transition-colors duration-150 ease-out [&::-webkit-details-marker]:hidden">
            {item.question}
            <span
              aria-hidden="true"
              className="text-accent mt-1 shrink-0 transition-transform duration-150 ease-out group-open/faq:rotate-45"
            >
              +
            </span>
          </summary>
          <p className="text-muted pb-6">{item.answer}</p>
        </details>
      ))}
    </div>
  )
}
