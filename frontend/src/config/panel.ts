/**
 * The management panel: routes, menu codes, and the navigation that renders
 * from them.
 *
 * Menu codes are the contract with the backend — a code here must exist in the
 * `menu` table (see MenuSeeder), because that is what a role's permissions
 * attach to. Adding a managed area means one entry in `panelNav` and one line
 * in the seeder; nothing else changes.
 */

export const PANEL_BASE = '/panel'

export const panelRoutes = {
  base: PANEL_BASE,
  dashboard: `${PANEL_BASE}/dasbor`,

  caseStudies: `${PANEL_BASE}/portofolio`,
  caseStudyCreate: `${PANEL_BASE}/portofolio/baru`,
  caseStudyEdit: (id: string) => `${PANEL_BASE}/portofolio/${id}`,

  media: `${PANEL_BASE}/berkas`,
} as const

export const authRoutes = {
  base: '/auth',
  login: '/auth/masuk',
  selectRole: '/auth/pilih-peran',
} as const

export const accessDeniedRoute = '/akses-ditolak'

/** Backend menu codes. Must match `MenuSeeder` in the API. */
export const menuCodes = {
  dashboard: 'dashboard',
  caseStudy: 'case-study',
  note: 'note',
  service: 'service',
  servicePackage: 'service-package',
  projectPhase: 'project-phase',
  addOn: 'add-on',
  paymentTerm: 'payment-term',
  processStep: 'process-step',
  vertical: 'vertical',
  pageCopy: 'page-copy',
  pageDocument: 'page-document',
  media: 'media',
  siteProfile: 'site-profile',
  locale: 'locale',
  user: 'user',
  role: 'role',
} as const

export type NavItem = {
  readonly label: string
  /** Short line under the label in the sidebar. Kept to one clause. */
  readonly hint: string
  readonly href: string
  readonly menuCode: string
}

export type NavSection = {
  readonly title: string
  readonly items: readonly NavItem[]
}

/**
 * What the sidebar renders — and, in this order, where a user lands when their
 * role has no default menu: the first entry they can actually see.
 *
 * Only areas with a real page belong here. The remaining content menus are
 * seeded and enforced on the API already; their entries arrive on the day their
 * pages do, not before.
 */
export const panelNav: readonly NavSection[] = [
  {
    title: 'Ringkasan',
    items: [
      {
        label: 'Dasbor',
        hint: 'Keadaan panel dan sesi Anda',
        href: panelRoutes.dashboard,
        menuCode: menuCodes.dashboard,
      },
    ],
  },
  {
    title: 'Konten',
    items: [
      {
        label: 'Portofolio',
        hint: 'Studi kasus yang jadi bukti',
        href: panelRoutes.caseStudies,
        menuCode: menuCodes.caseStudy,
      },
      {
        label: 'Berkas & Gambar',
        hint: 'Tangkapan layar dan unggahan',
        href: panelRoutes.media,
        menuCode: menuCodes.media,
      },
    ],
  },
]

const navLeaves = (): readonly NavItem[] => panelNav.flatMap((section) => section.items)

/** Route path for a menu code, or null when the code has no panel page yet. */
export const menuPath = (menuCode: string | null | undefined): string | null => {
  if (!menuCode) return null
  const code = menuCode.toLowerCase()
  return navLeaves().find((item) => item.menuCode.toLowerCase() === code)?.href ?? null
}

/** Menu code owning a pathname (longest matching prefix), or null. */
export const menuCodeForPath = (pathname: string): string | null => {
  let match: NavItem | null = null
  for (const item of navLeaves()) {
    if (pathname === item.href || pathname.startsWith(`${item.href}/`)) {
      if (!match || item.href.length > match.href.length) match = item
    }
  }
  return match?.menuCode ?? null
}

/**
 * Where to land after login or a role switch: the role's default menu when it
 * is actually accessible, else the first accessible menu, else access-denied.
 * Never a hard-coded dashboard — a role may legitimately not have one.
 */
export const resolveLandingPath = (
  defaultMenuCode: string | null | undefined,
  canAccess: (menuCode: string) => boolean,
): string => {
  if (defaultMenuCode && canAccess(defaultMenuCode)) {
    const path = menuPath(defaultMenuCode)
    if (path) return path
  }
  return navLeaves().find((item) => canAccess(item.menuCode))?.href ?? accessDeniedRoute
}
