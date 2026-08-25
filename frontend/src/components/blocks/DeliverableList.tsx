type Props = {
  readonly items: readonly string[]
}

export function DeliverableList({ items }: Props) {
  return (
    <ul className="grid max-w-[var(--measure)] gap-3.5">
      {items.map((item) => (
        <li key={item} className="relative pl-6">
          <span aria-hidden="true" className="bg-accent absolute top-[0.72em] left-0 h-px w-3" />
          {item}
        </li>
      ))}
    </ul>
  )
}
