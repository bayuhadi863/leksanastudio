/**
 * Single source of truth for the API base URL.
 *
 * Lives beside the panel switch in `features.ts`, because the two answer the
 * same question: does this build talk to a server at all?
 */
export { apiBaseUrl as API_BASE_URL } from '@/config/features'
