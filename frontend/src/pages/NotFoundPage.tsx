import { Link } from 'react-router-dom'

import { ButtonLink } from '@/components/ui/Button'
import { Label } from '@/components/ui/Label'
import { mainNav, routes } from '@/config/routes'
import { usePageMeta } from '@/lib/seo'

export function NotFoundPage() {
  usePageMeta({
    title: 'Halaman tidak ditemukan',
    description: 'Alamat ini tidak ada di sini.',
    // The path is whatever was typed; the canonical points home so a mistyped
    // URL never competes with a real page.
    path: routes.home,
    noIndex: true,
  })

  return (
    <section className="shell flex min-h-[70vh] items-center py-20">
      <div className="document">
        <Label as="p">Halaman tidak ditemukan</Label>

        <h1 className="type-h1 mt-5">Alamat ini tidak ada di sini.</h1>

        <p className="type-lead mt-6">
          Mungkin tautannya salah ketik, atau halamannya sudah dipindahkan. Yang di bawah ini
          biasanya yang dicari orang.
        </p>

        <ul className="mt-9 grid gap-3">
          {mainNav.map((item) => (
            <li key={item.href}>
              <Link
                to={item.href}
                className="text-accent inline-flex items-center gap-2 font-semibold"
              >
                {item.label}
                <span aria-hidden="true">&rarr;</span>
              </Link>
            </li>
          ))}
        </ul>

        <div className="mt-10">
          <ButtonLink href={routes.home} variant="secondary" size="large">
            Kembali ke beranda
          </ButtonLink>
        </div>
      </div>
    </section>
  )
}
