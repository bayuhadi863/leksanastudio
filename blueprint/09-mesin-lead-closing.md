# 09 — Mesin Lead & Closing

## 9.1 Corong

```
        SUMBER                          TINDAKAN               TAHAP
 ┌──────────────────────┐
 │ Pencarian Google     │──┐
 │ LinkedIn / Instagram │──┤
 │ Demo klinik gigi     │──┼──►  Situs studio           1. SADAR
 │ Rujukan              │──┤     (halaman vertikal /
 │ Outreach langsung    │──┘      studi kasus)
 └──────────────────────┘
                                        │
                                        ▼
                              Baca studi kasus /       2. PERCAYA
                              lihat demo hidup
                                        │
                                        ▼
                              Klik WhatsApp atau       3. KONTAK
                              isi form 3 field
                                        │
                                        ▼
                              Balasan < 2 jam +        4. KUALIFIKASI
                              4 pertanyaan penyaring
                                        │
                          ┌─────────────┴─────────────┐
                          ▼                           ▼
                   Lolos → discovery call      Gagal → kirim tautan
                   30 menit                    harga, akhiri sopan
                          │
                          ▼
                   Penawaran tertulis           5. PENAWARAN
                   (dalam 48 jam)
                          │
                          ▼
                   Follow-up terjadwal          6. CLOSING
                   H+3, H+7, H+14
                          │
                          ▼
                   DP masuk → mulai kerja
```

**Titik bocor terbesar ada di tahap 4.** Balasan lambat membunuh lebih banyak deal
daripada harga tinggi. Detail di §9.3.

---

## 9.2 CTA — Aturan Keras

| Aturan | Alasan |
|---|---|
| **Satu tindakan utama di seluruh situs:** WhatsApp | Prospek Indonesia hampir selalu memilih WhatsApp di atas form. Form ada sebagai cadangan, bukan sebagai pilihan utama |
| Setiap tautan WhatsApp membawa **pesan awal yang berbeda sesuai halaman** | Pesan awal memberitahumu dari halaman mana prospek datang, tanpa perlu bertanya |
| Bar WhatsApp tetap di bawah layar HP | Mayoritas prospek membaca dari HP dan tidak menggulir kembali ke atas |
| CTA muncul minimal 3× di halaman panjang | Prospek memutuskan di titik yang berbeda-beda |
| Tidak ada popup keluar-niat, tidak ada widget chat pihak ketiga | Merusak performa dan mengganggu. Kamu tidak punya orang untuk menjaga chat langsung |

### Peta pesan awal WhatsApp

| Halaman | Pesan awal |
|---|---|
| Homepage | `Halo, saya ingin diskusi soal pembuatan website.` |
| `/jasa-website-klinik-gigi` | `Halo, saya dari klinik gigi. Ingin tanya soal pembuatan website klinik.` |
| `/layanan/sistem-informasi` | `Halo, saya ingin tanya soal pembuatan sistem informasi.` |
| `/harga` | `Halo, saya sudah lihat halaman harga. Ingin tanya untuk kebutuhan [ ].` |
| Studi kasus P3M | `Halo, saya baru baca studi kasus P3M PENS. Kebutuhan saya mirip.` |

Prospek yang datang dengan pesan keempat atau kelima adalah prospek **paling matang**
yang akan kamu terima. Perlakukan berbeda: langsung ke discovery call, jangan
menyaring ulang dari nol.

---

## 9.3 Aturan Respons

| Situasi | Target | Kenapa |
|---|---|---|
| Pesan WhatsApp masuk | **< 2 jam** di jam kerja | Prospek jasa lokal biasanya menghubungi 2–3 penyedia sekaligus. Yang membalas pertama menang jauh lebih sering daripada yang termurah |
| Form masuk | < 4 jam | |
| Di luar jam kerja | Balasan otomatis + balasan sungguhan pagi berikutnya | Balasan otomatis harus menyebut jam berapa kamu membalas |
| Penawaran tertulis | < 48 jam setelah discovery call | Lebih dari itu, momentumnya hilang |

**Notifikasi WhatsApp untuk setiap lead form wajib menyala** — teknisnya di
[07-arsitektur-teknis.md](07-arsitektur-teknis.md) §7.8. Lead yang mendarat di email
dan baru dibaca besok pagi adalah lead yang hilang.

---

## 9.4 Kualifikasi (Empat Pertanyaan)

Kirim setelah sapaan, sebelum menjelaskan apa pun. Menjelaskan panjang lebar ke
prospek yang tidak terkualifikasi adalah cara paling umum membuang waktu.

