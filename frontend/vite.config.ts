import { fileURLToPath, URL } from 'node:url'

import mdx from '@mdx-js/rollup'
import tailwindcss from '@tailwindcss/vite'
import react from '@vitejs/plugin-react'
import remarkFrontmatter from 'remark-frontmatter'
import remarkGfm from 'remark-gfm'
import remarkMdxFrontmatter from 'remark-mdx-frontmatter'
import { defineConfig, loadEnv, type Connect, type Plugin } from 'vite'

import { contactMiddleware } from './server/contact-middleware'
import { buildReadingTimes } from './server/reading-time'
import { buildRobots, buildSitemap } from './server/sitemap'
import { themeInitScript } from './src/lib/theme'

/**
 * Runs before first paint, in <head>, so a reader who chose a theme never sees
 * the other one flash first. Injected from `src/lib/theme.ts` rather than
 * pasted into index.html, so the storage key can never drift apart.
 */
const themeScript: Plugin = {
  name: 'leksana-theme-script',
  transformIndexHtml: {
    order: 'pre',
    handler: (html) =>
      html.replace(
        '<!-- theme-init-script: injected before first paint by vite.config.ts -->',
        `<script>${themeInitScript}</script>`,
      ),
  },
}

/**
 * The site is entirely static. No remote images, no third-party scripts, no
 * embeds. Security headers are set here for the dev and preview servers; in
 * production they belong to whichever host serves the build output (see
 * README — vercel.json / netlify.toml / Caddy all carry the same four).
 */
const setSecurityHeaders: Connect.NextHandleFunction = (_request, response, next) => {
  response.setHeader('X-Content-Type-Options', 'nosniff')
  response.setHeader('Referrer-Policy', 'strict-origin-when-cross-origin')
  response.setHeader('X-Frame-Options', 'DENY')
  response.setHeader(
    'Permissions-Policy',
    'camera=(), microphone=(), geolocation=(), interest-cohort=()',
  )
  next()
}

const securityHeaders: Plugin = {
  name: 'leksana-security-headers',
  configureServer(server) {
    server.middlewares.use(setSecurityHeaders)
  },
  configurePreviewServer(server) {
    server.middlewares.use(setSecurityHeaders)
  },
}

/**
 * Enquiry intake for `vite dev` and `vite preview`.
 *
 * Next handled this with a route handler inside the same app. Vite has no
 * server of its own in production, so the handler lives in `server/` and is
 * mounted twice: here for local work, and from `api/kontak.ts` when deployed
 * to a host with serverless functions.
 */
const contactApi: Plugin = {
  name: 'leksana-contact-api',
  configureServer(server) {
    server.middlewares.use(contactMiddleware)
  },
  configurePreviewServer(server) {
    server.middlewares.use(contactMiddleware)
  },
}

const READING_TIME_ID = 'virtual:leksana-reading-time'
const RESOLVED_READING_TIME_ID = `\0${READING_TIME_ID}`

/**
 * Serves the reading-estimate table as a virtual module, so lib/content.ts can
 * import a plain `Record<string, number>` instead of the article sources.
 */
const readingTime: Plugin = {
  name: 'leksana-reading-time',
  resolveId(id) {
    return id === READING_TIME_ID ? RESOLVED_READING_TIME_ID : undefined
  },
  load(id) {
    if (id !== RESOLVED_READING_TIME_ID) return undefined
    return `export const readingTimes = ${JSON.stringify(buildReadingTimes(process.cwd()))}`
  },
  handleHotUpdate({ file, server }) {
    if (!file.endsWith('.mdx')) return
    const module = server.moduleGraph.getModuleById(RESOLVED_READING_TIME_ID)
    if (module) server.moduleGraph.invalidateModule(module)
  },
}

/**
 * Emits sitemap.xml and robots.txt into the build output. They are generated
 * rather than committed so a new service, vertical or article can never be
 * missing from them.
 */
const sitemapPlugin = (siteUrl: string): Plugin => ({
  name: 'leksana-sitemap',
  apply: 'build',
  generateBundle() {
    const root = process.cwd()
    this.emitFile({
      type: 'asset',
      fileName: 'sitemap.xml',
      source: buildSitemap(root, new Date(), siteUrl),
    })
    this.emitFile({ type: 'asset', fileName: 'robots.txt', source: buildRobots(siteUrl) })
  },
})

const mdxPlugin = mdx({
  remarkPlugins: [remarkGfm, remarkFrontmatter, [remarkMdxFrontmatter, { name: 'frontmatter' }]],
})

/**
 * MDX, but only for real module imports.
 *
 * `@mdx-js/rollup` strips the query string before deciding whether to compile
 * (`const [path] = id.split('?')`), so `foo.mdx?raw` would come back compiled
 * instead of as text — and lib/content.ts reads exactly that to count words
 * for the reading estimate.
 */
const mdxModules: Plugin = {
  ...(mdxPlugin as unknown as Plugin),
  enforce: 'pre',
  transform(value: string, id: string) {
    if (id.includes('?')) return undefined
    return mdxPlugin.transform.call(this, value, id)
  },
}

/**
 * Whether this build ships the management panel.
 *
 * Replaced as a literal at transform time, which is what lets the bundler drop
 * the panel's dynamic imports entirely: `false ? lazy(() => import(…)) : null`
 * folds away, and the chunk is never emitted. A value read through a module
 * import would survive that analysis and ship the whole panel to a site that
 * has no server to talk to.
 */
const readPanelFlag = (env: Record<string, string>): boolean => {
  const value = (env.VITE_PANEL ?? '').trim().toLowerCase()
  return value === 'on' || value === 'true' || value === '1'
}

/**
 * The canonical address the build should speak in.
 *
 * Read here and handed to the sitemap plugin, because that plugin runs in Node
 * where `import.meta.env` is empty: left to itself it would publish the
 * placeholder domain from `config/site.ts` on every deployment.
 */
const readSiteUrl = (env: Record<string, string>): string =>
  (env.VITE_SITE_URL ?? 'https://leksana.id').replace(/\/$/, '')

export default defineConfig(({ mode }) => {
  const env = loadEnv(mode, process.cwd(), '')

  return {
    define: {
      __PANEL_ENABLED__: JSON.stringify(readPanelFlag(env)),
    },

    plugins: [
      // MDX must run before the React plugin so that the JSX it emits is still
      // transformed — and Fast Refresh keeps working inside case studies.
      mdxModules,
      react({ include: /\.(jsx|js|mdx|md|tsx|ts)$/ }),
      tailwindcss(),
      readingTime,
      themeScript,
      sitemapPlugin(readSiteUrl(env)),
      securityHeaders,
      contactApi,
    ],

    resolve: {
      alias: {
        '@': fileURLToPath(new URL('./src', import.meta.url)),
      },
    },

    build: {
      target: 'es2022',
      sourcemap: false,
    },

    server: {
      port: 3000,
    },

    preview: {
      port: 3000,
    },
  }
})
