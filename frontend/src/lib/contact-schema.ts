import { z } from 'zod'

/**
 * Shared between the form and the route handler, so client and server can
 * never disagree about what a valid enquiry looks like.
 *
 * Three fields, no more. Every additional field lowers submissions, and
 * qualification belongs in the conversation, not in the form.
 */
export const contactSchema = z.object({
  name: z
    .string()
    .trim()
    .min(2, 'Nama perlu diisi minimal 2 karakter.')
    .max(120, 'Nama terlalu panjang.'),
  whatsapp: z
    .string()
    .trim()
    .min(8, 'Nomor WhatsApp perlu diisi.')
    .max(24, 'Nomor WhatsApp terlalu panjang.')
    .regex(/^[0-9+()\-\s]+$/, 'Nomor WhatsApp hanya boleh berisi angka dan tanda + ( ) -.')
    .refine(
      (value) => /^(\+?62|0)/.test(value.replace(/[\s()-]/g, '')),
      'Nomor WhatsApp perlu diawali 08 atau +62.',
    ),
  message: z
    .string()
    .trim()
    .min(20, 'Ceritakan kebutuhannya minimal 20 karakter — supaya jawaban saya bisa relevan.')
    .max(2000, 'Pesan terlalu panjang. Ringkas dulu, detailnya bisa lewat WhatsApp.'),
  /** Honeypot. Real people never fill this; bots almost always do. */
  company: z.string().max(0).optional().default(''),
})

export type ContactInput = z.infer<typeof contactSchema>

export type ContactFieldErrors = Partial<Record<keyof ContactInput, string>>
