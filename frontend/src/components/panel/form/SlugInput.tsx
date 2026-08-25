import { useId } from 'react'

import { FieldShell } from '@/components/panel/form/Field'
import { Label } from '@/components/ui/Label'
import { slugify } from '@/lib/slug'

type Props = {
  readonly value: string
  readonly onChange: (value: string) => void
  readonly error?: string
  /** Path the slug hangs off, e.g. <c>/portofolio/</c>. Shown so the address is never abstract. */
  readonly prefix: string
  readonly siteUrl?: string
  /**
   * True once this address has been served to the public. Changing it then costs
   * a redirect, and the editor deserves to know before they do it — not after.
   */
  readonly published?: boolean
  readonly disabled?: boolean
}

export function SlugInput({
  value,
  onChange,
  error,
  prefix,
  siteUrl = 'leksana.id',
  published,
  disabled,
}: Props) {
  const id = useId()

  return (
    <FieldShell
      id={id}
      label="Alamat halaman"
      hint="Bagian terakhir URL. Huruf kecil, angka, dan tanda hubung."
      error={error}
      required
    >
      <div className="border-muted bg-surface focus-within:border-accent focus-within:ring-accent/20 flex items-stretch overflow-hidden rounded-[var(--radius-control)] border transition-colors duration-150 ease-out focus-within:ring-3">
        <span className="border-line text-muted type-small hidden items-center border-r px-3 font-mono sm:flex">
          {siteUrl}
          {prefix}
        </span>
        <input
          id={id}
          type="text"
          value={value}
          disabled={disabled}
          aria-invalid={Boolean(error)}
          onChange={(event) => onChange(slugify(event.target.value))}
          onBlur={(event) => onChange(slugify(event.target.value))}
          className="flex-1 bg-transparent px-4 py-3 font-mono text-[0.9375rem] focus:outline-none"
        />
      </div>

      <p className="type-small text-muted mt-2 sm:hidden">
        <span className="font-mono">
          {siteUrl}
          {prefix}
          {value}
        </span>
      </p>

      {published ? (
        <p className="type-small text-muted border-line mt-3 border-l-2 pl-3">
          Alamat ini sudah tayang. Kalau diubah, alamat lama tetap dialihkan otomatis — tetapi
          tautan yang sudah dibagikan sebaiknya dibagikan ulang.
        </p>
      ) : null}
    </FieldShell>
  )
}

/** Small helper for showing what an entry's address will be, outside a form field. */
export function SlugPreview({
  prefix,
  slug,
  siteUrl = 'leksana.id',
}: {
  readonly prefix: string
  readonly slug: string | null
  readonly siteUrl?: string
}) {
  if (!slug) return <Label as="span">Belum ada alamat</Label>

  return (
    <span className="type-small text-muted font-mono break-all">
      {siteUrl}
      {prefix}
      {slug}
    </span>
  )
}
