/** First focusable element on every page. */
export function SkipLink() {
  return (
    <a
      href="#konten"
      className="focus:bg-accent focus:text-accent-fg sr-only focus:not-sr-only focus:fixed focus:top-3 focus:left-3 focus:z-100 focus:rounded-[var(--radius-control)] focus:px-4 focus:py-3"
    >
      Lewati ke konten
    </a>
  )
}
