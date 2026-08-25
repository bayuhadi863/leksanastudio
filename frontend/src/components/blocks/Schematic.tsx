import { cn } from '@/lib/cn'

export type SchematicVariant = 'system' | 'website' | 'catalog'

type Props = {
  readonly variant: SchematicVariant
  readonly className?: string
  readonly title: string
}

/**
 * Figures on this site are schematics, not photographs.
 *
 * Two reasons, in order of importance: a technical document illustrates itself
 * with diagrams, and a screenshot of someone else's private admin panel cannot
 * be published without redacting the very thing that makes it interesting.
 *
 * Drawn with tokens and `currentColor`, so the figure is legible in both
 * colour schemes and costs no network request.
 */
export function Schematic({ variant, className, title }: Props) {
  return (
    <svg
      viewBox="0 0 640 400"
      role="img"
      aria-label={title}
      className={cn('bg-surface text-line block w-full', className)}
      preserveAspectRatio="xMidYMid slice"
    >
      <rect width="640" height="400" fill="var(--color-surface)" />
      {variant === 'system' ? <SystemFigure /> : null}
      {variant === 'website' ? <WebsiteFigure /> : null}
      {variant === 'catalog' ? <CatalogFigure /> : null}
      <rect x="0.5" y="0.5" width="639" height="399" fill="none" stroke="var(--color-line)" />
    </svg>
  )
}

/* Shared primitives ------------------------------------------------------- */

const line = 'var(--color-line)'
const muted = 'var(--color-muted)'
const accent = 'var(--color-accent)'

type BarProps = {
  x: number
  y: number
  w: number
  h?: number
  fill?: string
  opacity?: number
}

const Bar = ({ x, y, w, h = 6, fill = muted, opacity = 0.45 }: BarProps) => (
  <rect x={x} y={y} width={w} height={h} rx="1" fill={fill} opacity={opacity} />
)

/* Variant: multi-role information system --------------------------------- */

const SystemFigure = () => {
  const rows = [0, 1, 2, 3, 4, 5]

  return (
    <g>
      {/* sidebar */}
      <rect x="0" y="0" width="148" height="400" fill="var(--color-bg)" />
      <line x1="148" y1="0" x2="148" y2="400" stroke={line} />
      <Bar x={24} y={28} w={52} h={8} fill={accent} opacity={0.9} />
      {[72, 100, 128, 156, 184, 212, 240, 268].map((y, index) => (
        <g key={y}>
          {index === 2 ? (
            <rect x="12" y={y - 8} width="124" height="24" rx="2" fill={accent} opacity="0.12" />
          ) : null}
          <Bar
            x={24}
            y={y}
            w={index === 2 ? 76 : 60 + ((index * 13) % 34)}
            opacity={index === 2 ? 0.8 : 0.35}
          />
        </g>
      ))}
      <line x1="12" y1="300" x2="136" y2="300" stroke={line} />
      <Bar x={24} y={318} w={44} opacity={0.3} />

      {/* topbar */}
      <line x1="148" y1="56" x2="640" y2="56" stroke={line} />
      <Bar x={176} y={26} w={96} h={8} opacity={0.65} />
      {/* active-role chip — the decision that made this system worth writing about */}
      <rect x="516" y="20" width="96" height="20" rx="2" fill={accent} opacity="0.12" />
      <Bar x={528} y={27} w={56} h={6} fill={accent} opacity={0.85} />

      {/* table header */}
      <Bar x={176} y={84} w={40} h={5} opacity={0.5} />
      <Bar x={276} y={84} w={52} h={5} opacity={0.5} />
      <Bar x={400} y={84} w={44} h={5} opacity={0.5} />
      <Bar x={520} y={84} w={36} h={5} opacity={0.5} />
      <line x1="176" y1="100" x2="612" y2="100" stroke={line} />

      {/* rows with status pills */}
      {rows.map((row) => {
        const y = 120 + row * 42
        const isPending = row % 3 === 0
        return (
          <g key={row}>
            <Bar x={176} y={y} w={64 + ((row * 17) % 28)} />
            <Bar x={276} y={y} w={88 - ((row * 11) % 24)} opacity={0.3} />
            <rect
              x="400"
              y={y - 6}
              width={isPending ? 68 : 58}
              height="18"
              rx="2"
              fill={isPending ? accent : muted}
              opacity={isPending ? 0.16 : 0.12}
            />
            <Bar
              x={410}
              y={y}
              w={isPending ? 40 : 32}
              h={5}
              fill={isPending ? accent : muted}
              opacity={isPending ? 0.8 : 0.5}
            />
            <Bar x={520} y={y} w={56} opacity={0.25} />
            <line x1="176" y1={y + 26} x2="612" y2={y + 26} stroke={line} />
          </g>
        )
      })}
    </g>
  )
}

