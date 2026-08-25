import { useEffect } from 'react'
import { useLocation, useNavigationType } from 'react-router-dom'

/**
 * A new page starts at the top; going back returns you where you were.
 *
 * The framework used to do this. Without it a client-side router leaves the
 * reader halfway down the next page, which reads as a broken link.
 */
export function ScrollManager() {
  const { pathname, hash } = useLocation()
  const navigationType = useNavigationType()

  useEffect(() => {
    if (navigationType === 'POP') return

    if (hash) {
      const target = document.querySelector(hash)
      if (target) {
        target.scrollIntoView()
        return
      }
    }

    window.scrollTo(0, 0)
  }, [pathname, hash, navigationType])

  return null
}
