# Tangkapan layar portofolio

Taruh berkas gambar di folder ini, lalu daftarkan di frontmatter studi kasus
(`cover`) atau di komponen `<Figure src="…">` di dalam MDX.

Selama `cover` belum diisi, situs memakai skema SVG sebagai penggantinya —
jadi tidak ada kotak kosong dan tidak ada yang rusak.

## Penamaan

```
<slug-studi-kasus>-cover.png      wajib — dipakai di kartu portofolio
<slug-studi-kasus>-01.png         opsional — figur di dalam badan studi kasus
<slug-studi-kasus>-02.png
```

## Ukuran

| Peran                  | Ukuran      | Rasio |
| ---------------------- | ----------- | ----- |
| `cover`                | 1600 × 1000 | 16:10 |
| Figur di badan tulisan | 2000 × 1250 | 16:10 |

PNG. Next.js yang mengubahnya ke AVIF/WebP dan menurunkan ukurannya saat build —
jangan dikompres dulu, teks antarmuka akan pecah.

## Aturan isi

1. **Potong chrome peramban.** Tanpa bingkai jendela, tanpa bingkai laptop,
   tanpa mockup 3D. Yang ditampilkan adalah antarmukanya, bukan perangkatnya.
2. **Ganti data pribadi dengan nama fiktif — jangan diblur.** Blur terbaca
   seperti menyembunyikan sesuatu; nama fiktif terbaca seperti data contoh.
   Berlaku untuk nama orang, NIP, surel, nomor telepon, dan isi dokumen.
3. **Rasio 16:10, isi penting di bagian atas.** Kartu memotong dari bawah
   (`object-top`), jadi baris pertama tabel harus terlihat.
4. **Ambil di mode terang.** Situs menampilkannya di kedua tema; tangkapan
   layar terang lebih netral di keduanya.
5. **Satu layar penuh, bukan potongan sempit.** Yang menjual adalah kepadatan
   antarmukanya.
