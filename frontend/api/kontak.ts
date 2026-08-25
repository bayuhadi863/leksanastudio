import { clientKeyFrom, handleContact, MALFORMED_BODY } from '../server/contact'

export const config = { runtime: 'nodejs' }

/**
 * Deployment entry point for hosts that run Web-standard serverless functions
 * (Vercel, Netlify, Cloudflare). Locally the same handler is mounted on the
 * Vite dev server instead — see vite.config.ts.
 */
export default async function POST(request: Request): Promise<Response> {
  if (request.method !== 'POST') {
    return Response.json({ error: 'Metode tidak diizinkan.' }, { status: 405 })
  }

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
