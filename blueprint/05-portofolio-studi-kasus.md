# 05 — Portofolio & Studi Kasus

## 5.1 Kenapa Studi Kasus, Bukan Galeri

Galeri tangkapan layar adalah portofolio desainer. Kamu bukan desainer — kamu menjual
**penilaian teknis dan tanggung jawab.** Keduanya tidak terlihat di tangkapan layar.

| | Galeri screenshot | Studi kasus |
|---|---|---|
| Yang dinilai prospek | Selera visual | Cara berpikir |
| Bisa ditiru | Ya, dalam semalam | Tidak — butuh benar-benar mengerjakannya |
| Membenarkan harga tinggi | Tidak | **Ya** |
| Bertahan sebagai konten | Beberapa detik | Berbulan-bulan (SEO, LinkedIn, kiriman ke prospek) |
| Menjawab "kok mahal?" | Tidak | **Ya, tanpa perlu berdebat** |

**Satu studi kasus yang ditulis serius mengalahkan dua belas tangkapan layar.**
Untuk portofolio yang masih tipis, ini juga solusi masalah kuantitas: dua studi kasus
mendalam terlihat lebih meyakinkan daripada delapan kartu dangkal.

---

## 5.2 Anatomi Studi Kasus (Tujuh Blok)

Urutan ini bukan gaya — ini urutan cara prospek menilai risiko.

```
┌────────────────────────────────────────────────────────────┐
│ 1. RINGKASAN — dibaca dalam 10 detik                       │
│    Klien · jenis proyek · durasi · peran saya · stack       │
│    + 3 angka kunci dalam kotak besar                        │
├────────────────────────────────────────────────────────────┤
│ 2. MASALAH — keadaan sebelum, dalam bahasa klien            │
│    Bukan "belum punya sistem". Melainkan apa yang           │
│    sebenarnya sakit: berapa jam terbuang, apa yang hilang   │
├────────────────────────────────────────────────────────────┤
│ 3. BATASAN — yang membuat proyek ini tidak biasa            │
│    ★ Blok terpenting dan paling sering dilewatkan.          │
│    Ini yang membuktikan kamu menghadapi kenyataan,          │
│    bukan mengerjakan latihan.                               │
├────────────────────────────────────────────────────────────┤
│ 4. KEPUTUSAN — 3-5 keputusan penting + alasan + alternatif  │
│    Format tetap: "Saya memilih X karena Y, walaupun Z"      │
├────────────────────────────────────────────────────────────┤
│ 5. HASIL — angka, tangkapan layar, sebelum/sesudah          │
├────────────────────────────────────────────────────────────┤
│ 6. YANG SAYA LAKUKAN BERBEDA KALAU MENGULANG                │
│    Jujur. Ini menaikkan kepercayaan, bukan menurunkan       │
├────────────────────────────────────────────────────────────┤
│ 7. CTA — "Punya masalah serupa? Mari bicara"                │
└────────────────────────────────────────────────────────────┘
```

### Blok 3 dan 6 adalah pembedanya

Semua orang bisa menulis blok 1, 2, 5. Blok **3 (batasan)** dan **6 (yang akan
diubah)** hanya bisa ditulis oleh orang yang benar-benar mengerjakan proyeknya.
Prospek teknis mengenali ini seketika, dan prospek non-teknis merasakannya sebagai
kejujuran. Jangan pernah menghapus dua blok itu untuk menghemat tempat.

---

## 5.3 Aturan Angka

**Angka adalah alat kepercayaan paling kuat — dan cara tercepat kehilangan
kepercayaan kalau dikarang.**

| Jenis angka | Boleh dipakai kalau | Cara menulis |
|---|---|---|
| **Ukuran sistem** (modul, entitas, endpoint, baris kode) | Bisa dihitung ulang dari repositori | "30 modul frontend, 34 entitas basis data" |
| **Performa** (Lighthouse, waktu muat) | Ada tangkapan layar hasil tes | **Wajib** sertakan sumber, tanggal, dan **Desktop/Mobile**. Lihat §5.3.1 |
| **Waktu pengerjaan** | Bisa dicek dari riwayat git | "Enam minggu, dikerjakan sendiri" |
| **Dampak operasional** (jam kerja hemat, kesalahan berkurang) | **Klien yang menyebutkan angkanya** | Kutip klien, jangan hitung sendiri |
| **Dampak bisnis** (lead naik, omzet naik) | Ada data analitik yang bisa ditunjukkan | Sebutkan periode & basisnya |

### Tiga larangan

