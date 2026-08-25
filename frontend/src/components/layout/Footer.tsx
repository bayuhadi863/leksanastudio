import { Link } from 'react-router-dom'

import { Label } from '@/components/ui/Label'
import { ThemeSwitch } from '@/components/ui/ThemeSwitch'
import { footerNav, routes } from '@/config/routes'
import { site } from '@/config/site'

const YEAR = new Date().getFullYear()

const socialLinks = [
  { label: 'LinkedIn', href: site.social.linkedin },
  { label: 'Instagram', href: site.social.instagram },
  { label: 'GitHub', href: site.social.github },
] as const

export function Footer() {
  return (
    <footer className="border-line border-t">
      <div className="shell py-16 lg:py-20">
        <div className="grid gap-12 lg:grid-cols-[1.4fr_repeat(3,1fr)] lg:gap-10">
          <div>
            <Link
              to={routes.home}
              className="font-display text-[1.375rem] leading-none font-semibold tracking-[-0.02em]"
            >
              {site.name}
              <span className="text-accent">.</span>
            </Link>
            <p className="type-small text-muted mt-4 max-w-xs">
              Website dan sistem web. {site.city}, Indonesia.
            </p>
            <p className="type-small mt-6">
              <a href={`mailto:${site.email}`} className="hover:text-accent">
                {site.email}
              </a>
              <br />
              <a href={`tel:+${site.whatsapp.number}`} className="numeric hover:text-accent">
                {site.whatsapp.display}
              </a>
            </p>
          </div>

          {footerNav.map((group) => (
            <nav key={group.title} aria-label={group.title}>
              <Label as="p">{group.title}</Label>
              <ul className="mt-4 grid gap-2.5">
                {group.items.map((item) => (
                  <li key={item.href}>
                    <Link to={item.href} className="type-small text-muted hover:text-accent">
                      {item.label}
                    </Link>
                  </li>
                ))}
              </ul>
            </nav>
          ))}
        </div>

        <div className="border-line mt-14 border-t pt-6">
          <ThemeSwitch className="mb-6" />
        </div>

        <div className="flex flex-col gap-4 sm:flex-row sm:items-center sm:justify-between">
          <p className="type-small text-muted">
            © {YEAR} {site.legalName} · {site.ownerName}
          </p>
          <ul className="flex flex-wrap gap-x-6 gap-y-2">
            {socialLinks.map((item) => (
              <li key={item.href}>
                <a
                  href={item.href}
                  target="_blank"
                  rel="noopener noreferrer"
                  className="type-small text-muted hover:text-accent"
                >
                  {item.label}
                </a>
              </li>
            ))}
          </ul>
        </div>
      </div>
    </footer>
  )
}
