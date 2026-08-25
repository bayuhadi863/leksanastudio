import { useCallback, useLayoutEffect, useState, type RefObject } from 'react'

/** Floor, so a very short window still leaves something usable on screen. */
const MINIMUM = 320

/**
 * The height that reaches from an element's top to the bottom of the window.
 *
 * Used by the split editor, where both columns scroll on their own instead of
 * the page scrolling under them. A CSS-only version would have to hard-code the
 * chrome above it — panel bar, page title, mode switch — and would be wrong the
 * first time any of those changed. Measuring asks the layout instead of
 * guessing at it.
 *
 * The save bar is measured rather than assumed for the same reason: it overlays
 * the bottom of the screen, and a column that runs under it hides its own last
 * line.
 */
export function useFillHeight(
  ref: RefObject<HTMLElement | null>,
  enabled: boolean,
  /** Anything that changes where the element starts — the editor's mode, say. */
  revision?: unknown,
) {
  const [height, setHeight] = useState<number | null>(null)

  const measure = useCallback(() => {
    const element = ref.current
    if (!element) return

    const top = element.getBoundingClientRect().top
    const saveBar = document.querySelector('[data-save-bar]')
    const reserved = saveBar ? saveBar.getBoundingClientRect().height + 12 : 24

    setHeight(Math.max(MINIMUM, Math.round(window.innerHeight - top - reserved)))
  }, [ref])

  useLayoutEffect(() => {
    if (!enabled) {
      setHeight(null)
      return
    }

    measure()

    // A second pass after paint: web fonts and the save bar settle a frame
    // later, and a height measured before they do is a few pixels short.
    const settle = window.requestAnimationFrame(measure)

    window.addEventListener('resize', measure)
    return () => {
      window.cancelAnimationFrame(settle)
      window.removeEventListener('resize', measure)
    }
  }, [enabled, measure, revision])

  return enabled ? height : null
}
