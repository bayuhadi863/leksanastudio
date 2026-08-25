import { useEffect, useState } from 'react'


export type EditorMode = 'tulis' | 'belah' | 'pratinjau'

const STORAGE_KEY = 'leksana-panel-editor-mode'

/** Below this the two columns would each be too narrow to work in. */
const SPLIT_QUERY = '(min-width: 75rem)'

const read = (): EditorMode => {
  try {
    const stored = window.localStorage.getItem(STORAGE_KEY)
    if (stored === 'tulis' || stored === 'belah' || stored === 'pratinjau') return stored
  } catch {
    // Storage blocked — the default is fine.
  }
  return 'belah'
}

/**
 * Which of the two halves the editor is looking at.
 *
 * Remembered across screens and sessions: an editor who works split does so all
 * the time, and being asked to re-open the preview on every entry is the kind
 * of small friction that ends with the preview never being used.
 *
 * Split disappears below 75rem rather than being offered and then feeling
 * cramped — two 300px columns are not two panes, they are one broken one.
 */
export function useEditorMode() {
  const [mode, setMode] = useState<EditorMode>('tulis')
  const [canSplit, setCanSplit] = useState(false)

  useEffect(() => {
    const query = window.matchMedia(SPLIT_QUERY)
    const stored = read()

    const sync = () => {
      setCanSplit(query.matches)
      setMode((current) => {
        const wanted = current === 'tulis' && stored !== 'tulis' ? stored : current
        return !query.matches && wanted === 'belah' ? 'tulis' : wanted
      })
    }

    sync()
    query.addEventListener('change', sync)
    return () => query.removeEventListener('change', sync)
  }, [])

  const choose = (next: EditorMode) => {
    setMode(next)
    try {
      window.localStorage.setItem(STORAGE_KEY, next)
    } catch {
      // Not remembering is survivable; not switching is not.
    }
  }

  return { mode, setMode: choose, canSplit }
}