1. **Jangan mengarang persentase.** "Efisiensi naik 40%" tanpa dasar adalah kebohongan
   yang akan ditanyakan detailnya oleh prospek serius — dan kamu tidak akan punya jawaban.
2. **Jangan mengklaim dampak yang tidak kamu ukur.** Kalau tidak ada data, tulis
   apa yang benar-benar berubah secara kualitatif.
3. **Jangan menyembunyikan bahwa demo adalah demo.** Lihat §5.6.

**Kalau tidak ada angka dampak sama sekali** — sangat wajar untuk proyek internal —
ganti dengan **angka kompleksitas**. "5 jenis pengajuan dengan alur verifikasi berbeda"
adalah angka jujur yang tetap mengesankan.

### 5.3.1 Aturan Khusus Skor Lighthouse

Skor Lighthouse adalah angka paling mudah diverifikasi ulang oleh prospek — dan
karena itu paling berbahaya kalau disajikan tanpa hati-hati.

| Aturan | Isi |
|---|---|
| **Sumber tunggal: PageSpeed Insights** | Bukan DevTools. Run DevTools tercemar oleh ekstensi, beban mesin, dan data tersimpan di peramban (IndexedDB, cache, service worker). Selisih 15-25 poin adalah hal biasa |
| **Label wajib tiga bagian** | Sumber · tanggal · **Desktop atau Mobile**. Standar industri membaca skor tanpa label sebagai mobile — jadi menampilkan skor desktop tanpa menyebutnya adalah menyesatkan |
| **Boleh menampilkan satu strategi saja** | Menampilkan desktop tanpa mobile itu sah, selama berlabel. Studi kasus adalah dokumen penjualan, bukan laporan audit |
| **Dilarang menggeneralisasi** | "Lighthouse hijau", "cepat di semua perangkat", "skor sempurna" — terlarang kecuali **keempat kategori di kedua strategi** memang hijau |
| **Siapkan jawaban untuk angka yang tidak ditampilkan** | Kalau ditanya, sebut angkanya langsung, sebut penyebabnya, sebut apa yang akan dilakukan. Berkelit di titik ini menghapus kredibilitas seluruh studi kasus |
| **Arsipkan tangkapan layarnya** | Skor berubah seiring waktu dan seiring perubahan yang dilakukan orang lain. Tanggal di label melindungimu |

**Aset yang kamu kendalikan penuh berlaku aturan berbeda.** Situs Leksana Studio dan
demo klinik gigi tidak boleh berlindung di balik label — keduanya di-hosting olehmu,
jadi targetnya 100 di mobile. Lihat
[07-arsitektur-teknis.md](07-arsitektur-teknis.md) §7.6.

Penerapan lengkap pada studi kasus pertama:
[portofolio/01-p3m-pens.md](portofolio/01-p3m-pens.md) → *Kebijakan Skor Lighthouse*.

---

## 5.4 Aturan Visual

| Aset | Aturan |
|---|---|
| Tangkapan layar | Data asli **disamarkan** kalau berisi nama orang atau data pribadi. Ganti dengan nama fiktif, jangan diblur (blur terlihat seperti menyembunyikan sesuatu) |
| Diagram arsitektur | Wajib ada minimal satu. Diagram adalah bukti bahwa sistemnya punya bentuk, bukan tumpukan file |
| Cuplikan kode | Boleh, maksimal 15 baris, harus mengilustrasikan **keputusan** — bukan pamer sintaks |
| Video | 30–60 detik menelusuri alur utama. Konversi jauh lebih tinggi dari screenshot statis |
| Foto orang | Tidak ada foto stok. Sama sekali |
| Bingkai perangkat | Tampilkan tampilan HP berdampingan dengan desktop. Sebagian besar prospek menilai dari HP |

---

## 5.5 Izin & Kerahasiaan Klien

Kesalahan di sini merusak hubungan yang butuh bertahun-tahun dibangun.

### Urutan yang benar

1. **Minta izin tertulis** — WhatsApp atau email cukup, yang penting terekam.
   Sebut secara spesifik: nama institusi, tangkapan layar, angka mana yang disebut.
2. **Tawarkan pratinjau naskah** sebelum publikasi. Hampir semua klien menyetujui
   setelah membaca, dan tawaran ini sendiri membangun kepercayaan.
3. **Kalau tidak ada jawaban dalam 2 minggu** — publikasikan versi anonim (§5.5.1).
   Jangan menunggu selamanya, dan jangan memaksa.

### 5.5.1 Versi anonim — siapkan sejak awal

