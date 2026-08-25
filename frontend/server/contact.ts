import { contactSchema, type ContactInput } from '../src/lib/contact-schema.js'

/**
 * Enquiry intake.
 *
 * Two notifications go out in parallel: email for the record, WhatsApp so it
 * actually reaches a phone. A lead that lands in an inbox and gets read
 * tomorrow morning is a lead that is already lost.
 *
 * Both channels are best-effort. If a provider is down or unconfigured, the
 * request still succeeds — losing the enquiry because a third party failed
 * would be the worse outcome.
 *
 * Under Next this was a route handler inside the app. Vite serves static files
 * and nothing else, so the logic lives here and is mounted twice: by the dev
 * and preview servers (server/contact-middleware.ts) and by the deployed
 * serverless function (api/kontak.ts).
 */

/** Kept in step with `site.email` in src/config/site.ts. */
const STUDIO_EMAIL = 'halo@leksana.id'

/* ----------------------------------------------------------- rate limiting */

const WINDOW_MS = 60_000
const MAX_PER_WINDOW = 3

// Per-instance, in-memory. Enough for the volume this site sees, and it needs
// no external service. Swap for a shared store the day that stops being true.
const hits = new Map<string, { count: number; resetAt: number }>()

const isRateLimited = (key: string): boolean => {
  const now = Date.now()
  const entry = hits.get(key)

  if (!entry || now > entry.resetAt) {
    hits.set(key, { count: 1, resetAt: now + WINDOW_MS })
    return false
  }

  entry.count += 1
  return entry.count > MAX_PER_WINDOW
}

/* ------------------------------------------------------------ notifications */

const composeText = (input: ContactInput): string =>
  [
    'Lead baru dari situs Leksana Studio',
    '',
    `Nama     : ${input.name}`,
    `WhatsApp : ${input.whatsapp}`,
    '',
    'Kebutuhan:',
    input.message,
  ].join('\n')

const sendEmail = async (input: ContactInput): Promise<void> => {
  const apiKey = process.env.RESEND_API_KEY
  const from = process.env.RESEND_FROM
  const to = process.env.RESEND_TO

  if (!apiKey || !from || !to) return

  await fetch('https://api.resend.com/emails', {
    method: 'POST',
    headers: {
      Authorization: `Bearer ${apiKey}`,
      'Content-Type': 'application/json',
    },
    body: JSON.stringify({
      from,
      to: [to],
      // Replying goes straight back to the sender rather than to the form.
      reply_to: `${input.name} <${STUDIO_EMAIL}>`,
      subject: `Lead baru — ${input.name}`,
      text: composeText(input),
    }),
  })
}

const sendWhatsApp = async (input: ContactInput): Promise<void> => {
  const token = process.env.FONNTE_TOKEN
  const target = process.env.FONNTE_TARGET

  if (!token || !target) return

  await fetch('https://api.fonnte.com/send', {
    method: 'POST',
    headers: {
      Authorization: token,
      'Content-Type': 'application/json',
    },
    body: JSON.stringify({ target, message: composeText(input) }),
  })
}

/* ------------------------------------------------------------------ handler */

export type ContactResult = {
  readonly status: number
  readonly body: Record<string, unknown>
}

export const clientKeyFrom = (headers: {
  readonly forwardedFor?: string | undefined
  readonly realIp?: string | undefined
}): string => headers.forwardedFor?.split(',')[0]?.trim() ?? headers.realIp ?? 'unknown'

export const handleContact = async (
  payload: unknown,
  clientKey: string,
): Promise<ContactResult> => {
  if (isRateLimited(clientKey)) {
    return {
      status: 429,
      body: { error: 'Terlalu banyak pengiriman. Coba lagi sebentar.' },
    }
  }

  const parsed = contactSchema.safeParse(payload)

  if (!parsed.success) {
    return {
      status: 422,
      body: { error: 'Data belum lengkap.', issues: parsed.error.flatten().fieldErrors },
    }
  }

  // Honeypot filled means a bot. Answer 200 so it never learns why nothing
  // arrives, and drop the submission.
  if (parsed.data.company) {
    return { status: 200, body: { ok: true } }
  }

  const results = await Promise.allSettled([sendEmail(parsed.data), sendWhatsApp(parsed.data)])

  for (const result of results) {
    if (result.status === 'rejected') {
      console.error('[kontak] notifikasi gagal terkirim', result.reason)
    }
  }

  if (!process.env.RESEND_API_KEY && !process.env.FONNTE_TOKEN) {
    console.info(
      '[kontak] tidak ada penyedia notifikasi terpasang. Isi pesan:\n',
      composeText(parsed.data),
    )
  }

  return { status: 200, body: { ok: true } }
}

export const MALFORMED_BODY: ContactResult = {
  status: 400,
  body: { error: 'Format permintaan tidak dikenali.' },
}
