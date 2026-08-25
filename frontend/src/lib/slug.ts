/** Address helpers shared by every slug field in the panel. */

/**
 * Turns a title into an address, the same way the server does.
 *
 * Kept identical to `SlugHelper` on the API deliberately: if the two disagreed,
 * the editor would show one address and the site would serve another.
 */
export const slugify = (value: string, maxLength = 200): string => {
  const slug = value
    .normalize('NFD')
    .replace(/[̀-ͯ]/g, '')
    .toLowerCase()
    .replace(/[^a-z0-9]+/g, '-')
    .replace(/^-+|-+$/g, '')

  return slug.length > maxLength ? slug.slice(0, maxLength).replace(/-+$/, '') : slug
}

/**
 * Keeps a slug in step with a title until someone edits the slug by hand.
 *
 * Auto-deriving forever would overwrite a deliberate choice; never deriving
 * makes every new entry two jobs. Following until touched is the compromise
 * that matches what people expect.
 */
export const deriveSlug = (title: string, current: string, touched: boolean): string =>
  touched ? current : slugify(title)
