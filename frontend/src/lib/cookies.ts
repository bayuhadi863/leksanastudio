export interface CookieOptions {
  expires?: number | Date
  path?: string
  domain?: string
  secure?: boolean
  sameSite?: 'Strict' | 'Lax' | 'None'
}

export const setCookie = (name: string, value: string, options: CookieOptions = {}): void => {
  let cookieString = `${encodeURIComponent(name)}=${encodeURIComponent(value)}`

  if (options.expires) {
    if (typeof options.expires === 'number') {
      const date = new Date()
      date.setTime(date.getTime() + options.expires * 24 * 60 * 60 * 1000)
      cookieString += `; expires=${date.toUTCString()}`
    } else {
      cookieString += `; expires=${options.expires.toUTCString()}`
    }
  }

  cookieString += `; path=${options.path ?? '/'}`

  if (options.domain) cookieString += `; domain=${options.domain}`
  if (options.secure) cookieString += '; secure'
  if (options.sameSite) cookieString += `; samesite=${options.sameSite}`

  document.cookie = cookieString
}

export const getCookie = (name: string): string | null => {
  const prefix = `${encodeURIComponent(name)}=`
  for (const raw of document.cookie.split(';')) {
    const entry = raw.trimStart()
    if (entry.startsWith(prefix)) {
      return decodeURIComponent(entry.slice(prefix.length))
    }
  }
  return null
}

export const removeCookie = (name: string, path = '/'): void => {
  setCookie(name, '', { expires: -1, path })
}

export const getCookieItem = <T>(key: string): T | null => {
  try {
    const item = getCookie(key)
    return item ? (JSON.parse(item) as T) : null
  } catch {
    // A cookie someone edited by hand, or a value from an older shape.
    // Treating it as absent is the only safe reading.
    return null
  }
}

export const setCookieItem = <T>(key: string, value: T, options?: CookieOptions): void => {
  try {
    setCookie(key, JSON.stringify(value), options)
  } catch {
    // Storage blocked (private mode, embedded webview). The session simply
    // does not persist across reloads; the page still works.
  }
}

export const removeCookieItem = (key: string): void => {
  try {
    removeCookie(key)
  } catch {
    // Same as above — nothing to recover from.
  }
}
