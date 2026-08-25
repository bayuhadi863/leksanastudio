import type { ReactNode } from 'react'
import { Link } from 'react-router-dom'

import { Label } from '@/components/ui/Label'
import { routes } from '@/config/routes'
import { site } from '@/config/site'

type Props = {
  readonly eyebrow: string
  readonly title: string
  readonly lead: string
  readonly children: ReactNode
}

/**
 * The sign-in shell: a letterhead on the left, the form on the right.
 *
 * The left column is the same document furniture the public site uses — an
 * accent rule, the wordmark, one plain sentence. The panel should read like
 * the back of the same piece of paper, not like a different product.
 */
export function AuthLayout({ eyebrow, title, lead, children }: Props) {
  return (
    <div className="bg-bg min-h-dvh lg:grid lg:grid-cols-[1fr_1.1fr]">
      <aside className="border-line bg-surface hidden flex-col justify-between border-r p-12 lg:flex xl:p-16">
        <Link
          to={routes.home}
          className="font-display text-[1.375rem] leading-none font-semibold tracking-[-0.02em]"
        >
          {site.name}
          <span className="text-accent">.</span>
        </Link>

        <div className="max-w-sm">
          <span aria-hidden="true" className="bg-accent block h-1 w-16" />
          <p className="type-h2 mt-8">{site.tagline}</p>
          <p className="text-muted mt-5">
            Panel ini bagian dari janji itu: situs yang bisa Anda urus sendiri, tanpa menunggu siapa
            pun.
          </p>
        </div>

        <div className="border-line border-t pt-6">
          <Label as="p">
            {site.city}, Indonesia · {site.legalName}
          </Label>
        </div>
      </aside>

      <main className="flex min-h-dvh items-center px-6 py-14 sm:px-10 lg:px-16 xl:px-24">
        <div className="w-full max-w-md">
          <Link
            to={routes.home}
            className="font-display mb-10 inline-block text-[1.375rem] leading-none font-semibold tracking-[-0.02em] lg:hidden"
          >
            {site.name}
            <span className="text-accent">.</span>
          </Link>

          <Label as="p">{eyebrow}</Label>
          <h1 className="type-h1 mt-4">{title}</h1>
          <p className="type-lead text-muted mt-4">{lead}</p>

          <div className="mt-10">{children}</div>
        </div>
      </main>
    </div>
  )
}
