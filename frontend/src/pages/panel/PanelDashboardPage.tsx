import { useEffect, useState } from 'react'

import { Label } from '@/components/ui/Label'
import { menuCodes, panelRoutes } from '@/config/panel'
import { site } from '@/config/site'
import { usePermission } from '@/hooks/usePermission'
import { formatDate } from '@/lib/format'
import { usePageMeta } from '@/lib/seo'
import { authRepository } from '@/repositories/AuthRepository'
import { useActiveRoleStore } from '@/store/active-role-store'
import { useMenuStore } from '@/store/menu-store'
import type { AuthUser } from '@/types/auth'

/**
 * The landing page inside the panel.
 *
 * It reports what the session actually is — who is signed in, under which
 * role, and exactly which menus that role grants. Useful on its own, and the
 * fastest way to see whether a permission change took effect.
 */
export function PanelDashboardPage() {
  usePageMeta({
    title: 'Dasbor',
    description: 'Ringkasan sesi dan akses Anda di panel Leksana Studio.',
    path: panelRoutes.dashboard,
    noIndex: true,
  })

  const menus = useMenuStore((state) => state.menus)
  const roles = useActiveRoleStore((state) => state.roles)
  const activeRoleId = useActiveRoleStore((state) => state.activeRoleId)
  const dashboard = usePermission(menuCodes.dashboard)

  const [user, setUser] = useState<AuthUser | null>(null)

  useEffect(() => {
    void authRepository
      .getUserInfo()
      .then((response) => {
        if (response.success && response.data) setUser(response.data)
      })
      .catch(() => {
        // Identity is decoration here; the page stands without it.
      })
  }, [])

  const activeRole = roles.find((role) => role.roleId === activeRoleId) ?? null

  return (
    <div className="mx-auto max-w-4xl">
      <header>
        <Label as="p">Dasbor</Label>
        <h1 className="type-h1 mt-4">
          {user?.name ? `Selamat datang, ${user.name.split(' ')[0]}.` : 'Selamat datang.'}
        </h1>
        <p className="type-lead text-muted mt-5 max-w-[var(--measure)]">
          Panel ini akan mengurus isi situs {site.legalName} — portofolio, catatan, harga, dan teks
          halaman. Untuk sekarang yang tersedia baru pengelolaan sesi dan akses.
        </p>
      </header>

      <section className="mt-14">
        <Label as="p">Sesi Anda</Label>
        <dl className="mt-5 grid gap-x-10 gap-y-6 sm:grid-cols-2 lg:grid-cols-3">
          <div className="border-line border-t pt-4">
            <dt className="type-small text-muted">Nama</dt>
            <dd className="mt-2 font-semibold">{user?.name ?? '—'}</dd>
          </div>
          <div className="border-line border-t pt-4">
            <dt className="type-small text-muted">Email</dt>
            <dd className="mt-2 font-semibold break-all">{user?.email ?? '—'}</dd>
          </div>
          <div className="border-line border-t pt-4">
            <dt className="type-small text-muted">Peran aktif</dt>
            <dd className="mt-2 font-semibold">
              {activeRole?.roleName ?? activeRole?.roleCode ?? 'Peran utama'}
            </dd>
          </div>
          <div className="border-line border-t pt-4">
            <dt className="type-small text-muted">Peran dimiliki</dt>
            <dd className="mt-2 font-semibold">{roles.length || '—'}</dd>
          </div>
          <div className="border-line border-t pt-4">
            <dt className="type-small text-muted">Menu dapat diakses</dt>
            <dd className="numeric mt-2 font-semibold">{menus.length}</dd>
          </div>
          <div className="border-line border-t pt-4">
            <dt className="type-small text-muted">Akun dibuat</dt>
            <dd className="mt-2 font-semibold">
              {user?.createdDate ? formatDate(user.createdDate) : '—'}
            </dd>
          </div>
        </dl>
      </section>

      <section className="mt-14">
        <Label as="p">Hak akses</Label>
        <h2 className="type-h2 mt-3">Yang boleh Anda lakukan</h2>
        <p className="text-muted mt-3 max-w-[var(--measure)]">
          Daftar ini datang dari server, bukan dari tebakan di sisi peramban. Kalau administrator
          mengubah izin, baris di bawah ikut berubah begitu Anda memuat ulang.
        </p>

        <div className="scroll-x mt-8">
          <table className="w-full min-w-2xl text-left">
            <caption className="sr-only">Menu dan izin untuk peran aktif</caption>
            <thead>
              <tr className="border-line border-b">
                <th scope="col" className="pb-3">
                  <Label as="span">Menu</Label>
                </th>
                <th scope="col" className="pb-3">
                  <Label as="span">Lihat</Label>
                </th>
                <th scope="col" className="pb-3">
                  <Label as="span">Tambah</Label>
                </th>
                <th scope="col" className="pb-3">
                  <Label as="span">Ubah</Label>
                </th>
                <th scope="col" className="pb-3">
                  <Label as="span">Hapus</Label>
                </th>
              </tr>
            </thead>
            <tbody>
              {menus.map((menu) => (
                <tr key={menu.id} className="border-line border-b">
                  <th scope="row" className="type-small py-4 pr-8 font-semibold">
                    {menu.name ?? menu.code}
                    <span className="type-label text-muted mt-1 block">{menu.code}</span>
                  </th>
                  <PermissionCell granted={menu.canView} />
                  <PermissionCell granted={menu.canCreate} />
                  <PermissionCell granted={menu.canUpdate} />
                  <PermissionCell granted={menu.canDelete} />
                </tr>
              ))}

              {menus.length === 0 ? (
                <tr className="border-line border-b">
                  <td colSpan={5} className="type-small text-muted py-6">
                    Belum ada menu yang diberikan ke peran ini.
                  </td>
                </tr>
              ) : null}
            </tbody>
          </table>
        </div>

        {!dashboard.canCreate ? (
          <p className="type-small text-muted mt-6 max-w-[var(--measure)]">
            Dasbor sengaja hanya bisa dilihat — tidak ada yang bisa dibuat atau dihapus di halaman
            ini.
          </p>
        ) : null}
      </section>
    </div>
  )
}

function PermissionCell({ granted }: { readonly granted: boolean }) {
  return (
    <td className="py-4 pr-8">
      {granted ? (
        <>
          <span aria-hidden="true" className="text-accent">
            ✓
          </span>
          <span className="sr-only">Diizinkan</span>
        </>
      ) : (
        <>
          <span aria-hidden="true" className="text-muted opacity-45">
            —
          </span>
          <span className="sr-only">Tidak diizinkan</span>
        </>
      )}
    </td>
  )
}
