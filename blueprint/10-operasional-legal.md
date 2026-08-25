# 10 — Operasional & Legal

> Dokumen ini bukan nasihat hukum. Ia adalah daftar praktik yang mencegah masalah
> paling umum di jasa web di Indonesia. Untuk kontrak bernilai besar (Lini C atau
> kerja sama institusi), tinjau bersama orang yang memang berlatar hukum.

---

## 10.1 Yang Harus Ada Sebelum Klien Pertama

| Item | Wajib kapan | Catatan |
|---|---|---|
| Rekening bank terpisah | **Sebelum DP pertama** | Mencampur uang usaha dan pribadi membuat pembukuan mustahil dan pajak berantakan |
| Template kontrak / SPK | **Sebelum DP pertama** | §10.3 |
| Template invoice bernomor | **Sebelum DP pertama** | §10.6 |
| Template penawaran | Sebelum penawaran pertama | [09-mesin-lead-closing.md](09-mesin-lead-closing.md) §9.6 |
| Email domain sendiri | Sebelum situs live | Gmail pribadi terlihat di setiap kontak |
| NPWP pribadi | Sudah wajib bagi setiap WP | Dibutuhkan begitu klien institusi minta faktur |
| Badan usaha (CV/PT) | **Belum** — tunggu pemicunya | §10.2 |

**Jangan mendirikan badan usaha sebelum ada pendapatan.** Biaya pendirian dan
kepatuhan bulanan nyata; manfaatnya belum.

---

## 10.2 Kapan Naik ke Badan Usaha

Tiga pemicu. **Satu saja terpenuhi, urus.**

1. **Klien institusi (kampus, pemda, BUMN, perusahaan menengah) minta kontrak atas
   nama badan usaha.** Ini pemicu paling sering, dan kemungkinan besar akan datang
   dari jalur Lini C-mu.
2. **Nilai proyek melewati ~Rp 50 juta**, atau omzet tahunan mendekati Rp 500 juta.
3. **Kamu mulai membayar orang lain** secara rutin.

| Bentuk | Cocok untuk | Catatan |
|---|---|---|
| **Perorangan** | Sekarang | Pajak lewat SPT pribadi. Cukup untuk klien UKM |
| **CV** | Pemicu 1 atau 2 | Lebih murah & cepat dari PT. Diterima banyak institusi |
| **PT Perorangan** | Kalau klien mensyaratkan PT | Bisa satu orang, biaya menengah, tanggung jawab terbatas |

**Yang biasanya diminta institusi:** akta, NIB, NPWP badan, dan rekening atas nama
badan. Kalau targetmu memang institusi (§Lini C di
[01-strategi-usaha.md](01-strategi-usaha.md)), siapkan sebelum tender/penunjukan —
mengurusnya di tengah proses membuatmu kehilangan jadwal.

---

## 10.3 Kontrak — Sembilan Klausul yang Menyelamatkan

Untuk Lini A, dokumen 2–3 halaman sudah cukup dan justru lebih mungkin ditandatangani
daripada kontrak 12 halaman.

### 1. Ruang lingkup, sespesifik mungkin
Jumlah halaman, fitur yang disebut namanya, jumlah putaran revisi.
**Wajib disertai daftar "yang tidak termasuk".** Ini klausul pencegah sengketa nomor satu.

### 2. Kewajiban klien & konsekuensi keterlambatan
> "Klien menyerahkan konten (teks, logo, foto) paling lambat [tanggal]. Keterlambatan
> penyerahan konten menggeser tenggat penyelesaian dengan jumlah hari yang sama."

**Penyebab molor nomor satu di jasa web adalah konten dari klien yang tidak datang.**
Tanpa klausul ini, keterlambatan mereka menjadi kesalahanmu.

### 3. Termin pembayaran & konsekuensi tunggakan
> "Pekerjaan dihentikan sementara apabila pembayaran termin melewati 14 hari dari
> tanggal jatuh tempo."

### 4. Batas revisi & harga di luar batas
> "Termasuk [N] putaran revisi pada tahap desain. Revisi tambahan atau perubahan
> ruang lingkup dikenakan Rp [X] per jam, selalu disampaikan dan disetujui tertulis
> sebelum dikerjakan."

