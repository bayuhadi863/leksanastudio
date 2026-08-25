import { useEffect, useRef, useState, type ReactNode } from 'react'
import { createPortal } from 'react-dom'

/**
 * A real viewport for the preview.
 *
 * The first version rendered the preview straight into the page, and for a
 * same-width preview that was fine. It broke the moment the sheet was narrowed
 * to a phone: `@media (min-width: 64rem)` and every `vw` in the type scale
 * answer to the *browser* viewport, not to the box they happen to sit in. So a
 * 390px sheet kept the desktop layout — four fact columns crushed into a phone,
 * headings sized for a 1440px screen — and reported a phone check that had not
 * happened.
 *
 * An iframe is the only thing that gives a box its own viewport. It costs a
 * stylesheet copy and a theme mirror, which is a small price for a preview that
 * is telling the truth. Same origin, so the parent can reach in and do both.
 */
export function PreviewFrame({
  width,
  children,
  className,
  onFrameReady,
}: {
  /** CSS width for the frame — a phone width, or 100% of the pane. */
  readonly width: string
  readonly children: ReactNode
  readonly className?: string
  /** Hands the frame's document to the pane, which scrolls it to the edited block. */
  readonly onFrameReady?: (document: Document | null) => void
}) {
  const frameRef = useRef<HTMLIFrameElement>(null)
  const [mountNode, setMountNode] = useState<HTMLElement | null>(null)

  useEffect(() => {
    const frame = frameRef.current
    if (!frame) return

    let cancelled = false
    let styleObserver: MutationObserver | null = null
    let themeObserver: MutationObserver | null = null

    const attach = () => {
      const doc = frame.contentDocument
      if (!doc || cancelled) return

      doc.documentElement.lang = 'id'
      doc.body.style.margin = '0'

      // The stylesheet is copied rather than linked: in dev Vite serves it as
      // inline <style> tags that change on every edit, and in production it is
      // a hashed <link>. Copying handles both, and the observer keeps hot
      // reloads from leaving the preview on yesterday's CSS.
      const syncStyles = () => {
        // The frame can be torn down between a mutation and this callback —
        // switching to writing mode does exactly that — and a half-gone
        // document has a null head.
        const head = frame.contentDocument?.head
        if (!head) return

        head.querySelectorAll('[data-preview-style]').forEach((node) => node.remove())

        document.head
          .querySelectorAll('style, link[rel="stylesheet"]')
          .forEach((node) => {
            const clone = node.cloneNode(true) as HTMLElement
            clone.setAttribute('data-preview-style', '')
            head.append(clone)
          })
      }

      // The reader's theme is the reader's theme, inside the frame as well.
      const syncTheme = () => {
        const root = frame.contentDocument?.documentElement
        if (!root) return

        const theme = document.documentElement.getAttribute('data-theme')
        if (theme) root.setAttribute('data-theme', theme)
        else root.removeAttribute('data-theme')
      }

      syncStyles()
      syncTheme()

      styleObserver = new MutationObserver(syncStyles)
      styleObserver.observe(document.head, { childList: true, subtree: true })

      themeObserver = new MutationObserver(syncTheme)
      themeObserver.observe(document.documentElement, {
        attributes: true,
        attributeFilter: ['data-theme'],
      })

      setMountNode(doc.body)
      onFrameReady?.(doc)
    }

    if (frame.contentDocument?.readyState === 'complete') attach()
    frame.addEventListener('load', attach)

    return () => {
      cancelled = true
      frame.removeEventListener('load', attach)
      styleObserver?.disconnect()
      themeObserver?.disconnect()
      onFrameReady?.(null)
    }
    // `onFrameReady` is a stable callback from the pane; re-running on every
    // render would tear the frame down mid-edit.
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [])

  return (
    <>
      <iframe
        ref={frameRef}
        title="Pratinjau halaman"
        // A doctype, so the frame is in standards mode like the real page.
        srcDoc="<!doctype html><html><head><meta charset='utf-8'></head><body></body></html>"
        style={{ width }}
        className={className}
      />
      {/* The portal renders into the frame's body; its place in this tree only
          decides which React context the preview sees. */}
      {mountNode ? createPortal(children, mountNode) : null}
    </>
  )
}
