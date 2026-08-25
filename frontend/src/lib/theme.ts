/**
 * Theme preference.
 *
 * Three states, not two. "System" is a real preference — a reader who has
 * configured their device already made a choice, and replacing that with a
 * binary switch throws it away.
 *
 * Storage is per-browser and never leaves the device.
 */

export const THEME_STORAGE_KEY = 'leksana-theme'

export const THEMES = ['light', 'system', 'dark'] as const
export type Theme = (typeof THEMES)[number]

export const THEME_LABEL: Record<Theme, string> = {
  light: 'Terang',
  system: 'Sistem',
  dark: 'Gelap',
}

export const isTheme = (value: unknown): value is Theme =>
  typeof value === 'string' && (THEMES as readonly string[]).includes(value)

/**
 * Applies a preference to the document.
 *
 * `system` removes the attribute entirely rather than writing a value, so the
 * `prefers-color-scheme` media query in globals.css takes over again.
 */
export const applyTheme = (theme: Theme): void => {
  const root = document.documentElement
  if (theme === 'system') {
    root.removeAttribute('data-theme')
  } else {
    root.dataset.theme = theme
  }
}

export const readStoredTheme = (): Theme => {
  try {
    const stored = localStorage.getItem(THEME_STORAGE_KEY)
    return isTheme(stored) ? stored : 'system'
  } catch {
    // Private mode, blocked storage, or an embedded webview. Falling back to
    // the device setting is the correct answer, not an error.
    return 'system'
  }
}

export const storeTheme = (theme: Theme): void => {
  try {
    if (theme === 'system') {
      localStorage.removeItem(THEME_STORAGE_KEY)
    } else {
      localStorage.setItem(THEME_STORAGE_KEY, theme)
    }
  } catch {
    // Preference simply does not persist. The page still works.
  }
}

/**
 * Runs before first paint, in <head>, so a reader who chose a theme never sees
 * the other one flash first. Kept to one line and wrapped in try/catch because
 * a throw here would block the whole document.
 */
export const themeInitScript = `try{var t=localStorage.getItem(${JSON.stringify(
  THEME_STORAGE_KEY,
)});if(t==="light"||t==="dark"){document.documentElement.setAttribute("data-theme",t)}}catch(e){}`