### 5. Kepemilikan
> "Seluruh hak atas kode, desain, dan konten hasil pekerjaan beralih kepada Klien
> setelah pelunasan. Penyedia mempertahankan hak untuk menggunakan kembali komponen,
> pustaka, dan kerangka kerja generik yang tidak khas Klien."

**Kalimat kedua adalah nyawa model produktisasimu.** Tanpa itu, klien pertama secara
teknis memiliki inti produk yang kamu pakai untuk klien kedua.

### 6. Domain, hosting, dan akun
> "Domain, hosting, dan seluruh akun pihak ketiga didaftarkan atas nama dan dibayar
> langsung oleh Klien."

**Jangan pernah menalangi domain atau hosting klien.** Ini sumber sengketa terbesar
di jasa web: klien menghilang, layanan mati, namamu yang tercatat sebagai pemilik.

### 7. Garansi & batasnya
> "Garansi perbaikan bug [60] hari sejak peluncuran. Tidak termasuk perubahan konten,
> penambahan fitur, atau kerusakan akibat perubahan yang dilakukan pihak lain."

### 8. Portofolio
> "Penyedia berhak menampilkan hasil pekerjaan dalam portofolio, termasuk nama dan
> tangkapan layar, kecuali Klien menyatakan keberatan tertulis. Data pribadi pengguna
> akhir tidak akan ditampilkan dalam kondisi apa pun."

Menghemat percakapan canggung di setiap proyek. Lihat
[05-portofolio-studi-kasus.md](05-portofolio-studi-kasus.md) §5.5.

### 9. Pembatalan
> "Apabila Klien membatalkan setelah pekerjaan dimulai, DP tidak dikembalikan.
> Apabila Penyedia tidak dapat melanjutkan, DP dikembalikan proporsional terhadap
> pekerjaan yang belum diselesaikan."

**Dua arah.** Klausul yang hanya melindungi satu pihak membuat klien waspada dan
sering menunda tanda tangan.

---

## 10.4 Tambahan Khusus Lini C

Sistem informasi memerlukan klausul yang tidak relevan untuk website:

| Klausul | Isi |
|---|---|
| **Uji terima (UAT)** | Kriteria terima tertulis, masa uji [N] hari, dan ketentuan bahwa tidak adanya tanggapan dalam masa itu berarti diterima |
| **Data & kerahasiaan** | Data klien tidak disalin keluar lingkungan yang disepakati; NDA dua arah bila diminta |
| **Lingkungan deployment** | Siapa menyediakan server, siapa memegang akses root, siapa bertanggung jawab atas backup |
| **Perubahan ruang lingkup (CR)** | Proses tertulis: usulan → estimasi → persetujuan → kerjakan. Tanpa ini, proyek Lini C tidak pernah selesai |
| **Serah terima** | Daftar barang yang diserahkan: kode, dokumentasi, kredensial, panduan deployment, sesi pelatihan |
| **Masa dukungan** | Berapa lama, jam berapa, kanal apa, berapa lama waktu respons |

**Klausul uji terima dan CR adalah dua hal yang membedakan proyek Lini C yang selesai
dari yang berjalan tanpa akhir.** Jangan mulai tanpa keduanya.

---

## 10.5 Pemeriksaan Sebelum Proyek Dimulai

- [ ] Ruang lingkup tertulis & disetujui (balasan email/WhatsApp cukup sebagai bukti)
- [ ] DP masuk ke rekening — **bukan "sudah ditransfer", tapi sudah masuk**
- [ ] Konten & aset dari klien sudah diterima, atau tanggalnya disepakati tertulis
- [ ] Akses yang dibutuhkan sudah diberikan (domain, analytics, akun media sosial)
- [ ] Satu orang penanggung jawab dari sisi klien sudah ditunjuk namanya
- [ ] Kanal komunikasi disepakati (satu grup WhatsApp, bukan tiga jalur pribadi)

**Poin terakhir sering diremehkan.** Tanpa satu penanggung jawab, kamu akan menerima
revisi yang saling bertentangan dari tiga orang dan tidak ada yang bisa memutuskan.

---

## 10.6 Invoice & Pencatatan

**Format nomor invoice:** `INV/2026/08/001` — berurutan, tidak pernah diulang.

**Isi wajib:** nomor, tanggal, jatuh tempo, identitas kedua pihak, rincian pekerjaan,
termin ke berapa dari berapa, jumlah, rekening tujuan, dan catatan pajak bila berlaku.

