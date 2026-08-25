# Plans

Dokumen rencana untuk pekerjaan yang cukup rumit sehingga keputusannya perlu
disepakati **sebelum** kodenya ditulis.

Bukan setiap tugas butuh berkas di sini. Yang butuh: perubahan yang menyentuh
banyak berkas sekaligus, yang mengubah kontrak antar lapisan (URL, skema basis
data, bentuk API), atau yang punya lebih dari satu jalan keluar yang sama-sama
masuk akal. Kalau sebuah tugas hanya punya satu cara benar dan lingkupnya jelas,
langsung kerjakan — menulis rencananya hanya menunda.

## Penamaan

```
plans/NNNN-judul-kebab.md
```

Nomor urut, tidak pernah dipakai ulang, tidak pernah diurutkan ulang. Rencana yang
ditolak tetap tinggal dengan status `Ditolak` — alasan sebuah jalan **tidak**
diambil sama berharganya dengan alasan jalan lain diambil, dan itu yang paling
sering hilang dari sebuah repositori.

## Bentuk

Tiap berkas dibuka dengan blok status:

```markdown
- **Status:** Draf · Menunggu keputusan · Disetujui · Dikerjakan · Selesai · Ditolak
- **Tanggal:** YYYY-MM-DD
- **Menyentuh:** frontend / backend / infra
```

Isinya, urutannya kira-kira begini:

1. **Masalahnya apa** — bukan solusinya, masalahnya.
2. **Keadaan sekarang** — hasil survei, bukan ingatan. Sebutkan berkas dan barisnya.
3. **Pilihan yang ada** — minimal dua, dengan konsekuensi tiap pilihan.
4. **Rekomendasi** — satu, dengan alasan yang bisa dibantah.
5. **Rencana eksekusi** — langkah demi langkah, berkas per berkas.
6. **Rencana verifikasi** — bagaimana kita tahu ini berhasil.
7. **Yang tidak dikerjakan** — batas lingkup, ditulis eksplisit.

Setelah dikerjakan, statusnya diperbarui dan bagian yang meleset dari rencana
dicatat di bawah. Rencana yang tidak pernah dibandingkan dengan hasilnya adalah
dokumen pemasaran, bukan dokumen teknis.

## Daftar

| No | Judul | Status |
| --- | --- | --- |
| [0001](0001-konvensi-routing-frontend.md) | Konvensi routing frontend | Sebagian digantikan 0002 |
| [0002](0002-arsitektur-multibahasa.md) | Arsitektur multibahasa (ID + EN) | Fase 1–3 disetujui · EN ditunda |
| [0003](0003-cms-konten.md) | CMS konten | Dikerjakan |
