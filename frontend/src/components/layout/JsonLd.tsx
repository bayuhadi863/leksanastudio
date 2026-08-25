type Props = {
  /** Static, build-time JSON generated from typed config — no user input reaches this. */
  readonly data: unknown
}

/** Structured data. Read from the DOM by crawlers; never executed. */
export function JsonLd({ data }: Props) {
  return (
    <script type="application/ld+json" dangerouslySetInnerHTML={{ __html: JSON.stringify(data) }} />
  )
}
