import { CtaBlock } from '@/components/blocks/CtaBlock'
import { FaqList } from '@/components/blocks/FaqList'
import { OutOfScopeList } from '@/components/blocks/OutOfScopeList'
import { PhaseList } from '@/components/blocks/PhaseList'
import { PricingTable } from '@/components/blocks/PricingTable'
import { SectionHeading } from '@/components/blocks/SectionHeading'
import { JsonLd } from '@/components/layout/JsonLd'
import { WhatsAppBar } from '@/components/layout/WhatsAppBar'
import { Label } from '@/components/ui/Label'
import { Note } from '@/components/ui/Note'
import { homeFaq, outOfScope } from '@/config/copy'
import {
  addOns,
  businessPackages,
  corporatePackages,
  paymentTerms,
  systemPhases,
} from '@/config/packages'
import { routes } from '@/config/routes'
import { formatIDR } from '@/config/site'
import { PRICE_FLOOR } from '@/config/packages'
import { usePageMeta } from '@/lib/seo'
import { breadcrumbSchema, faqSchema } from '@/lib/structured-data'
import { whatsappMessages } from '@/lib/whatsapp'

const pricingFaq = [
  {
    question: 'Kenapa harganya ditampilkan?',
    answer:
      'Karena menyembunyikan harga tidak menyaring prospek yang menawar, tetapi menyaring prospek yang serius. Kalau anggaran Anda di bawah angka terkecil di halaman ini, saya lebih suka Anda tahu sekarang daripada setelah tiga kali pertemuan.',
  },
  {
    question: 'Bisakah harganya turun kalau ruang lingkupnya dikecilkan?',
    answer:
      'Bisa, dan itu memang cara yang benar. Yang tidak saya lakukan adalah menurunkan harga untuk ruang lingkup yang sama — karena itu berarti harga sebelumnya tidak jujur.',
  },
  {
    question: 'Apakah ada biaya bulanan?',
    answer:
      'Untuk website bisnis, biaya hosting biasanya nol sampai puluhan ribu rupiah per bulan, dibayar langsung oleh Anda ke penyedianya. Retainer bulanan sifatnya opsional dan hanya masuk akal kalau ada perubahan rutin.',
  },
  ...homeFaq.slice(3, 5),
]

