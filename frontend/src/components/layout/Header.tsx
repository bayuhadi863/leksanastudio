import { useEffect, useState } from 'react'
import { Link, useLocation } from 'react-router-dom'

import { ButtonLink } from '@/components/ui/Button'
import { mainNav, routes } from '@/config/routes'
import { site } from '@/config/site'
import { cn } from '@/lib/cn'
import { whatsappLink, whatsappMessages } from '@/lib/whatsapp'

const Wordmark = () => (
  <Link
    to={routes.home}
    className="font-display flex h-[var(--header-height)] items-center text-[1.375rem] leading-none font-semibold tracking-[-0.02em]"
  >
    {site.name}
    <span className="text-accent">.</span>
  </Link>
)

export function Header() {
  const { pathname } = useLocation()
  const [scrolled, setScrolled] = useState(false)
  const [menuOpen, setMenuOpen] = useState(false)

  useEffect(() => {
    const onScroll = () => setScrolled(window.scrollY > 40)
    onScroll()
    window.addEventListener('scroll', onScroll, { passive: true })
    return () => window.removeEventListener('scroll', onScroll)
  }, [])

  // Route change closes the menu. Without this it stays open behind the new page.
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

  const isActive = (href: string) =>
    href === routes.home ? pathname === href : pathname.startsWith(href)

  return (
    <header
      className={cn(
        'sticky top-0 z-50 transition-colors duration-150 ease-out',
        scrolled ? 'border-line bg-bg border-b' : 'bg-bg border-b border-transparent',
      )}
    >
      <div className="shell flex h-[var(--header-height)] items-center justify-between gap-6">
        <Wordmark />

        <nav aria-label="Navigasi utama" className="hidden items-center gap-7 lg:flex">
          {mainNav.map((item) => (
            <Link
              key={item.href}
              to={item.href}
              aria-current={isActive(item.href) ? 'page' : undefined}
              className={cn(
                'type-small hover:text-accent relative py-1 transition-colors duration-150 ease-out',
                isActive(item.href) ? 'text-text' : 'text-muted',
              )}
            >
              {item.label}
              {isActive(item.href) ? (
                <span
                  aria-hidden="true"
                  className="bg-accent absolute inset-x-0 -bottom-0.5 h-0.5"
                />
              ) : null}
            </Link>
          ))}
        </nav>

        <div className="hidden lg:block">
          <ButtonLink href={whatsappLink(whatsappMessages.default)}>
            Diskusi lewat WhatsApp
          </ButtonLink>
        </div>

        <button
          type="button"
          onClick={() => setMenuOpen((open) => !open)}
          aria-expanded={menuOpen}
          aria-controls="menu-utama"
          className="type-label text-text -mr-2 flex min-h-11 items-center gap-2 px-2 lg:hidden"
        >
          {menuOpen ? 'Tutup' : 'Menu'}
          <span aria-hidden="true" className="grid gap-[3px]">
            <span
              className={cn(
                'block h-px w-4 bg-current transition-transform duration-150',
                menuOpen && 'translate-y-[4px] rotate-45',
              )}
            />
            <span
              className={cn(
                'block h-px w-4 bg-current transition-opacity duration-150',
                menuOpen && 'opacity-0',
              )}
            />
            <span
              className={cn(
                'block h-px w-4 bg-current transition-transform duration-150',
                menuOpen && '-translate-y-[4px] -rotate-45',
              )}
            />
          </span>
        </button>
      </div>

      {menuOpen ? (
        <div
          id="menu-utama"
          className="bg-bg fixed inset-x-0 top-[var(--header-height)] bottom-0 z-40 overflow-y-auto lg:hidden"
        >
          <nav aria-label="Navigasi utama" className="shell flex flex-col gap-1 pt-6 pb-10">
            {mainNav.map((item) => (
              <Link
                key={item.href}
                to={item.href}
                className="type-h3 border-line hover:text-accent border-b py-4"
              >
                {item.label}
              </Link>
            ))}

            <Link to={routes.notes} className="type-h3 border-line hover:text-accent border-b py-4">
              Catatan teknis
            </Link>

            <div className="mt-8">
              <ButtonLink
                href={whatsappLink(whatsappMessages.default)}
                size="large"
                className="w-full"
              >
                Diskusi lewat WhatsApp
              </ButtonLink>
            </div>

            <p className="type-small text-muted mt-4">
              Dibalas dalam {site.promises.replyWithinHours} jam pada jam kerja.
            </p>
          </nav>
        </div>
      ) : null}
    </header>
  )
}