> Halo Pak/Bu [nama], terima kasih sudah menghubungi.
> Supaya saya bisa langsung memberi jawaban yang relevan, boleh saya tanya 4 hal singkat?
>
> 1. Sekarang pelanggan/pengguna paling banyak datang dari mana?
> 2. Sudah pernah pakai jasa serupa atau pasang iklan sebelumnya?
> 3. Kalau website/sistemnya jadi, apa yang harus berubah dalam 3 bulan?
> 4. Rentang anggaran yang sudah disiapkan kira-kira berapa?

### Penilaian

| Jawaban | Skor |
|---|---|
| Menyebut angka atau sumber spesifik (Q1) | +1 |
| Pernah membayar untuk pertumbuhan (Q2) | +1 |
| Punya target terukur (Q3) | +1 |
| Menyebut angka anggaran ≥ lantai harga (Q4) | +2 |
| Menolak menyebut anggaran sama sekali | −2 |
| "Yang penting murah" | **diskualifikasi** |

**Skor ≥ 3 → discovery call.** Skor < 3 → kirim tautan halaman harga, akhiri sopan:

> Terima kasih infonya. Untuk kebutuhan seperti ini, paket yang paling mendekati ada
> di [tautan]. Kalau ternyata cocok, kabari saja — saya siap bantu.

Ini bukan menolak. Ini memindahkan beban keputusan ke prospek dan membebaskan waktumu.

---

## 9.5 Discovery Call (30 Menit)

**Aturan pertama: kamu mendengarkan 70%, bicara 30%.** Penyedia jasa yang gugup
menjelaskan panjang lebar. Penyedia jasa yang dipercaya bertanya.

```
Menit 0-5    Konteks bisnis
             "Ceritakan sedikit soal [bisnis/unit] Bapak/Ibu."
             "Siapa yang paling sering jadi pelanggan/pengguna?"

Menit 5-15   Masalah sebenarnya
             "Apa yang paling bikin repot sekarang?"
             "Kalau ini tidak diselesaikan tahun ini, apa akibatnya?"
             "Sudah pernah coba apa saja sebelumnya?"        ← paling penting

Menit 15-22  Keberhasilan & batasan
             "Kalau 6 bulan lagi ini berhasil, apa yang berbeda?"
             "Siapa lagi yang ikut memutuskan?"              ← jangan dilewat
             "Kapan targetnya harus sudah jalan?"

Menit 22-28  Kamu bicara — pendek
             Ringkas ulang masalahnya dengan kata-katamu sendiri
             Sebut satu pekerjaan serupa + apa hasilnya
             Sebut rentang harga dan durasi. Jangan menunda ini

Menit 28-30  Langkah berikutnya, dengan tanggal
             "Saya kirim penawaran tertulis hari [X]. Boleh saya
              kabari lagi hari [Y] kalau belum ada kabar?"
```

### Tiga pertanyaan yang paling sering dilewatkan dan paling mahal

1. **"Siapa lagi yang ikut memutuskan?"** — deal yang mati di menit terakhir hampir
   selalu mati karena ada orang yang tidak pernah ikut bicara.
2. **"Sudah pernah coba apa saja sebelumnya?"** — jawabannya memberitahumu keberatan
   sebenarnya, dan memberitahumu apa yang tidak boleh kamu janjikan.
3. **"Kapan targetnya harus sudah jalan?"** — tanpa tenggat, deal mengambang selamanya.

### Yang tidak dilakukan

- Jangan menjelaskan stack. Tidak ada yang peduli
- Jangan menjanjikan hasil yang tidak kamu kendalikan ("pasti ranking 1")
- Jangan memberi harga pasti untuk Lini C di telepon. Beri rentang, sebut Tahap 0
- Jangan menutup panggilan tanpa langkah berikutnya bertanggal

---

## 9.6 Penawaran Tertulis

Satu berkas PDF, 2–4 halaman. Lebih dari itu tidak dibaca.

```
1. Ringkasan masalah — dengan kata-kata mereka, dari discovery call
   ★ Ini halaman paling penting. Prospek yang merasa dipahami
     sudah setengah memutuskan.
2. Yang akan dikerjakan — daftar konkret
3. Yang TIDAK termasuk — daftar eksplisit
4. Waktu pengerjaan + apa yang dibutuhkan dari mereka
5. Harga + termin pembayaran
6. Bukti relevan — satu studi kasus, satu paragraf + tautan
7. Langkah berikutnya + masa berlaku penawaran (14 hari)
```

**Masa berlaku 14 hari bukan trik.** Ia jujur (hargamu memang berubah, jadwalmu
memang terisi) dan ia memberi alasan untuk memutuskan.