**Pembukuan minimal — satu spreadsheet:**

| Tanggal | Jenis | Klien | Keterangan | Masuk | Keluar |
|---|---|---|---|---|---|

Catat **setiap** transaksi usaha, termasuk langganan Rp 20 ribu. Menyusun ulang
setahun ke belakang jauh lebih menyakitkan daripada mencatat lima detik hari ini.

**Pajak:** sebagai perorangan, penghasilan usaha dilaporkan di SPT tahunan. Sisihkan
persentase tetap dari setiap pembayaran masuk ke rekening terpisah sejak transaksi
pertama — ini kebiasaan yang mencegah kejutan di bulan Maret.

---

## 10.7 Batas Etis yang Tidak Dilanggar

Berlaku di semua lini, termasuk saat prospek memintanya.

| Larangan | Alasan |
|---|---|
| **Tidak ada klaim medis** di situs klinik mana pun — "dijamin tidak sakit", "sembuh total" | Melanggar aturan iklan kesehatan dan membahayakan klien. Detail: [`../02-produk-klinik-gigi/12-etika-legal.md`](../02-produk-klinik-gigi/12-etika-legal.md) |
| **Tidak mengarang testimoni, angka, atau logo klien** | Satu klaim palsu menghapus seluruh kredibilitas yang dibangun bertahun-tahun |
| **Tidak menampilkan data pribadi pengguna nyata** di portofolio | Termasuk yang tidak sengaja tertangkap di tangkapan layar |
| **Tidak menjanjikan peringkat Google** | Tidak dalam kendalimu. Janjikan proses, bukan posisi |
| **Tidak memakai aset berlisensi tanpa hak** — font, foto, ikon, template | Tagihan lisensi datang ke klien, dan itu menghancurkan hubungan |
| **Tidak menyandera klien** lewat akun atas namamu | Ini justru salah satu pilar penjualanmu. Jangan melanggarnya sendiri |
| **Tidak menyalin kode klien A untuk klien B** di luar komponen generik | Lihat klausul kepemilikan §10.3.5 |

---

## 10.8 Perlindungan Diri Sendiri

Hal-hal yang sering diabaikan pekerja solo dan menjadi mahal:

| Praktik | Kenapa |
|---|---|
| **Backup kode di dua tempat** (remote git + salinan lokal/offsite) | Kehilangan repositori di tengah proyek Lini C adalah bencana yang tidak bisa dijelaskan ke klien |
| **Jangan simpan kredensial klien di catatan biasa** | Pakai pengelola kata sandi. Kebocoran kredensial klien adalah tanggung jawabmu |
| **Batasi satu proyek besar aktif** | Dua proyek Lini C bersamaan akan mematikan keduanya |
| **Tulis jam kerja & kanal di kontrak** | Tanpa batas tertulis, WhatsApp jam 11 malam menjadi normal dalam sebulan |
| **Simpan seluruh persetujuan tertulis** | "Kan dulu katanya termasuk" hanya bisa dijawab dengan tangkapan layar |
| **Jangan bekerja tanpa DP karena "kenalan"** | Proyek dari kenalan yang bermasalah merusak dua hal sekaligus: uang dan hubungan |

---

## 10.9 Serah Terima (Definisi "Selesai")

Proyek tidak selesai saat situs live. Proyek selesai saat berikut ini diserahkan:

- [ ] Situs/sistem live di domain klien
- [ ] Seluruh akun atas nama klien, kredensial diserahkan lewat kanal aman
- [ ] Repositori kode diserahkan atau diberikan akses
- [ ] Dokumentasi: cara menyunting konten, cara deploy, apa yang tidak boleh disentuh
- [ ] Sesi pelatihan dilakukan **dan direkam** — rekaman menghemat 80% pertanyaan susulan
- [ ] Analytics & Search Console terpasang atas nama klien
- [ ] Backup awal dibuat
- [ ] Masa garansi dan cara menghubungi disampaikan tertulis
- [ ] Invoice terakhir diterbitkan dan dilunasi
- [ ] **Izin portofolio dikonfirmasi** ([05-portofolio-studi-kasus.md](05-portofolio-studi-kasus.md) §5.5)

**Poin terakhir dilakukan saat klien paling puas** — hari serah terima, bukan enam
bulan kemudian saat mereka sudah lupa.
