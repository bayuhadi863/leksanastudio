import { ButtonLink } from '@/components/ui/Button'
import { routes } from '@/config/routes'
import { site } from '@/config/site'
import { whatsappLink } from '@/lib/whatsapp'

type Props = {
  readonly title?: string
  readonly body?: string
  readonly whatsappMessage: string
}

export function CtaBlock({
  title = 'Ceritakan dulu masalahnya',
  body = 'Obrolan awal 30 menit, tanpa biaya dan tanpa kewajiban. Kalau ternyata kebutuhan Anda bukan bidang saya, saya katakan sejak awal.',
  whatsappMessage,
}: Props) {
  return (
    <section className="section--tight border-line border-t">
      <div className="shell">
        <div className="max-w-[var(--measure)]">
          <h2 className="type-h2">{title}</h2>
          <p className="type-lead mt-4">{body}</p>

          <div className="mt-8 flex flex-col gap-3 sm:flex-row">
            <ButtonLink href={whatsappLink(whatsappMessage)} size="large">
              Diskusi lewat WhatsApp
            </ButtonLink>
            <ButtonLink href={routes.contact} variant="secondary" size="large">
              Kirim lewat formulir
            </ButtonLink>
          </div>

          <p className="type-small text-muted mt-5">
            Dibalas dalam {site.promises.replyWithinHours} jam pada jam kerja.
          </p>
        </div>
      </div>
    </section>
  )
}
