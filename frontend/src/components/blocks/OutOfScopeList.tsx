import type { OutOfScopeItem } from '@/config/copy'

type Props = {
  readonly items: readonly OutOfScopeItem[]
}

/**
 * The refused-work list. It reads like turning down money; its actual job is
 * to filter, and to signal that there is enough work to be selective.
 */
export function OutOfScopeList({ items }: Props) {
  return (
    <ul className="grid gap-x-10 gap-y-0 sm:grid-cols-2">
      {items.map((item) => (
        <li key={item.title} className="border-line border-t py-6">
          <h3 className="flex gap-3 font-semibold">
            <span aria-hidden="true" className="text-muted opacity-55">
              ✕
            </span>
            {item.title}
          </h3>
          <p className="type-small text-muted mt-2 pl-7">{item.reason}</p>
        </li>
      ))}
    </ul>
  )
}
