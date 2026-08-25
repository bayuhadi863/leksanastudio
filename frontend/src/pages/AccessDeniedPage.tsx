import { Link } from 'react-router-dom'

import { Button } from '@/components/ui/Button'
import { Label } from '@/components/ui/Label'
import { authRoutes } from '@/config/panel'
import { routes } from '@/config/routes'
import { usePageMeta } from '@/lib/seo'
import { useActiveRoleStore } from '@/store/active-role-store'

export function AccessDeniedPage() {
  usePageMeta({
    title: 'Akses ditolak',
    description: 'Peran Anda tidak memiliki akses ke halaman ini.',
    path: routes.home,
    noIndex: true,
  })

  const roles = useActiveRoleStore((state) => state.roles)

  return (
    <section className="shell flex min-h-dvh items-center py-20">
      <div className="document">
        <Label as="p">Akses ditolak</Label>

        <h1 className="type-h1 mt-5">Peran Anda tidak mencakup halaman ini.</h1>

        <p className="type-lead mt-6">
          Bukan halaman yang hilang — hanya tidak termasuk yang boleh Anda buka. Kalau menurut Anda
          seharusnya boleh, minta administrator menambahkan aksesnya.
        </p>

        <div className="mt-9 flex flex-col gap-3 sm:flex-row">
          {roles.length > 1 ? (
            <Button
              variant="secondary"
              size="large"
              onClick={() => {
                window.location.href = authRoutes.selectRole
              }}
            >
              Ganti peran
            </Button>
          ) : null}
          <Link
            to={routes.home}
            className="text-accent inline-flex items-center gap-2 self-center font-semibold sm:ml-2"
          >
            <span aria-hidden="true">&larr;</span>
            Kembali ke situs
          </Link>
        </div>
      </div>
    </section>
  )
}
