import { getCookieItem, removeCookieItem, setCookieItem } from '@/lib/cookies'
import type { AuthTokens } from '@/types/auth'

const AUTH_KEY = 'leksana_auth'

export const getAuthTokens = (): AuthTokens | null => getCookieItem<AuthTokens>(AUTH_KEY)

export const setAuthTokens = (tokens: AuthTokens): void => {
  // SameSite=Lax so ordinary navigation keeps the session, without sending the
  // cookie on cross-site requests. `secure` only outside localhost, where the
  // browser would otherwise drop it over plain HTTP.
  setCookieItem(AUTH_KEY, tokens, {
    expires: 30,
    path: '/',
    sameSite: 'Lax',
    secure: window.location.protocol === 'https:',
  })
}

export const removeAuthTokens = (): void => removeCookieItem(AUTH_KEY)

export const getAccessToken = (): string | null => getAuthTokens()?.accessToken ?? null

export const getRefreshToken = (): string | null => getAuthTokens()?.refreshToken ?? null