export function PricingPage() {
  usePageMeta({
    title: 'Harga',
    description:
      'Paket dan rentang harga untuk website bisnis, company profile, dan sistem web — lengkap dengan termin pembayaran dan apa yang tidak termasuk.',
    path: routes.pricing,
  })

  return (
    <>
      <section className="shell pt-14 pb-16 lg:pt-24 lg:pb-20">
        <div className="annotation">
          <div className="annotation__body">
            <h1 className="type-h1">Harga ditampilkan, bukan disembunyikan</h1>
            <p className="type-lead mt-6">
              Angka di halaman ini berlaku untuk ruang lingkup yang tertulis. Kalau kebutuhan Anda
              berbeda, angkanya berubah — dan saya sampaikan sebelum dikerjakan, bukan sesudah.
            </p>
          </div>
          <Note>
            Lantai harga saya {formatIDR(PRICE_FLOOR)}. Di bawah itu yang bisa saya kerjakan
            hanyalah memasang template dan mengganti teksnya, dan saya tidak bisa
            mempertanggungjawabkan hasilnya.
          </Note>
        </div>
      </section>

      <section className="section--tight border-line border-t">
        <div className="shell">
          <SectionHeading
            eyebrow="Website bisnis"
            title="Untuk klinik, jasa lokal, dan UKM"
            lead="Dibangun dari inti produk yang sudah matang, jadi waktu terpakai untuk hal yang khas bisnis Anda."
          />
          <div className="mt-12">
            <PricingTable
              packages={businessPackages}
              whatsappIntro="Halo, saya ingin tanya soal website bisnis."
            />
          </div>
        </div>
      </section>

      <section className="section--tight border-line border-t">
        <div className="shell">
          <SectionHeading
            eyebrow="Company profile & situs institusi"
            title="Desain dibuat dari nol"
            lead="Bukan tema yang diganti warnanya. Itu yang membuat selisih harga dan selisih waktunya."
          />
          <div className="mt-12">
            <PricingTable
              packages={corporatePackages}
              whatsappIntro="Halo, saya ingin tanya soal company profile."
            />
          </div>
        </div>
      </section>

      <section className="section--tight border-line border-t">
        <div className="shell">
          <div className="annotation">
            <div className="annotation__body">
              <SectionHeading
                eyebrow="Sistem & aplikasi web"
                title="Dijual per tahap, bukan per paket"
                lead="Sistem tidak bisa diberi harga pasti sebelum ruang lingkupnya jelas. Tahap pertama menyelesaikan itu, dan hasilnya milik Anda."
              />
            </div>
            <Note>
              Lokakarya berbayar menyaring dua hal sekaligus: prospek yang belum serius, dan
              estimasi yang asal-asalan. Estimasi asal-asalan adalah awal dari proyek yang meleset
              dua kali lipat.
            </Note>
          </div>

          <div className="mt-12">
            <PhaseList phases={systemPhases} />
          </div>
        </div>
      </section>

      <section className="section--tight border-line border-t">
        <div className="shell">
          <SectionHeading
            eyebrow="Tambahan"
            title="Disebut setelah kebutuhan utamanya jelas"
            lead="Menyebut semuanya di depan hanya memperumit keputusan dan menunda hal yang lebih penting."
          />

          <div className="scroll-x mt-10">
            <table className="w-full min-w-2xl text-left">
              <caption className="sr-only">Daftar layanan tambahan dan harganya</caption>
              <thead>
                <tr className="border-line border-b">
                  <th scope="col" className="pb-3">
                    <Label as="span">Tambahan</Label>
                  </th>
                  <th scope="col" className="pb-3">
                    <Label as="span">Harga</Label>
                  </th>
                  <th scope="col" className="pb-3">
                    <Label as="span">Berlaku untuk</Label>
                  </th>
                </tr>
              </thead>
              <tbody>
                {addOns.map((addOn) => (
                  <tr key={addOn.name} className="border-line border-b">
                    <th scope="row" className="type-small py-4 pr-8 font-semibold">
                      {addOn.name}
                    </th>
                    <td className="type-small numeric py-4 pr-8">{addOn.price}</td>
                    <td className="type-small text-muted py-4">{addOn.appliesTo}</td>
                  </tr>
                ))}
              </tbody>
            </table>
          </div>
        </div>
      </section>

      <section className="section--tight border-line border-t">
        <div className="shell">
          <SectionHeading eyebrow="Pembayaran" title="Termin" />

          <div className="scroll-x mt-10">
            <table className="w-full min-w-lg text-left">
              <caption className="sr-only">Termin pembayaran per jenis pekerjaan</caption>
              <thead>
                <tr className="border-line border-b">
                  <th scope="col" className="pb-3">
                    <Label as="span">Pekerjaan</Label>
                  </th>
                  <th scope="col" className="pb-3">
                    <Label as="span">Termin</Label>
                  </th>
                </tr>
              </thead>
              <tbody>
                {paymentTerms.map((term) => (
                  <tr key={term.scope} className="border-line border-b">
                    <th scope="row" className="type-small py-4 pr-8 font-semibold">
                      {term.scope}
                    </th>
                    <td className="type-small text-muted py-4">{term.schedule}</td>
                  </tr>
                ))}
              </tbody>
            </table>
          </div>
        </div>
      </section>

      <section className="section--tight border-line border-t">
        <div className="shell">
          <SectionHeading eyebrow="Batas" title="Yang tidak saya kerjakan" />
          <div className="mt-10">
            <OutOfScopeList items={outOfScope} />
          </div>
        </div>
      </section>

      <section className="section--tight border-line border-t">
        <div className="shell">
          <SectionHeading eyebrow="Pertanyaan" title="Soal harga" />
          <div className="mt-10">
            <FaqList items={pricingFaq} />
          </div>
        </div>
      </section>

      <CtaBlock
        title="Belum yakin masuk paket yang mana?"
        body="Ceritakan kebutuhannya lewat WhatsApp. Kalau ternyata paket yang lebih murah sudah cukup, saya akan mengatakannya."
        whatsappMessage={whatsappMessages.pricing}
      />

      <WhatsAppBar message={whatsappMessages.pricing} />

      <JsonLd
        data={[faqSchema(pricingFaq), breadcrumbSchema([{ label: 'Harga', path: routes.pricing }])]}
      />
    </>
  )
}