/* Variant: local business website ---------------------------------------- */

const WebsiteFigure = () => (
  <g>
    {/* header */}
    <line x1="0" y1="52" x2="640" y2="52" stroke={line} />
    <Bar x={40} y={22} w={64} h={9} fill={accent} opacity={0.85} />
    {[300, 360, 420].map((x) => (
      <Bar key={x} x={x} y={24} w={40} h={5} opacity={0.4} />
    ))}
    <rect x="512" y="16" width="88" height="22" rx="2" fill={accent} opacity="0.9" />

    {/* hero */}
    <Bar x={40} y={104} w={300} h={16} opacity={0.7} />
    <Bar x={40} y={132} w={244} h={16} opacity={0.7} />
    <Bar x={40} y={172} w={200} h={7} opacity={0.32} />
    <Bar x={40} y={188} w={168} h={7} opacity={0.32} />
    <rect x="40" y="216" width="128" height="30" rx="2" fill={accent} opacity="0.9" />
    <rect x="180" y="216" width="112" height="30" rx="2" fill="none" stroke={muted} opacity="0.5" />

    {/* proof strip */}
    <line x1="40" y1="284" x2="600" y2="284" stroke={line} />
    {[0, 1, 2].map((index) => (
      <g key={index}>
        <Bar x={40 + index * 190} y={308} w={44} h={14} opacity={0.6} />
        <Bar x={40 + index * 190} y={334} w={72} h={5} opacity={0.3} />
      </g>
    ))}

    {/* service cards */}
    <rect x="392" y="96" width="208" height="150" rx="2" fill="var(--color-bg)" stroke={line} />
    <Bar x={412} y={120} w={56} h={5} fill={accent} opacity={0.8} />
    <Bar x={412} y={140} w={140} h={9} opacity={0.55} />
    <Bar x={412} y={162} w={116} h={6} opacity={0.28} />
    <Bar x={412} y={178} w={132} h={6} opacity={0.28} />
    <line x1="412" y1="204" x2="580" y2="204" stroke={line} />
    <Bar x={412} y={220} w={84} h={6} fill={accent} opacity={0.75} />
  </g>
)

/* Variant: corporate site with a searchable catalogue --------------------- */

const CatalogFigure = () => (
  <g>
    <line x1="0" y1="52" x2="640" y2="52" stroke={line} />
    <Bar x={40} y={22} w={72} h={9} opacity={0.7} />
    {[340, 400, 460].map((x) => (
      <Bar key={x} x={x} y={24} w={40} h={5} opacity={0.4} />
    ))}

    {/* filter rail */}
    <rect x="40" y="88" width="150" height="272" rx="2" fill="var(--color-bg)" stroke={line} />
    <Bar x={60} y={112} w={52} h={6} fill={accent} opacity={0.8} />
    {[140, 164, 188, 220, 244, 268, 300].map((y, index) => (
      <g key={y}>
        <rect
          x="60"
          y={y - 6}
          width="12"
          height="12"
          rx="1"
          fill={index === 1 || index === 4 ? accent : 'none'}
          opacity={index === 1 || index === 4 ? 0.8 : 1}
          stroke={index === 1 || index === 4 ? 'none' : line}
        />
        <Bar x={82} y={y - 3} w={64 - ((index * 9) % 26)} h={6} opacity={0.35} />
      </g>
    ))}

    {/* search + result grid */}
    <rect x="214" y="88" width="386" height="34" rx="2" fill="var(--color-bg)" stroke={line} />
    <Bar x={234} y={102} w={110} h={6} opacity={0.3} />

    {[0, 1, 2, 3].map((index) => {
      const x = 214 + (index % 2) * 198
      const y = 142 + Math.floor(index / 2) * 116
      return (
        <g key={index}>
          <rect x={x} y={y} width="188" height="100" rx="2" fill="var(--color-bg)" stroke={line} />
          <Bar x={x + 18} y={y + 20} w={44} h={5} fill={accent} opacity={0.75} />
          <Bar x={x + 18} y={y + 38} w={132} h={8} opacity={0.55} />
          <Bar x={x + 18} y={y + 58} w={104} h={5} opacity={0.28} />
          <Bar x={x + 18} y={y + 72} w={120} h={5} opacity={0.28} />
        </g>
      )
    })}
  </g>
)