| Yang disamarkan | Yang tetap boleh disebut |
|---|---|
| Nama institusi → "sebuah unit penelitian di politeknik negeri" | Jenis institusi, skala, kompleksitas |
| Logo & branding klien | Tangkapan layar dengan branding diganti netral |
| Data pribadi pengguna | Struktur data, jumlah entitas, alur |
| Angka anggaran | Durasi, ukuran tim, stack |

**Versi anonim hampir sama kuatnya.** Yang menjual bukan nama klien — yang menjual
adalah blok batasan dan blok keputusan.

### Yang tidak boleh, bahkan dengan izin

- Menampilkan data pribadi pengguna nyata (nama, NIP, email, dokumen)
- Membocorkan kredensial, endpoint internal, atau detail yang membantu penyerang
- Menyebut angka anggaran institusi tanpa persetujuan tertulis eksplisit
- Mengunggah repositori privat klien ke GitHub publik

### Klausul yang harus ada di kontrak berikutnya

> "Penyedia berhak menampilkan hasil pekerjaan ini dalam portofolio, termasuk nama
> dan tangkapan layar, kecuali Klien menyatakan keberatan secara tertulis. Data
> pribadi pengguna akhir tidak akan ditampilkan dalam kondisi apa pun."

Klausul ini menghemat percakapan canggung di setiap proyek berikutnya.
Detail: [10-operasional-legal.md](10-operasional-legal.md).

---

## 5.6 Label Jujur — Klien vs Demo vs Internal

Portofolio awal akan berisi campuran. Itu wajar dan tidak masalah — **selama dilabeli.**

| Label | Arti | Contoh |
|---|---|---|
| `Klien` | Dikerjakan untuk pihak lain, dipakai sungguhan | P3M PENS |
| `Produk sendiri` | Dibangun sendiri sebagai produk/demo, tidak ada klien | Demo klinik gigi |
| `Konsep` | Latihan atau eksplorasi, tidak pernah dipakai | (hindari — lemah) |

**Kenapa ini penting secara praktis, bukan cuma moral:** prospek serius akan bertanya
"ini kliennya siapa?". Jawaban "sebenarnya itu demo saya sendiri" setelah ditampilkan
seolah pekerjaan klien membuat semua item portofolio lain ikut diragukan.
Melabeli di depan justru menghilangkan pertanyaan itu — dan demo yang dibangun
sebaik proyek klien tetap mengesankan.

---

## 5.7 Daftar Portofolio & Prioritas

| # | Proyek | Label | Status | Prioritas | Naskah |
|---|---|---|---|---|---|
| 1 | **Sistem Informasi P3M PENS** | Klien | ✅ Selesai | **Tertinggi** — tulis pertama | [portofolio/01-p3m-pens.md](portofolio/01-p3m-pens.md) |
| 2 | **Demo Website Klinik Gigi** | Produk sendiri | ⬜ Belum dibangun | Tinggi — setelah demo jadi | [portofolio/02-demo-klinik-gigi.md](portofolio/02-demo-klinik-gigi.md) |
| 3 | Website studio ini sendiri | Produk sendiri | ⬜ | Sedang — jadikan studi kasus mini di /proses | – |
| 4 | Klien berbayar pertama | Klien | ⬜ | Tinggi begitu ada | – |

**Target minimum untuk meluncurkan situs: satu studi kasus lengkap (P3M PENS).**
Jangan menunda peluncuran demi mengumpulkan tiga.

---

## 5.8 Template Naskah Studi Kasus

Salin ini untuk setiap proyek baru.

```markdown
# [Nama Proyek]

**Label:** Klien | Produk sendiri
**Jenis:** [Sistem informasi / Website bisnis / Company profile]
**Durasi:** [ ]   **Peran:** [ ]   **Tahun:** [ ]
**Stack:** [ ]

## Tiga angka
| [angka] | [angka] | [angka] |
| [label] | [label] | [label] |

## Masalah
[2-3 paragraf. Keadaan sebelum, dalam bahasa klien. Apa yang sebenarnya sakit.]

## Batasan yang membuat proyek ini tidak biasa
- **[Batasan 1]** — [kenapa ini sulit, apa konsekuensinya]
- **[Batasan 2]** — ...

## Keputusan
### [Keputusan 1]
Saya memilih **[X]** karena **[Y]**, walaupun **[Z]**.
[2-4 kalimat. Alternatif yang ditolak dan alasannya.]

### [Keputusan 2] ...

## Hasil
[Angka, tangkapan layar, diagram. Sebelum/sesudah kalau ada.]

## Yang saya lakukan berbeda kalau mengulang
[1-2 hal jujur. Bukan kelemahan palsu.]

## Kalau Anda punya masalah serupa
[CTA]
```
