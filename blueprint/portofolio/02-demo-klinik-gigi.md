# Studi Kasus 02 — Demo Website Klinik Gigi

> **Status:** kerangka. Diisi setelah demo induk dibangun.
> Blueprint produknya: [`../../02-produk-klinik-gigi/`](../../02-produk-klinik-gigi/)

---

## Metadata

| Field | Nilai |
|---|---|
| **Label** | `Produk sendiri` ← wajib, jangan disamarkan |
| **Jenis** | Website bisnis lokal — produk terproduktisasi |
| **Peran** | Perancang produk & pengembang |
| **Durasi** | [isi] |
| **Stack** | Next.js 15 · TypeScript · Tailwind CSS · shadcn/ui · Vercel |
| **Tautan** | Demo hidup: [isi] |

---

## Kenapa Studi Kasus Ini Ditulis Berbeda

Ini bukan pekerjaan klien, dan tidak boleh ditulis seolah begitu. Tapi ia menjual
hal yang **tidak bisa** dijual oleh studi kasus P3M PENS:

| P3M PENS menjual | Demo klinik gigi menjual |
|---|---|
| Kedalaman & tanggung jawab sistem | **Kecepatan** dan **kesiapan** |
| "Saya sanggup menangani yang rumit" | "Untuk kebutuhan Anda, sebagian besar sudah jadi" |
| Membenarkan harga Lini C | Membenarkan **waktu pengerjaan** Lini A |
| Bukti tunggal, dalam | Bukti hidup, bisa diklik sekarang juga |

**Sudut naskahnya bukan "lihat website buatan saya" melainkan "lihat bagaimana saya
membangun produk, bukan proyek satuan".** Itu cerita yang lebih sulit ditiru dan
lebih mahal.

---

## Kerangka Naskah

### Tiga angka
| **[N]** | **[N] menit** | **[N]/100** |
|---|---|---|
| tema bisa ditukar | dari demo ke versi ber-brand klien | skor Lighthouse di HP |

### Masalah (dari sisi pemilik klinik)
- Pasien mencari lewat HP dan menyerah kalau nomor WhatsApp tidak langsung terlihat
- Iklan diarahkan ke halaman depan, bukan ke halaman layanan yang diiklankan
- Website lama tidak bisa diperbarui sendiri
- [lengkapi dari [`../../02-produk-klinik-gigi/02-riset-pasar.md`](../../02-produk-klinik-gigi/02-riset-pasar.md)]

### Batasan yang saya pilih sendiri
Blok "batasan" di sini bukan kendala dari luar — melainkan **aturan yang saya
tetapkan supaya produknya bisa dipakai ulang.** Sampaikan begitu, jujur.

- Tidak boleh ada satu pun data klinik yang ditulis langsung di dalam komponen
- Satu basis kode harus melayani klinik mana pun tanpa dicabang
- Ganti brand lengkap harus selesai di bawah 30 menit, bisa diukur
- Skor performa harus tetap hijau setelah semua konten terisi penuh

### Keputusan
1. Arsitektur digerakkan satu file konfigurasi — [alasan + alternatif yang ditolak]
2. Tema sebagai token CSS, bukan sebagai cabang komponen — [alasan]
3. Booking lewat WhatsApp dengan pesan kontekstual, bukan form — [alasan: perilaku nyata pasien]
4. [lengkapi dari [`../../02-produk-klinik-gigi/07-arsitektur-teknis.md`](../../02-produk-klinik-gigi/07-arsitektur-teknis.md)]

### Hasil
- Demo hidup yang bisa diklik — **ini aset utamanya**, bukan tulisannya
- Rekaman layar: mengubah demo jadi versi ber-brand klinik lain, tanpa potongan
- Tangkapan layar tiga tema berdampingan
- Tangkapan layar hasil Lighthouse, dengan tanggal

### Yang saya lakukan berbeda kalau mengulang
[Isi jujur setelah membangun. Kalau kosong, hapus bloknya — jangan diisi
kelemahan palsu.]

---

## Aturan Kejujuran (Wajib)

1. Kartu portofolio dan halaman studi kasus **wajib** berlabel `Produk sendiri`.
2. Jangan menulis "klien puas", "meningkatkan pasien 40%", atau testimoni apa pun.
   Tidak ada klien di proyek ini.
3. Demo memakai brand fiktif milik sendiri. Aturan lengkapnya:
   [`../../02-produk-klinik-gigi/12-etika-legal.md`](../../02-produk-klinik-gigi/12-etika-legal.md).
4. Halaman demo tetap wajib punya penanda "ini demo" dan halaman
   "Tentang Demo Ini" — supaya orang yang mendarat dari pencarian tidak mengira
   klinik itu nyata.

**Satu paragraf pembuka yang menyelesaikan semuanya sekaligus:**

> Ini bukan pekerjaan untuk klien. Ini produk yang saya bangun sendiri supaya klinik
> berikutnya tidak perlu menunggu berbulan-bulan. Semua isinya fiktif — nama, dokter,
> harga, testimoni. Yang nyata adalah kodenya, kecepatannya, dan berapa lama waktu
> yang dibutuhkan untuk mengubahnya menjadi milik Anda.
