import { Outlet } from 'react-router-dom'

import { Footer } from '@/components/layout/Footer'
import { Header } from '@/components/layout/Header'
import { JsonLd } from '@/components/layout/JsonLd'
import { SkipLink } from '@/components/layout/SkipLink'
import { organizationSchema } from '@/lib/structured-data'

/**
 * The public site's shell. The panel and the sign-in screen deliberately do
 * not use it — a management tool has no business carrying a marketing nav.
 */
export function SiteLayout() {
  return (
    <>
      <SkipLink />
      <Header />

      <main id="konten">
        <Outlet />
      </main>

      <Footer />

      <JsonLd data={organizationSchema()} />
    </>
  )
}
