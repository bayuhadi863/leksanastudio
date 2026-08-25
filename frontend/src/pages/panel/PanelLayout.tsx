import { useEffect, useMemo, useState } from 'react'
import { Link, NavLink, Outlet, useLocation, useNavigate } from 'react-router-dom'
import { toast } from 'sonner'

import { ThemeSwitch } from '@/components/ui/ThemeSwitch'
import { Label } from '@/components/ui/Label'
import { authRoutes, panelNav, panelRoutes } from '@/config/panel'
import { routes } from '@/config/routes'
import { site } from '@/config/site'
import { getErrorMessage } from '@/lib/api-error'
import { cn } from '@/lib/cn'
import { removeAuthTokens } from '@/lib/tokens'
import { authRepository } from '@/repositories/AuthRepository'
import { useActiveRoleStore } from '@/store/active-role-store'
import { useMenuStore } from '@/store/menu-store'
import type { AuthUser } from '@/types/auth'

/**
 * The shell every panel page renders inside.
 *
 * The navigation is built from the menus the server returned, not from a fixed
 * list — so an editor and an administrator genuinely see different sidebars,
 * and removing an access in the panel removes the link on the next load.
 */
export function PanelLayout() {
  const navigate = useNavigate()
  const { pathname } = useLocation()
  const canAccess = useMenuStore((state) => state.canAccess)
  const clearMenus = useMenuStore((state) => state.clearMenus)

  const roles = useActiveRoleStore((state) => state.roles)
  const setRoles = useActiveRoleStore((state) => state.setRoles)
  const activeRoleId = useActiveRoleStore((state) => state.activeRoleId)
  const clearActiveRole = useActiveRoleStore((state) => state.clear)

  const [user, setUser] = useState<AuthUser | null>(null)
  const [menuOpen, setMenuOpen] = useState(false)
  const [isSigningOut, setIsSigningOut] = useState(false)

  useEffect(() => {
    void authRepository
      .getUserInfo()
      .then((response) => {
        if (response.success && response.data) setUser(response.data)
      })
      .catch(() => {
        // The header simply shows less. The session itself is already
        // guaranteed by ProtectedRoute, so there is nothing to recover here.
      })
  }, [])

  useEffect(() => {
    if (roles.length > 0) return
    void authRepository
      .getMyRoles()
      .then((response) => {
        if (response.success && response.data) setRoles(response.data)
      })
      .catch(() => {
        // Role switcher stays hidden; everything else works.
      })
  }, [roles.length, setRoles])

  // A route change closes the drawer. Without this it stays open behind the
  // page the reader just asked for.
  useEffect(() => {
    setMenuOpen(false)
  }, [pathname])

  useEffect(() => {
    if (!menuOpen) return

    const onKeyDown = (event: KeyboardEvent) => {
      if (event.key === 'Escape') setMenuOpen(false)
    }

    document.addEventListener('keydown', onKeyDown)
    document.body.style.overflow = 'hidden'

    return () => {
      document.removeEventListener('keydown', onKeyDown)
      document.body.style.overflow = ''
    }
  }, [menuOpen])

  const sections = useMemo(
    () =>
      panelNav
        .map((section) => ({
          ...section,
          items: section.items.filter((item) => canAccess(item.menuCode)),
        }))
        .filter((section) => section.items.length > 0),
    [canAccess],
  )

  const activeRole = roles.find((role) => role.roleId === activeRoleId) ?? null

  const signOut = async () => {
    setIsSigningOut(true)
    try {
      // Best effort: the server revokes the refresh tokens and blacklists the
      // access token. Whether or not it answers, this browser is signed out.
      await authRepository.logout()
    } catch (error) {
      toast.error(getErrorMessage(error, 'Gagal keluar di server. Sesi lokal tetap dihapus.'))
    } finally {
      removeAuthTokens()
      clearActiveRole()
      clearMenus()
      navigate(authRoutes.login, { replace: true })
    }
  }

  const navigation = (
    <nav aria-label="Navigasi panel" className="grid gap-5">
      {sections.map((section) => (
        <div key={section.title}>
          <Label as="p">{section.title}</Label>
          <ul className="mt-2 grid">
            {section.items.map((item) => (
              <li key={item.href}>
                <NavLink
                  to={item.href}
                  className={({ isActive }) =>
                    cn(
                      'border-line block border-b py-2 transition-colors duration-150 ease-out',
                      isActive ? 'text-text' : 'text-muted hover:text-accent',
                    )
                  }
                >
                  {({ isActive }) => (
                    <span className="flex items-start gap-2.5">
                      <span
                        aria-hidden="true"
                        className={cn(
                          'mt-[0.7em] h-px w-2.5 shrink-0 transition-colors duration-150 ease-out',
                          isActive ? 'bg-accent' : 'bg-line',
                        )}
                      />
                      <span>
                        <span className="block font-semibold">{item.label}</span>
                        {/*
                          The one-line description belongs to the screen you are
                          on, not to all seventeen of them: kept for the active
                          item, where it still says what this area is for, and
                          dropped everywhere else, where seventeen of them turn
                          the rail into something you have to scroll.
                        */}
                        {isActive ? (
                          <span className="type-small text-muted block">{item.hint}</span>
                        ) : null}
                      </span>
                    </span>
                  )}
                </NavLink>
              </li>
            ))}
          </ul>
        </div>
      ))}

      {sections.length === 0 ? (
        <p className="type-small text-muted">
          Peran Anda belum diberi akses ke satu pun menu. Hubungi administrator.
        </p>
      ) : null}
    </nav>
  )

  // `minmax(0,1fr)` rather than a bare `1fr`: a `1fr` track refuses to shrink
  // below its own content, so at exactly the width where the rail appears the
  // content column pushes the whole page sideways.
  return (
    <div className="panel-ui type-scale-tool bg-bg min-h-dvh lg:grid lg:grid-cols-[15rem_minmax(0,1fr)]">
      {/* Desktop rail */}
      <aside className="border-line bg-surface hidden h-dvh flex-col justify-between overflow-y-auto border-r px-5 py-6 lg:sticky lg:top-0 lg:flex">
        <div>
          <Link
            to={panelRoutes.dashboard}
            className="font-display text-[1.125rem] leading-none font-semibold tracking-[-0.02em]"
          >
            {site.name}
            <span className="text-accent">.</span>
          </Link>
          <Label as="p" className="mt-1.5">
            Panel
          </Label>

          <div className="mt-7">{navigation}</div>
        </div>

        {/*
          Reading preference sits at the bottom of the rail, not in the page
          footer: a content form can run ten thousand pixels tall, and a control
          that far down is a control nobody finds. Here it is on screen whatever
          the page is doing, and grouped with the other workspace-level link
          rather than with the actions of whichever screen is open.
        */}
        <div className="border-line mt-8 border-t pt-4">
          <Link to={routes.home} className="type-small text-muted hover:text-accent">
            Lihat situs &rarr;
          </Link>

          <ThemeSwitch layout="stacked" className="mt-4" />
        </div>
      </aside>

      <div className="flex min-h-dvh flex-col">
        <header className="border-line bg-bg sticky top-0 z-40 border-b">
          <div className="flex items-center justify-between gap-4 px-5 py-2.5 lg:px-7">
            <div className="flex items-center gap-4">
              <button
                type="button"
                onClick={() => setMenuOpen((open) => !open)}
                aria-expanded={menuOpen}
                aria-controls="panel-menu"
                className="type-label text-text -ml-2 flex min-h-11 items-center gap-2 px-2 lg:hidden"
              >
                {menuOpen ? 'Tutup' : 'Menu'}
              </button>

              <div className="lg:hidden">
                <Link
                  to={panelRoutes.dashboard}
                  className="font-display text-[1.125rem] leading-none font-semibold tracking-[-0.02em]"
                >
                  {site.name}
                  <span className="text-accent">.</span>
                </Link>
              </div>

              {activeRole ? (
                <p className="hidden lg:block">
                  <Label as="span">Peran aktif</Label>
                  <span className="ml-3 font-semibold">
                    {activeRole.roleName ?? activeRole.roleCode}
                  </span>
                </p>
              ) : null}
            </div>

            <div className="flex items-center gap-5">
              <p className="type-small hidden font-semibold sm:block">
                {user?.name ?? 'Memuat…'}
              </p>

              {roles.length > 1 ? (
                <Link
                  to={authRoutes.selectRole}
                  className="type-small text-muted hover:text-accent"
                >
                  Ganti peran
                </Link>
              ) : null}

              <button
                type="button"
                onClick={() => void signOut()}
                disabled={isSigningOut}
                className="type-small text-muted hover:text-accent disabled:opacity-45"
              >
                {isSigningOut ? 'Keluar…' : 'Keluar'}
              </button>
            </div>
          </div>
        </header>

        {/* Mobile drawer */}
        {menuOpen ? (
          <div
            id="panel-menu"
            className="bg-bg fixed inset-x-0 top-[3.25rem] bottom-0 z-40 overflow-y-auto px-5 py-6 lg:hidden"
          >
            {navigation}
            <div className="border-line mt-10 border-t pt-6">
              <Link to={routes.home} className="type-small text-muted hover:text-accent">
                Lihat situs &rarr;
              </Link>

              <ThemeSwitch layout="stacked" className="mt-5 max-w-xs" />
            </div>
          </div>
        ) : null}

        <main id="panel-konten" className="flex-1 px-5 py-6 lg:px-7 lg:py-8">
          <Outlet />
        </main>

        <footer className="border-line border-t px-5 py-4 lg:px-7">
          <p className="type-small text-muted">{site.legalName} · Panel pengelolaan</p>
        </footer>
      </div>
    </div>
  )
}
