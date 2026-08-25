import type { IncomingMessage, ServerResponse } from 'node:http'

import { clientKeyFrom, handleContact, MALFORMED_BODY } from './contact'

const readBody = (request: IncomingMessage): Promise<string> =>
  new Promise((resolve, reject) => {
    let raw = ''
    request.setEncoding('utf8')
    request.on('data', (chunk: string) => {
      raw += chunk
      // A form with three fields has no business sending a megabyte.
      if (raw.length > 100_000) {
        request.destroy()
        reject(new Error('payload terlalu besar'))
      }
    })
    request.on('end', () => resolve(raw))
    request.on('error', reject)
  })

const send = (response: ServerResponse, status: number, body: unknown): void => {
  response.statusCode = status
  response.setHeader('Content-Type', 'application/json; charset=utf-8')
  response.end(JSON.stringify(body))
}

const header = (request: IncomingMessage, name: string): string | undefined => {
  const value = request.headers[name]
  return Array.isArray(value) ? value[0] : value
}

/**
 * Mounts POST /api/kontak on the Vite dev and preview servers, so the form
 * behaves locally exactly as it does once deployed.
 */
export const contactMiddleware = (
  request: IncomingMessage,
  response: ServerResponse,
  next: (error?: unknown) => void,
): void => {
  const url = request.url ?? ''
  if (!url.split('?')[0]?.endsWith('/api/kontak')) {
    next()
    return
  }

  if (request.method !== 'POST') {
    response.setHeader('Allow', 'POST')
    send(response, 405, { error: 'Metode tidak diizinkan.' })
    return
  }

  void (async () => {
    let payload: unknown
    try {
      payload = JSON.parse(await readBody(request))
    } catch {
      send(response, MALFORMED_BODY.status, MALFORMED_BODY.body)
      return
    }

    const clientKey = clientKeyFrom({
      forwardedFor: header(request, 'x-forwarded-for'),
      realIp: header(request, 'x-real-ip') ?? request.socket.remoteAddress ?? undefined,
    })

    const result = await handleContact(payload, clientKey)
    send(response, result.status, result.body)
  })()
}