**Bagian 3 mencegah sebagian besar sengketa ruang lingkup.** Tulis spesifik:
"tidak termasuk penulisan konten, foto produk, pengelolaan iklan, dan integrasi
dengan sistem pihak ketiga yang belum disebutkan di atas."

---

## 9.7 Follow-up

Sebagian besar deal ditutup di follow-up ketiga. Sebagian besar penyedia jasa berhenti
di follow-up pertama. Itu seluruh peluangnya.

| Kapan | Isi |
|---|---|
| **H+3** | "Pak/Bu, sudah sempat baca penawarannya? Ada bagian yang perlu saya jelaskan?" |
| **H+7** | Kirim **hal berguna**, bukan tagihan. Tangkapan layar pesaing mereka, satu ide konkret, satu artikel relevan |
| **H+14** | "Saya tutup dulu penawarannya ya, Pak/Bu. Kalau nanti dibutuhkan lagi, kabari saja — harganya mungkin sudah berbeda, tapi saya usahakan." |
| **H+60** | Sapaan ringan tanpa jualan. Sekitar 1 dari 10 deal lahir di sini |

**Aturan nada:** setiap follow-up harus memberi sesuatu, bukan meminta sesuatu.
Follow-up yang hanya berisi "gimana Pak?" tiga kali berturut-turut membuatmu
terlihat butuh — dan orang tidak membeli dari orang yang terlihat butuh.

---

## 9.8 Menangani Keberatan

| Keberatan | Jangan | Lakukan |
|---|---|---|
| "Mahal" | Menurunkan harga | Tanya: "dibanding apa?" — lalu bandingkan biaya, bukan harga. Kalau tetap tidak cocok, kurangi **ruang lingkup**, bukan harga |
| "Ada yang menawarkan Rp 1,5 juta" | Menjelekkan pesaing | "Bisa jadi itu memang pas untuk kebutuhan Bapak. Yang membedakan biasanya [satu hal konkret]. Kalau yang itu tidak penting, saya sarankan ambil yang lebih murah." |
| "Saya pikir-pikir dulu" | Diam menunggu | "Boleh saya tahu bagian mana yang masih mengganjal? Kalau soal [X], saya bisa jelaskan sekarang." |
| "Nanti saya diskusi dulu dengan tim" | Menunggu tanpa tenggat | "Boleh saya bantu siapkan ringkasan satu halaman untuk didiskusikan? Kira-kira kapan kabarnya bisa saya tunggu?" |
| "Bisa lihat contoh lain?" | Panik karena portofolio tipis | Kirim studi kasus yang **paling mirip masalahnya**, bukan yang paling banyak. Satu yang relevan mengalahkan lima yang tidak |
| "Kok cuma sendiri?" | Membesar-besarkan | "Betul, saya kerjakan sendiri. Artinya tidak ada yang hilang di antara oper-operan, dan Anda selalu bicara ke orang yang benar-benar mengerjakan." |
| "Bisa dicicil?" | Menyetujui begitu saja | Termin sudah bertahap. Kalau tetap tidak cukup, kecilkan ruang lingkup jadi paket di bawahnya |

---

## 9.9 Kalau Ditolak

1. **Tanya alasannya, satu pertanyaan saja.** "Boleh saya tahu apa yang membuat
   pilihannya jatuh ke sana? Supaya saya bisa memperbaiki penawaran berikutnya."
   Sekitar setengahnya akan menjawab jujur, dan jawabannya berharga.
2. **Catat.** Alasan penolakan yang berulang adalah masalah nyata di penawaranmu —
   bukan nasib buruk.
3. **Tetap sopan dan tetap hadir.** Vendor murah sering gagal. Prospek yang ditolak
   hari ini adalah prospek yang kembali enam bulan lagi — tapi hanya kalau kamu
   tidak berubah dingin saat ditolak.

---

## 9.10 Papan Pelacakan Minimal

Spreadsheet cukup. Jangan pasang CRM sebelum ada 20 lead aktif.

| Kolom | Isi |
|---|---|
| Tanggal masuk | |
| Nama & bisnis | |
| Sumber | Halaman vertikal / studi kasus / LinkedIn / rujukan / outreach |
| Skor kualifikasi | 0–5 |
| Tahap | Kontak · Kualifikasi · Call · Penawaran · Menang · Kalah |
| Nilai | Rp |
| Follow-up berikutnya | **Tanggal — kolom paling penting** |
| Alasan kalah | |

**Kolom "follow-up berikutnya" adalah alasan papan ini ada.** Tanpa tanggal yang
terlihat, follow-up tidak terjadi — dan follow-up yang tidak terjadi adalah bocor
terbesar di seluruh corong.
