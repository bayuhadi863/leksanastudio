import { useEffect, useState } from 'react'

import { ButtonLink } from '@/components/ui/Button'
import { site } from '@/config/site'
import { cn } from '@/lib/cn'
import { whatsappLink } from '@/lib/whatsapp'

type Props = {
  /**
   * The opening message differs per page on purpose — it tells us where the
   * prospect came from without having to ask.
   */
  readonly message: string
}

/**
 * Phone-only sticky bar. Most prospects read on a phone and will not scroll
 * back up to find the call to action.
 */
export function WhatsAppBar({ message }: Props) {
  const [visible, setVisible] = useState(false)

  useEffect(() => {
    const onScroll = () => setVisible(window.scrollY > 400)
    onScroll()
    window.addEventListener('scroll', onScroll, { passive: true })
    return () => window.removeEventListener('scroll', onScroll)
  }, [])

  return (
    <div
      className={cn(
        'floating border-line bg-surface fixed inset-x-0 bottom-0 z-40 border-t lg:hidden',
        'transition-transform duration-200 ease-out',
        'pb-[env(safe-area-inset-bottom)]',
        visible ? 'translate-y-0' : 'pointer-events-none translate-y-full',
      )}
      aria-hidden={!visible}
    >
      <div className="shell flex items-center gap-4 py-3">
        <p className="type-small text-muted hidden flex-1 sm:block">
          Dibalas dalam {site.promises.replyWithinHours} jam.
        </p>
        <ButtonLink href={whatsappLink(message)} size="large" className="flex-1 sm:flex-none">
          Diskusi lewat WhatsApp
        </ButtonLink>
      </div>
    </div>
  )
}
