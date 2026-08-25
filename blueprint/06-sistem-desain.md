# 06 — Arah Desain Situs Studio

> **Dokumen ini:** prinsip dan alasan — *kenapa* situs studio terlihat seperti itu.
> **Spesifikasi implementasi** (token, skala tipografi, grid, komponen, state,
> aksesibilitas, kode): **[06b-design-system.md](06b-design-system.md)**.
>
> Baca dokumen ini dulu. Kalau sudah paham alasannya, kerjakan dari 06b.

---

## 6.1 Prinsip

Situs studio punya satu pekerjaan yang tidak dimiliki situs klien: **ia adalah barang
buktinya sendiri.** Klien menilai kemampuanmu dari situs ini sebelum membaca satu
kalimat pun.

| # | Prinsip | Konsekuensi konkret |
|---|---|---|
| 1 | **Tidak boleh terlihat seperti situs klien** | Kalau demo klinik gigi dan situs studio terasa dari cetakan yang sama, kemampuanmu terlihat sempit. Palet, tipografi, dan ritme harus berbeda |
| 2 | **Presisi mengalahkan kemeriahan** | Perataan rapi, spasi konsisten, dan tipografi yang benar lebih meyakinkan daripada animasi |
| 3 | **Hindari seragam bawaan** | Gradien ungu-biru, blob berputar, ilustrasi isometrik, krem hangat + serif kontras tinggi + terakota — semuanya sinyal "template". §6.3 |
| 4 | **Teks adalah antarmuka utama** | Situs jasa dibaca, bukan dijelajahi. Anggaran desain terbesar masuk ke tipografi dan hierarki, bukan ke komponen |
| 5 | **Setiap elemen harus punya alasan** | Kalau ditanya "kenapa ada garis di sini?" dan jawabannya "biar rame", hapus |
| 6 | **Performa adalah bagian dari desain** | Keputusan visual yang menurunkan Lighthouse di bawah 100 tidak lolos, seindah apa pun |
| 7 | **Perangkat struktural mengkodekan kebenaran** | Nomor urut hanya untuk hal yang memang berurutan. Selebihnya hiasan |

---

## 6.2 Tesis

> **Situs ini terlihat seperti dokumen kerja yang ditulis seseorang, ditandatangani,
> dan dipertanggungjawabkan.**

Pembeda Leksana bukan tampilan, bukan stack, bukan harga — melainkan **alasan setiap
keputusan ditulis dan diserahkan.** Klien terakhir menerima 13 dokumen teknis dan
satu buku PDF bersama sistemnya. Tidak ada pesaing tingkat freelancer yang melakukan itu.

Kalau itu pembedanya, itu juga yang harus terlihat lebih dulu. Maka bahasa visualnya
diambil dari benda yang benar-benar diserahkan ke klien: **berkas kerja** — bukan
brosur agensi, bukan dasbor produk, bukan portofolio desainer.

**Elemen tanda tangan yang lahir dari tesis ini: "Catatan Pinggir"** — anotasi orang
pertama di margin luar, berisi alasan dan batas, sejajar dengan klaim yang
dianotasinya. Brand yang dijadikan struktur, bukan diklaim. Spesifikasi lengkap:
[06b-design-system.md](06b-design-system.md) §6b.2.

---

## 6.3 Yang Ditolak, dan Alasannya

Bagian ini sama pentingnya dengan yang dipilih. Tanpa daftar ini, desain akan
hanyut ke tampilan bawaan begitu ada tekanan waktu.

### Arah yang dipertimbangkan serius lalu ditolak

| Arah | Kenapa menarik | Kenapa ditolak |
|---|---|---|
| **Cetak biru / grid teknik** | Sesuai "dibangun untuk bertahan" | Kertas milimeter sudah jadi klise portofolio developer. Meniru *gambar* insinyur, bukan *kerja* insinyur |
| **Dasbor / status sistem** | Sesuai "sistem yang jalan" | Angka berjalan dan lampu hijau membaca sebagai produk SaaS. Kamu menjual jasa, bukan langganan |
| **Editorial surat kabar** | Rapi, dewasa | Garis rambut + serif kontras tinggi + nol radius muncul di ribuan situs. Ia tidak mengatakan apa pun tentang Leksana |

### Seragam bawaan yang dilarang

- Krem hangat + serif kontras tinggi + aksen terakota
- Latar mendekati hitam + satu aksen hijau asam atau vermilion
- Gradien ungu-biru, blob mengambang, ilustrasi isometrik orang bekerja
- **Inter + JetBrains Mono** — pasangan huruf bawaan. Bukan salah, tapi tidak mengatakan apa-apa
- Eyebrow bernomor `01 / TENTANG` di setiap bagian
- Mockup 3D, bingkai laptop mengambang, foto stok orang tersenyum di depan laptop
- Kartu yang terangkat dan membesar saat disentuh kursor

**Kenapa daftar ini keras.** Semua item di atas adalah pilihan *bawaan*, bukan
*keputusan*. Situs yang seluruhnya tersusun dari pilihan bawaan terbaca sebagai
dibuat cepat — dan itu kesan yang persis berlawanan dengan yang kamu jual.

---

## 6.4 Hubungan dengan Situs Klien

Prinsip 1 punya konsekuensi praktis yang mudah dilanggar saat Fase C.

| Boleh dipinjam demo klinik gigi dari situs studio | Tidak boleh dipinjam |
|---|---|
| Skala ruang (kelipatan 4) | Palet |
| Struktur token semantik | Pasangan huruf |
| Utilitas tata letak, pola form, aturan fokus | Grid asimetris + jalur catatan |
| Anggaran performa dan checklist aksesibilitas | Radius 2px & ketiadaan bayangan |

Demo klinik harus terasa seperti **klinik** — hangat, menenangkan, ramah pasien.
Situs studio terasa seperti **berkas kerja**. Keduanya dibuka berdampingan harus
terasa jelas berbeda; itu bagian dari checklist selesai di kedua proyek.

Sistem desain demo klinik: [`../02-produk-klinik-gigi/06-desain-sistem.md`](../02-produk-klinik-gigi/06-desain-sistem.md).

---

## 6.5 Anggaran Keberanian

Aturan tunggal yang menjaga desain minimal tidak berubah jadi desain kosong, dan
tidak berubah jadi desain ramai:

> **Seluruh keberanian dibelanjakan pada satu elemen: Catatan Pinggir.
> Semua yang lain tenang dan disiplin.**

Konsekuensinya, tertulis:

- **Gerak** hanya untuk catatan pinggir. Sisanya diam
- **Warna aksen** di bawah 5% permukaan halaman
- **Bayangan** hanya untuk elemen yang benar-benar melayang (bar WhatsApp HP, menu)
- **Huruf** maksimal tiga peran, lima berkas
- Sebelum menyelesaikan sebuah layar: **lepas satu elemen.** Hampir selalu lebih baik

---

## 6.6 Cara Memakai Dokumen Ini

| Kalau kamu sedang… | Buka |
|---|---|
| Memutuskan arah, menilai apakah sesuatu "terasa Leksana" | Dokumen ini |
| Menulis kode, butuh nilai token / ukuran / state | [06b-design-system.md](06b-design-system.md) |
| Menyusun halaman dan copy-nya | [04-arsitektur-informasi-copy.md](04-arsitektur-informasi-copy.md) |
| Mengejar skor performa | [07-arsitektur-teknis.md](07-arsitektur-teknis.md) §7.6 |
| Menampilkan angka & bukti di portofolio | [05-portofolio-studi-kasus.md](05-portofolio-studi-kasus.md) |
