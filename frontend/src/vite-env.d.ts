/// <reference types="vite/client" />

/**
 * Whether this build includes the management panel.
 *
 * Defined in `vite.config.ts` and replaced with a literal at transform time —
 * see `config/features.ts` for what that buys.
 */
declare const __PANEL_ENABLED__: boolean

interface ImportMetaEnv {
  /** `on` builds the panel and its sign-in screens; anything else leaves them out. */
  readonly VITE_PANEL?: string
  /** API base for builds that include the panel, e.g. `/api/v1`. */
  readonly VITE_API_BASE_URL?: string
  /** Canonical site address. Feeds the sitemap, canonical tags, and JSON-LD. */
  readonly VITE_SITE_URL?: string
}

interface ImportMeta {
  readonly env: ImportMetaEnv
}
