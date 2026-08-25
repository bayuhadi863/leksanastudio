/**
 * What this build of the site includes.
 *
 * The public site stands on its own: every page it serves is built from files
 * in this repository, so it needs no server of its own and keeps working when
 * one is down. The management panel is the only part that needs the API — sign
 * in, permissions, content editing, uploads.
 *
 * So the two are separable, and the switch is a build-time flag rather than a
 * runtime check. At build time the branch below folds to a constant, the
 * bundler drops the panel's imports, and a public deployment ships no panel
 * code at all — not hidden, not routed, simply absent. A runtime flag would
 * have shipped the whole panel and asked it politely not to render.
 *
 * Default off. The safe default is the one that ships less: a deployment that
 * forgot to set anything gets the site, not an admin panel pointed at an API
 * that is not there.
 */

/** True when this build includes the management panel and its sign-in screens. */
export const panelEnabled = __PANEL_ENABLED__

/**
 * Where the API lives for builds that include the panel.
 *
 * Absolute in local development (the fallback). In production set
 * `VITE_API_BASE_URL` to an origin-relative path (e.g. `/api/v1`) so the panel
 * and its file URLs stay portable behind a reverse proxy.
 */
export const apiBaseUrl = import.meta.env.VITE_API_BASE_URL || 'http://localhost:5180/api/v1'
