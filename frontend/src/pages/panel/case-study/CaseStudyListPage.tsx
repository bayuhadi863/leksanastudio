import { useCallback, useEffect, useMemo, useState } from 'react'
import { Link, useNavigate } from 'react-router-dom'

import { DataTable, EmptyState, type Column } from '@/components/panel/DataTable'
import { PanelPage } from '@/components/panel/PanelPage'
import { TranslationChips } from '@/components/panel/StatusChip'
import { SlugPreview } from '@/components/panel/form/SlugInput'
import { ButtonLink } from '@/components/ui/Button'
import { Label } from '@/components/ui/Label'
import { menuCodes, panelRoutes } from '@/config/panel'
import { usePermission } from '@/hooks/usePermission'
import { formatDate } from '@/lib/format'
import { usePageMeta } from '@/lib/seo'
import { caseStudyRepository } from '@/repositories/CaseStudyRepository'
import type { CaseStudyPaginationDTO, ContentStatus } from '@/types/content'

const LABEL_TEXT: Record<CaseStudyPaginationDTO['label'], string> = {
  Client: 'Klien',
  OwnProduct: 'Produk sendiri',
}

/**
 * Every case study, in the order they appear on the site.
 *
 * Sorted by display order rather than by date, because that is the question
 * being asked here — "what does a visitor see first?" — and sorting by anything
 * else would make the reorder control lie.
 */
export function CaseStudyListPage() {
  usePageMeta({
    title: 'Portofolio',
    description: 'Kelola studi kasus yang tampil di situs.',
    path: panelRoutes.caseStudies,
    noIndex: true,
  })

  const navigate = useNavigate()
  const permission = usePermission(menuCodes.caseStudy)

  const [rows, setRows] = useState<CaseStudyPaginationDTO[]>([])
  const [isLoading, setIsLoading] = useState(true)
  const [error, setError] = useState<unknown>(null)
  const [search, setSearch] = useState('')
  const [status, setStatus] = useState<ContentStatus | ''>('')

  const load = useCallback(async () => {
    setIsLoading(true)
    setError(null)
    try {
      const response = await caseStudyRepository.getPagination({
        page: 1,
        pageSize: 100,
        search: search || undefined,
        status: status || undefined,
      })
      setRows(response.data?.items ?? [])
    } catch (caught) {
      setError(caught)
    } finally {
      setIsLoading(false)
    }
  }, [search, status])

  useEffect(() => {
    const timer = window.setTimeout(() => void load(), search ? 250 : 0)
    return () => window.clearTimeout(timer)
  }, [load, search])

  const columns = useMemo<Column<CaseStudyPaginationDTO>[]>(
    () => [
      {
        key: 'title',
        header: 'Studi kasus',
        width: '42%',
        render: (row) => (
          <>
            <span className="block font-semibold">{row.title ?? 'Tanpa judul'}</span>
            <SlugPreview prefix="/portofolio/" slug={row.slug} />
          </>
        ),
      },
      {
        key: 'label',
        header: 'Label',
        secondary: true,
        render: (row) => <span className="type-small">{LABEL_TEXT[row.label]}</span>,
      },
      {
        key: 'year',
        header: 'Tahun',
        secondary: true,
        render: (row) => <span className="type-small numeric">{row.year}</span>,
      },
      {
        key: 'status',
        header: 'Bahasa',
        render: (row) => <TranslationChips translations={row.translations} />,
      },
      {
        key: 'updated',
        header: 'Diperbarui',
        secondary: true,
        align: 'right',
        render: (row) => (
          <span className="type-small text-muted">
            {row.updatedDate ? formatDate(row.updatedDate) : '—'}
          </span>
        ),
      },
    ],
    [],
  )

  return (
    <PanelPage
      eyebrow="Konten"
      title="Portofolio"
      lead="Pekerjaan yang bisa diperiksa. Tiap entri punya alamatnya sendiri dan status terbit per bahasa."
      actions={
        permission.canCreate ? (
          <ButtonLink href={panelRoutes.caseStudyCreate}>Tambah studi kasus</ButtonLink>
        ) : null
      }
      wide
    >
      <div className="border-line flex flex-wrap items-end gap-x-6 gap-y-4 border-b pb-5">
        <div className="min-w-56 flex-1 sm:max-w-sm">
          <label htmlFor="cs-search" className="sr-only">
            Cari studi kasus
          </label>
          <input
            id="cs-search"
            type="search"
            value={search}
            onChange={(event) => setSearch(event.target.value)}
            placeholder="Cari judul, alamat, atau klien…"
            className="border-muted bg-surface focus:border-accent focus:ring-accent/20 w-full rounded-[var(--radius-control)] border px-4 py-2.5 transition-colors duration-150 ease-out focus:ring-3 focus:outline-none"
          />
        </div>

        <div>
          <Label as="p" className="mb-1.5">
            Status
          </Label>
          <div className="border-line flex rounded-[var(--radius-control)] border">
            {(
              [
                { value: '', label: 'Semua' },
                { value: 'Published', label: 'Terbit' },
                { value: 'Draft', label: 'Draf' },
              ] as const
            ).map((option) => (
              <button
                key={option.value}
                type="button"
                onClick={() => setStatus(option.value)}
                className={
                  'type-label flex min-h-11 items-center px-3.5 transition-colors duration-150 ease-out first:rounded-l-[3px] last:rounded-r-[3px] ' +
                  (status === option.value
                    ? 'bg-accent-soft text-accent'
                    : 'text-muted hover:text-text')
                }
              >
                {option.label}
              </button>
            ))}
          </div>
        </div>
      </div>

      <div className="mt-8">
        <DataTable
          caption="Daftar studi kasus"
          columns={columns}
          rows={rows}
          rowKey={(row) => row.id}
          isLoading={isLoading}
          error={error}
          onRetry={() => void load()}
          onRowClick={(row) => navigate(panelRoutes.caseStudyEdit(row.id))}
          empty={
            search || status ? (
              <EmptyState
                title="Tidak ada yang cocok"
                body="Coba kata kunci lain, atau kosongkan penyaringnya."
              />
            ) : (
              <EmptyState
                title="Belum ada studi kasus"
                body="Studi kasus adalah bukti pertama yang dibaca calon klien. Mulai dari satu proyek yang paling mewakili."
                action={
                  permission.canCreate ? (
                    <Link
                      to={panelRoutes.caseStudyCreate}
                      className="text-accent inline-flex items-center gap-2 font-semibold"
                    >
                      Tambah studi kasus pertama
                      <span aria-hidden="true">&rarr;</span>
                    </Link>
                  ) : null
                }
              />
            )
          }
        />
      </div>
    </PanelPage>
  )
}
