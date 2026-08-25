import { ButtonLink } from '@/components/ui/Button'
import { Label } from '@/components/ui/Label'
import { routes } from '@/config/routes'
import { site } from '@/config/site'
import { usePageMeta } from '@/lib/seo'
import { whatsappLink, whatsappMessages } from '@/lib/whatsapp'

/**
 * A real URL, not a state flag — so the conversion can be tracked and used as
 * an advertising goal later. Kept out of the index for the same reason.
 */
export function ContactThanksPage() {
  usePageMeta({
    title: 'Pesan terkirim',
    description: 'Pesan Anda sudah masuk.',
    path: routes.contactThanks,
    noIndex: true,
  })

  return (
    <section className="shell flex min-h-[70vh] items-center py-20">
      <div className="document">
        <Label as="p">Pesan terkirim</Label>

        <h1 className="type-h1 mt-5">Sudah masuk. Terima kasih.</h1>

        <p className="type-lead mt-6">
          Saya balas dalam {site.promises.replyWithinHours} jam pada jam kerja. Kalau Anda mengirim
          di luar jam kerja, balasannya datang pagi berikutnya.
        </p>

        <div className="mt-9 flex flex-col gap-3 sm:flex-row">
          <ButtonLink href={whatsappLink(whatsappMessages.default)} size="large">
            Lanjutkan lewat WhatsApp
          </ButtonLink>
          <ButtonLink href={routes.work} variant="secondary" size="large">
            Lihat portofolio
          </ButtonLink>
        </div>

        <p className="type-small border-line text-muted mt-10 border-t pt-5">
          Kalau kebutuhannya mendesak, WhatsApp jauh lebih cepat daripada menunggu balasan surel.
        </p>
      </div>
    </section>
  )
}
