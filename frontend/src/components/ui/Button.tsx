import type { ComponentPropsWithoutRef, ReactNode } from 'react'

import { AppLink } from '@/components/ui/AppLink'
import { cn } from '@/lib/cn'

export type ButtonVariant = 'primary' | 'secondary' | 'ghost'
export type ButtonSize = 'medium' | 'large'

const base =
  'inline-flex items-center justify-center gap-2 rounded-[var(--radius-control)] font-semibold ' +
  'transition-colors duration-150 ease-out select-none ' +
  'disabled:opacity-45 disabled:cursor-not-allowed'

const variants: Record<ButtonVariant, string> = {
  // Hover shifts colour only. Nothing lifts, nothing scales.
  primary:
    'bg-accent text-accent-fg hover:bg-[color-mix(in_oklab,var(--color-accent)_88%,var(--color-text))] active:translate-y-px',
  secondary:
    'border border-muted text-text hover:border-accent hover:text-accent active:translate-y-px',
  ghost: 'text-muted hover:text-accent',
}

/*
 * `medium` is written in tokens rather than fixed values so one button reads
 * correctly in both products: full size on the site, and trimmed to the tool
 * scale inside the panel, without a second variant to remember at every call.
 */
const sizes: Record<ButtonSize, string> = {
  medium:
    'min-h-[var(--control-height)] px-[var(--control-px)] text-[length:var(--control-text)]',
  large: 'min-h-13 px-8 text-[1.0625rem]',
}

type SharedProps = {
  readonly variant?: ButtonVariant
  readonly size?: ButtonSize
  readonly className?: string
  readonly children: ReactNode
}

type LinkButtonProps = SharedProps & {
  readonly href: string
  readonly external?: boolean
}

type ActionButtonProps = SharedProps &
  Omit<ComponentPropsWithoutRef<'button'>, 'className' | 'children'>

export function ButtonLink({
  href,
  external,
  variant = 'primary',
  size = 'medium',
  className,
  children,
}: LinkButtonProps) {
  return (
    <AppLink
      href={href}
      external={external}
      className={cn(base, variants[variant], sizes[size], className)}
    >
      {children}
    </AppLink>
  )
}

export function Button({
  variant = 'primary',
  size = 'medium',
  className,
  children,
  type = 'button',
  ...rest
}: ActionButtonProps) {
  return (
    <button type={type} className={cn(base, variants[variant], sizes[size], className)} {...rest}>
      {children}
    </button>
  )
}
