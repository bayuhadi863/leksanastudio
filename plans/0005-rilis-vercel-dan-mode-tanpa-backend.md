# 0005 — Rilis ke Vercel dan mode tanpa backend

- **Status:** Selesai — situs publik tayang di Vercel, repo satu untuk semua bagian
- **Tanggal:** 2026-08-25
- **Menyentuh:** frontend (sakelar panel), repo (git, ignore, CI), Vercel (proyek + CI/CD)
- **Menggantikan sebagian:** [0003](0003-cms-konten.md) §8 — situs masih membaca berkas,
  belum menarik dari API saat build. Itu tetap rencananya; rilis ini tidak menunggunya.

---

## 1. Kenapa sekarang

Situsnya sudah bisa dinilai, CMS-nya belum selesai. Menahan rilis sampai CMS rampung
berarti menahan satu-satunya barang bukti yang dimiliki studio ini — dan situs yang
belum tayang tidak meyakinkan siapa pun.

Yang menahan rilis sebenarnya cuma satu hal: frontend memuat panel pengelolaan, dan
panel butuh backend. Backend butuh PostgreSQL, Redis, MinIO, dan sebuah server. Itu
keputusan hosting yang belum perlu diambil hari ini.

## 2. Keputusan

| # | Pertanyaan | Keputusan |
| --- | --- | --- |
| D1 | Satu repo atau banyak? | **Satu** — `frontend/`, `backend/`, `infra/`, `blueprint/`, `plans/` |
| D2 | Bagaimana frontend bisa jalan tanpa backend? | Sakelar **waktu build**, bukan waktu jalan |
| D3 | Bawaan sakelarnya? | **Mati** — deployment yang lupa mengatur dapat situs, bukan panel |
| D4 | Bagaimana Vercel membangun dari repo banyak folder? | *Root Directory* = `frontend`, plus `ignoreCommand` |
| D5 | CI di mana? | GitHub Actions untuk pemeriksaan, Vercel Git untuk deploy |

### D2 — kenapa waktu build, bukan waktu jalan

Sakelar waktu jalan (`if (config.panelEnabled)`) tetap **mengirimkan seluruh kode panel**
ke setiap pengunjung, lalu memintanya dengan sopan untuk tidak merender. Yang dikirim
tetap ada: dapat dibaca, dapat dipanggil, dan tetap 480 kB.

`__PANEL_ENABLED__` diganti jadi literal saat transform, jadi
`false ? lazy(() => import('…')) : null` lipat sendiri, `import()`-nya hilang, dan
bundler tidak pernah membuat potongannya.

Hasil terukur:

```
tanpa panel   122 modul → 1 berkas JS   430 kB
dengan panel  306 modul → 5 berkas JS   430 kB + 483 kB panel + 87 kB + 40 kB
```

Situs publik tidak sekadar menyembunyikan panel — panelnya memang tidak ada di sana.

### D3 — bawaan mati

Bawaan yang aman adalah yang mengirim lebih sedikit. Deployment yang lupa mengisi apa pun
mendapat situsnya, bukan panel admin yang menunjuk API yang tidak ada di alamat mana pun.

Konsekuensinya: pengembang lokal harus menulis satu baris di `.env` (`VITE_PANEL="on"`).
Itu murah, dan eksplisit.

## 3. Bentuknya di kode

```
frontend/src/config/features.ts   panelEnabled — satu tempat menjawab "ada panel?"
frontend/src/App.tsx              rute panel/auth hanya dipasang kalau ada
frontend/vite.config.ts           __PANEL_ENABLED__ diisi dari VITE_PANEL
frontend/src/vite-env.d.ts        tipe untuk konstanta dan env
```

Formulir kontak **tidak** ikut mati: ia berjalan di fungsi serverless
(`frontend/api/kontak.ts`), bukan di backend .NET. Tanpa kunci Resend/Fonnte ia tetap
menerima kiriman dan mencatatnya; dengan kunci, ia mengirim email dan WhatsApp.

## 4. Rahasia yang tidak boleh ikut terdorong

Diperiksa sebelum komit pertama, bukan sesudahnya:

| Berkas | Perlakuan |
| --- | --- |
| `frontend/.env` | diabaikan · ada `.env.example` |
| `infra/.env` | diabaikan · ada `.env.example` |
| `backend/src/appsettings.Development.json` | diabaikan · ada `.json.example` |
| `backend/src/appsettings.json` | **dikomit** — isinya placeholder `{Jwt__Secret}`, bukan nilai |
| `.remember/` | diabaikan — catatan sesi, bukan bagian proyek |

## 5. Rantai rilis

```
git push  →  GitHub  →  ┬─  GitHub Actions  ·  typecheck, lint, build ×2, build backend
                        └─  Vercel Git      ·  Root Directory = frontend
                                                ignoreCommand: lewati kalau frontend/ tak berubah
                                                VITE_PANEL tidak diisi → situs publik saja
```

`ignoreCommand` harus tahan komit pertama. Versi awalnya `git diff --quiet HEAD^ HEAD -- .`
dan deployment pertama langsung gagal — `HEAD^` tidak ada pada komit tanpa induk, keluar
dengan kode 128, dan Vercel menganggap itu galat, bukan "bangun saja". Sekarang parent-nya
diperiksa lebih dulu; kalau tidak ada, perintahnya gagal dengan sengaja dan buildnya jalan.

Deploy tidak lewat Actions. Integrasi Git milik Vercel sudah melakukannya, dan menambah
`VERCEL_TOKEN` ke rahasia repo hanya menambah satu kunci untuk dijaga tanpa menambah
kemampuan apa pun.

## 6. Yang belum dikerjakan

- Situs masih membaca MDX dan berkas config, bukan API. Rencananya tetap [0003](0003-cms-konten.md) §8.
- Backend belum di-deploy ke mana pun. Panel berjalan lokal.
- Domain `leksana.id` belum dipasang ke proyek Vercel.
