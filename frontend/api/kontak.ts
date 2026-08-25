import { clientKeyFrom, handleContact, MALFORMED_BODY } from '../server/contact.js'

export const config = { runtime: 'nodejs' }

/*
 * The `.js` on the import above is not a typo. This package is `"type":
 * "module"`, so the deployed function is a real ES module and Node resolves
 * specifiers literally — an extensionless path that TypeScript accepts happily
 * becomes ERR_MODULE_NOT_FOUND at runtime. TypeScript maps the `.js` back to
 * the `.ts` file; Node gets the name it needs.
 */

/**
 * Deployment entry point for hosts that run Web-standard serverless functions
 * (Vercel, Netlify, Cloudflare). Locally the same handler is mounted on the
 * Vite dev server instead — see vite.config.ts.
 *
 * Exported by method name, not as a default. A default export is read as the
 * Node signature `(req, res) => void`, and a `Response` returned from one is
 * discarded — the request simply hangs until the platform times it out. The
 * named export is what selects the Web-standard signature, and it lets the
 * platform answer anything that is not a POST on its own.
 */
export async function POST(request: Request): Promise<Response> {
  let payload: unknown
  try {
    payload = await request.json()
  } catch {
    return Response.json(MALFORMED_BODY.body, { status: MALFORMED_BODY.status })
  }

  const clientKey = clientKeyFrom({
    forwardedFor: request.headers.get('x-forwarded-for') ?? undefined,
    realIp: request.headers.get('x-real-ip') ?? undefined,
  })

  const result = await handleContact(payload, clientKey)
  return Response.json(result.body, { status: result.status })
}
