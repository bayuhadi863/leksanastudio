import { getCookieItem, removeCookieItem, setCookieItem } from '@/lib/cookies'

/**
 * The role the user is acting as.
 *
 * Persisted so it survives a reload, and sent as `X-Role-Active` on every
 * request. The server validates it against the user's own roles — this is a
 * preference, never an authorisation.
 */
const ACTIVE_ROLE_KEY = 'leksana_active_role'

const cookieOptions = (days: number) => ({
  expires: days,
  path: '/',
  sameSite: 'Lax' as const,
  secure: window.location.protocol === 'https:',
})

export const getActiveRoleId = (): string | null => getCookieItem<string>(ACTIVE_ROLE_KEY)

export const setActiveRoleId = (roleId: string): void =>
  setCookieItem(ACTIVE_ROLE_KEY, roleId, cookieOptions(30))

export const removeActiveRoleId = (): void => removeCookieItem(ACTIVE_ROLE_KEY)

/**
 * The last role actually used, kept across logout (unlike the active role,
 * which is cleared). Only a role id, and always re-validated against the
 * signed-in user's roles before use — so it cannot leak another account's choice.
 */
const LAST_ROLE_KEY = 'leksana_last_role'

export const getLastRoleId = (): string | null => getCookieItem<string>(LAST_ROLE_KEY)

export const setLastRoleId = (roleId: string): void =>
  setCookieItem(LAST_ROLE_KEY, roleId, cookieOptions(180))
