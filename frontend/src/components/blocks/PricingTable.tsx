import { ButtonLink } from '@/components/ui/Button'
import { Label } from '@/components/ui/Label'
import type { FeatureValue, ServicePackage } from '@/config/packages'
import { formatIDR } from '@/config/site'
import { cn } from '@/lib/cn'
import { whatsappLink } from '@/lib/whatsapp'

type Props = {
  readonly packages: readonly ServicePackage[]
  readonly whatsappIntro: string
}

const FeatureCell = ({ value }: { readonly value: FeatureValue }) => {
  if (value === true) {
    return (
      <>
        <span aria-hidden="true" className="text-accent">
          ✓
        </span>
        <span className="sr-only">Termasuk</span>
      </>
    )
  }

  if (value === false) {
    return (
      <>
        <span aria-hidden="true" className="text-muted opacity-45">
          —
        </span>
        <span className="sr-only">Tidak termasuk</span>
      </>
    )
  }

  return <span className="type-small">{value}</span>
}

/**
 * One table, not three floating cards.
 *
 * A document presents prices in a table, and a table is genuinely easier to
 * compare than three separate columns — which is the only job this block has.
 */
export function PricingTable({ packages, whatsappIntro }: Props) {
  const featureLabels = packages[0]?.features.map((feature) => feature.label) ?? []

  // On phones the recommended package leads: nobody scrolls three cards to
  // find the one they should have seen first.
  const stacked = [...packages].sort(
    (a, b) => Number(Boolean(b.highlighted)) - Number(Boolean(a.highlighted)),
  )

  return (
    <>
      {/* Phone: stacked cards */}
      <div className="grid gap-6 lg:hidden">
        {stacked.map((servicePackage) => (
          <article
            key={servicePackage.code}
            className={cn(
              'border p-6',
              servicePackage.highlighted ? 'border-accent bg-accent-soft' : 'border-line',
            )}
          >
            <div className="flex items-baseline justify-between gap-4">
              <Label as="p">{servicePackage.code}</Label>
              {servicePackage.highlighted ? (
                <Label as="p" className="text-accent">
                  Paling sering dipilih
                </Label>
              ) : null}
            </div>

            <h3 className="type-h3 mt-3">{servicePackage.name}</h3>
            <p className="type-small text-muted mt-1">{servicePackage.audience}</p>

            <p className="numeric font-display mt-5 text-[1.75rem] leading-none font-semibold">
              {formatIDR(servicePackage.price)}
            </p>
            <p className="type-small text-muted mt-2">
              {servicePackage.priceNote ?? `Pengerjaan ${servicePackage.duration}`}
            </p>

            <p className="text-muted mt-5">{servicePackage.summary}</p>

            <dl className="border-line mt-6 grid gap-0 border-t">
              {servicePackage.features.map((feature) => (
                <div
                  key={feature.label}
                  className="border-line flex items-start justify-between gap-6 border-b py-3"
                >
                  <dt className="type-small text-muted">{feature.label}</dt>
                  <dd className="type-small text-right">
                    <FeatureCell value={feature.value} />
                  </dd>
                </div>
              ))}
            </dl>

            <ButtonLink
              href={whatsappLink(`${whatsappIntro} Paket ${servicePackage.name}.`)}
              variant={servicePackage.highlighted ? 'primary' : 'secondary'}
              className="mt-6 w-full"
            >
              Tanyakan paket ini
            </ButtonLink>
          </article>
        ))}
      </div>

      {/* Desktop: one comparison table */}
      <div className="scroll-x hidden lg:block">
        <table className="w-full min-w-3xl border-collapse text-left">
          <caption className="sr-only">Perbandingan paket dan harganya</caption>
          <thead>
            <tr>
              <th scope="col" className="w-[26%] pb-6 align-bottom">
                <Label as="span">Paket</Label>
              </th>
              {packages.map((servicePackage) => (
                <th
                  key={servicePackage.code}
                  scope="col"
                  className={cn(
                    'w-[24.6%] px-5 pt-5 pb-6 align-bottom',
                    servicePackage.highlighted && 'bg-accent-soft',
                  )}
                >
                  {servicePackage.highlighted ? (
                    <Label as="span" className="text-accent">
                      Paling sering dipilih
                    </Label>
                  ) : (
                    <Label as="span">{servicePackage.code}</Label>
                  )}
                  <span className="type-h3 mt-3 block">{servicePackage.name}</span>
                  <span className="type-small text-muted mt-1 block font-normal">
                    {servicePackage.audience}
                  </span>
                  <span className="numeric font-display mt-5 block text-[1.75rem] leading-none font-semibold">
                    {formatIDR(servicePackage.price)}
                  </span>
                  <span className="type-small text-muted mt-2 block font-normal">
                    {servicePackage.priceNote ?? `Pengerjaan ${servicePackage.duration}`}
                  </span>
                </th>
              ))}
            </tr>
          </thead>

          <tbody>
            {featureLabels.map((label) => (
              <tr key={label} className="border-line border-t">
                <th scope="row" className="type-small text-muted py-3.5 pr-6 font-normal">
                  {label}
                </th>
                {packages.map((servicePackage) => {
                  const feature = servicePackage.features.find((item) => item.label === label)
                  return (
                    <td
                      key={servicePackage.code}
                      className={cn('px-5 py-3.5', servicePackage.highlighted && 'bg-accent-soft')}
                    >
                      {feature ? <FeatureCell value={feature.value} /> : null}
                    </td>
                  )
                })}
              </tr>
            ))}

            <tr className="border-line border-t">
              <td />
              {packages.map((servicePackage) => (
                <td
                  key={servicePackage.code}
                  className={cn('px-5 pt-6 pb-7', servicePackage.highlighted && 'bg-accent-soft')}
                >
                  <ButtonLink
                    href={whatsappLink(`${whatsappIntro} Paket ${servicePackage.name}.`)}
                    variant={servicePackage.highlighted ? 'primary' : 'secondary'}
                    className="w-full"
                  >
                    Tanyakan paket ini
                  </ButtonLink>
                </td>
              ))}
            </tr>
          </tbody>
        </table>
      </div>
    </>
  )
}
