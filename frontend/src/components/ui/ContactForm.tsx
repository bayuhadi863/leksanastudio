import { useId, useState, type FormEvent } from 'react'
import { useNavigate } from 'react-router-dom'

import { Button } from '@/components/ui/Button'
import { routes } from '@/config/routes'
import { contactSchema, type ContactFieldErrors } from '@/lib/contact-schema'
import { cn } from '@/lib/cn'

type Status = 'idle' | 'sending' | 'error'

const fieldClasses = (hasError: boolean) =>
  cn(
    'w-full rounded-[var(--radius-control)] border bg-surface px-4 py-3',
    'transition-colors duration-150 ease-out',
    'focus:border-accent focus:ring-3 focus:ring-accent/20 focus:outline-none',
    hasError ? 'border-danger' : 'border-muted',
  )

export function ContactForm() {
  const navigate = useNavigate()
  const baseId = useId()
  const [status, setStatus] = useState<Status>('idle')
  const [errors, setErrors] = useState<ContactFieldErrors>({})
  const [formError, setFormError] = useState<string | null>(null)

  const fieldId = (name: string) => `${baseId}-${name}`
  const errorId = (name: string) => `${baseId}-${name}-error`

  const handleSubmit = async (event: FormEvent<HTMLFormElement>) => {
    event.preventDefault()
    setFormError(null)

    const formData = new FormData(event.currentTarget)
    const parsed = contactSchema.safeParse({
      name: formData.get('name'),
      whatsapp: formData.get('whatsapp'),
      message: formData.get('message'),
      company: formData.get('company') ?? '',
    })

    if (!parsed.success) {
      const nextErrors: ContactFieldErrors = {}
      for (const issue of parsed.error.issues) {
        const key = issue.path[0]
        if (typeof key === 'string' && !(key in nextErrors)) {
          nextErrors[key as keyof ContactFieldErrors] = issue.message
        }
      }
      setErrors(nextErrors)
      setStatus('error')
      return
    }

    setErrors({})
    setStatus('sending')

    try {
      const response = await fetch('/api/kontak', {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(parsed.data),
      })

      if (!response.ok) {
        throw new Error(`Gagal mengirim (${response.status})`)
      }

      navigate(routes.contactThanks)
    } catch {
      setStatus('error')
      setFormError(
        'Pesan belum terkirim. Coba lagi sebentar, atau langsung lewat WhatsApp supaya tidak tertunda.',
      )
    }
  }

  const sending = status === 'sending'

  return (
    <form onSubmit={handleSubmit} noValidate className="max-w-[var(--measure)]">
      <div className="grid gap-6">
        <div>
          <label htmlFor={fieldId('name')} className="block font-semibold">
            Nama
          </label>
          <input
            id={fieldId('name')}
            name="name"
            type="text"
            autoComplete="name"
            required
            aria-invalid={Boolean(errors.name)}
            aria-describedby={errors.name ? errorId('name') : undefined}
            className={cn(fieldClasses(Boolean(errors.name)), 'mt-2')}
          />
          {/* Space is reserved so an appearing error never shifts the layout. */}
          <p
            id={errorId('name')}
            className="type-small text-danger min-h-6 pt-1"
            aria-live="polite"
          >
            {errors.name}
          </p>
        </div>

        <div>
          <label htmlFor={fieldId('whatsapp')} className="block font-semibold">
            Nomor WhatsApp
          </label>
          <input
            id={fieldId('whatsapp')}
            name="whatsapp"
            type="tel"
            inputMode="tel"
            autoComplete="tel"
            placeholder="0812 3456 7890"
            required
            aria-invalid={Boolean(errors.whatsapp)}
            aria-describedby={errors.whatsapp ? errorId('whatsapp') : undefined}
            className={cn(fieldClasses(Boolean(errors.whatsapp)), 'numeric mt-2')}
          />
          <p
            id={errorId('whatsapp')}
            className="type-small text-danger min-h-6 pt-1"
            aria-live="polite"
          >
            {errors.whatsapp}
          </p>
        </div>

        <div>
          <label htmlFor={fieldId('message')} className="block font-semibold">
            Apa yang sedang Anda butuhkan?
          </label>
          <textarea
            id={fieldId('message')}
            name="message"
            rows={6}
            required
            aria-invalid={Boolean(errors.message)}
            aria-describedby={errors.message ? errorId('message') : undefined}
            className={cn(fieldClasses(Boolean(errors.message)), 'mt-2 resize-y')}
          />
          <p
            id={errorId('message')}
            className="type-small text-danger min-h-6 pt-1"
            aria-live="polite"
          >
            {errors.message}
          </p>
        </div>

        {/* Honeypot — visually and semantically hidden from real users. */}
        <div aria-hidden="true" className="absolute -left-[9999px] h-0 w-0 overflow-hidden">
          <label htmlFor={fieldId('company')}>Perusahaan</label>
          <input
            id={fieldId('company')}
            name="company"
            type="text"
            tabIndex={-1}
            autoComplete="off"
          />
        </div>
      </div>

      <div className="mt-2 flex flex-wrap items-center gap-4">
        <Button type="submit" size="large" disabled={sending} className="min-w-52">
          {sending ? 'Mengirim…' : 'Kirim pesan'}
        </Button>
      </div>

      {formError ? (
        <p role="alert" className="type-small text-danger mt-4">
          {formError}
        </p>
      ) : null}
    </form>
  )
}
