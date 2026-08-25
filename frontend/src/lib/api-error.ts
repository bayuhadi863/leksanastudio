import { isAxiosError } from 'axios'
import { toast } from 'sonner'

/**
 * Network-level failure (server unreachable, DNS, timeout, CORS) — the request
 * never got an HTTP response.
 */
export const isNetworkError = (error: unknown): boolean => {
  if (!isAxiosError(error)) return false
  if (error.response) return false
  return (
    error.code === 'ERR_NETWORK' ||
    error.code === 'ECONNABORTED' ||
    error.code === 'ETIMEDOUT' ||
    error.message === 'Network Error'
  )
}

/**
 * "Unexpected" means not an actionable 4xx the caller already handles, but a
 * transient infrastructure failure: the network is down, or the server broke.
 */
export const isUnexpectedError = (error: unknown): boolean => {
  if (isNetworkError(error)) return true
  if (isAxiosError(error) && error.response) return error.response.status >= 500
  return false
}

/** Reader-facing message for an API error. The backend's own 4xx message wins. */
export const getErrorMessage = (error: unknown, fallback?: string): string => {
  if (isNetworkError(error)) {
    return 'Tidak dapat terhubung ke server. Periksa koneksi Anda.'
  }

  if (isAxiosError(error)) {
    const status = error.response?.status
    if (status && status >= 500) {
      return 'Server sedang bermasalah. Coba beberapa saat lagi.'
    }
    const data = error.response?.data as { message?: string } | undefined
    if (data?.message) return data.message
  }

  return fallback ?? 'Terjadi kesalahan. Coba lagi.'
}

/**
 * Field messages from a 400 the server rejected on validation, flattened.
 *
 * Empty for anything else, so a caller can render these when there are any and
 * fall back to the plain message when there are not. The keys are server-side
 * property paths, which are meaningless to an editor — the messages are not.
 */
export const getValidationMessages = (error: unknown): string[] => {
  if (!isAxiosError(error)) return []

  const data = error.response?.data as
    | { code?: string; errors?: Record<string, string[] | null> }
    | undefined

  if (data?.code !== 'VALIDATION_ERROR' || !data.errors) return []

  return Object.values(data.errors)
    .flatMap((messages) => messages ?? [])
    .filter(Boolean)
}

// A burst of failed requests is one outage, not twelve. Collapse it.
const TOAST_ID = 'api-error'
const THROTTLE_MS = 4000
let lastNotifiedAt = 0

/**
 * One throttled toast for an unexpected (5xx/network) error. Safe to call from
 * the interceptor on every failing request — duplicates collapse.
 */
export const notifyUnexpectedError = (error: unknown): void => {
  const now = Date.now()
  if (now - lastNotifiedAt < THROTTLE_MS) return
  lastNotifiedAt = now
  toast.error(getErrorMessage(error), { id: TOAST_ID })
}

/**
 * Toast an API error with its message. For form and action handlers, where the
 * backend's 4xx text should surface — unlike the throttled global toast.
 */
export const notifyApiError = (error: unknown, fallback?: string): void => {
  toast.error(getErrorMessage(error, fallback))
}
