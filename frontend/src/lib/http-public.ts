import axios from 'axios'

import { API_BASE_URL } from '@/config/api'
import { isUnexpectedError, notifyUnexpectedError } from '@/lib/api-error'

/**
 * The unauthenticated client — for endpoints the public site reads, and for
 * login itself. Deliberately separate from `http`: it carries no token, so a
 * stale session can never change what a visitor sees.
 */
const httpPublic = axios.create({
  baseURL: API_BASE_URL,
  headers: { 'Content-Type': 'application/json' },
})

httpPublic.interceptors.response.use(
  (response) => response,
  (error) => {
    if (isUnexpectedError(error)) notifyUnexpectedError(error)
    return Promise.reject(error)
  },
)

export default httpPublic
