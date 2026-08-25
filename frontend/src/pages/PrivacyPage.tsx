import { Label } from '@/components/ui/Label'
import { routes } from '@/config/routes'
import { site } from '@/config/site'
import { usePageMeta } from '@/lib/seo'

const LAST_UPDATED = '23 Agustus 2026'

export function PrivacyPage() {
  usePageMeta({
    title: 'Kebijakan privasi',
    description: 'Data apa yang dikumpulkan situs ini, untuk apa, dan berapa lama disimpan.',
    path: routes.privacy,
  })

  return (
    <section className="shell py-16 lg:py-24">
      <div className="document copy">
        <Label as="p">Diperbarui {LAST_UPDATED}</Label>
        <h1 className="type-h1 mt-5">Kebijakan privasi</h1>

        <p className="type-lead mt-6">
          Situs ini mengumpulkan sesedikit mungkin. Halaman ini menjelaskan apa saja yang
          dikumpulkan, untuk apa, dan berapa lama disimpan.
        </p>

        <h2>Data yang Anda kirimkan sendiri</h2>
        <p>
          Kalau Anda mengisi formulir kontak, saya menerima tiga hal: nama, nomor WhatsApp, dan isi
          pesan Anda. Ketiganya dipakai hanya untuk membalas dan menindaklanjuti pertanyaan Anda.
        </p>
        <p>
          Data itu dikirim ke alamat surel dan nomor WhatsApp saya lewat penyedia layanan pengiriman
          pesan. Saya tidak menjualnya, tidak menukarnya, dan tidak memakainya untuk mengirim pesan
          pemasaran massal.
        </p>
        <p>
          Kalau setelah tiga bulan tidak ada tindak lanjut, percakapan dan datanya saya hapus. Anda
          juga bisa meminta penghapusan kapan saja lewat{' '}
          <a href={`mailto:${site.email}`}>{site.email}</a>.
        </p>

        <h2>Yang tidak dilakukan situs ini</h2>
        <ul>
          <li>Tidak memasang piksel pelacak pihak ketiga</li>
          <li>Tidak memasang widget obrolan atau sematan media sosial</li>
          <li>Tidak menampilkan iklan</li>
          <li>Tidak membuat profil perilaku pengunjung</li>
        </ul>

        <h2>Cookie</h2>
        <p>
          Situs ini tidak memasang cookie untuk pelacakan, dan tidak memasang cookie sama sekali.
        </p>
        <p>
          Satu hal yang disimpan: kalau Anda memilih tampilan terang atau gelap lewat kontrol di
          bagian bawah halaman, pilihan itu disimpan di penyimpanan lokal peramban Anda. Isinya
          hanya kata <code>light</code> atau <code>dark</code>, tidak pernah dikirim ke server mana
          pun, dan hilang begitu Anda menghapus data situs. Kalau Anda membiarkannya di
          &ldquo;Sistem&rdquo;, tidak ada apa pun yang disimpan.
        </p>

        <h2>Analitik</h2>
        <p>
          Bila analitik dipasang, yang dipakai adalah layanan yang mengukur secara agregat tanpa
          cookie dan tanpa mengidentifikasi individu. Angka yang saya lihat adalah jumlah kunjungan
          per halaman, bukan siapa yang berkunjung.
        </p>

        <h2>Layanan pihak ketiga</h2>
        <p>
          Situs ini dijalankan di penyedia hosting yang, seperti semua penyedia hosting, mencatat
          alamat IP permintaan di log servernya untuk keperluan keamanan dan operasional. Huruf
          ditampilkan dari berkas yang dilayani oleh situs ini sendiri, bukan dari server pihak
          ketiga.
        </p>

        <h2>Hak Anda</h2>
        <p>
          Anda berhak meminta salinan data yang saya simpan tentang Anda, meminta koreksinya, dan
          meminta penghapusannya. Kirim permintaannya ke{' '}
          <a href={`mailto:${site.email}`}>{site.email}</a> dan saya tindak lanjuti dalam tujuh hari
          kerja.
        </p>

        <h2>Perubahan</h2>
        <p>
          Kalau kebijakan ini berubah, tanggal di bagian atas halaman ikut berubah. Versi sebelumnya
          tidak disimpan secara publik.
        </p>
      </div>
    </section>
  )
}
