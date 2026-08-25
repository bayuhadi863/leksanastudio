import axios from 'axios'

import { API_BASE_URL } from '@/config/api'
import { authRoutes } from '@/config/panel'
import { getActiveRoleId } from '@/lib/active-role'
import { isUnexpectedError, notifyUnexpectedError } from '@/lib/api-error'
import { getAccessToken, getRefreshToken, removeAuthTokens, setAuthTokens } from '@/lib/tokens'
import type { BaseResponse } from '@/types/api'
import type { AuthTokens } from '@/types/auth'

/**
 * The authenticated client. Everything behind the panel goes through here.
 *
 * Two jobs beyond carrying the token: it renews an expired session once,
 * transparently, so a reader is never bounced to the login screen for a token
 * that could have been refreshed — and it turns an outage into a single toast
 * instead of one per in-flight request.
 */
const http = axios.create({
  baseURL: API_BASE_URL,
  headers: { 'Content-Type': 'application/json' },
})

http.interceptors.request.use((config) => {
  const token = getAccessToken()
  if (token) {
    config.headers.set('Authorization', `Bearer ${token}`)
  }

  // Scope the request to the role the user is acting as. The server validates
  // it against their assigned roles, so this cannot grant anything.
  const activeRoleId = getActiveRoleId()
  if (activeRoleId) {
    config.headers.set('X-Role-Active', activeRoleId)
  }

  return config
})

const endSession = (): void => {
  removeAuthTokens()
  window.location.href = authRoutes.login
}

http.interceptors.response.use(
  (response) => response,
  async (error) => {
    const originalRequest = error.config as (typeof error.config & { _retry?: boolean }) | undefined

    if (error.response?.status === 401 && originalRequest && !originalRequest._retry) {
      originalRequest._retry = true
      const refreshToken = getRefreshToken()

      if (refreshToken) {
        try {
          // A bare axios call on purpose: routing the refresh through this
          // instance would re-enter the same interceptor when it 401s too.
          const refreshResponse = await axios.post<BaseResponse<AuthTokens>>(
            `${API_BASE_URL}/auth/refresh`,
            { data: { refreshToken } },
          )

          if (refreshResponse.data?.success && refreshResponse.data.data) {
            const tokens = refreshResponse.data.data
            setAuthTokens(tokens)
            originalRequest.headers.Authorization = `Bearer ${tokens.accessToken}`
            return http(originalRequest)
          }
        } catch (refreshError) {
          // The refresh token is gone too — the session is genuinely over.
          endSession()
          return Promise.reject(refreshError)
        }
      }

      endSession()
    }

    // 5xx and network failures get one throttled toast; 4xx passes through for
    // the caller (usually a form) to render in place.
    if (isUnexpectedError(error)) notifyUnexpectedError(error)

    return Promise.reject(error)
  },
)

export default http
