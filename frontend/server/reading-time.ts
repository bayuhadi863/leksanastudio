import { readdirSync, readFileSync, statSync } from 'node:fs'
import path from 'node:path'

import { readingMinutes } from '../src/lib/format'

/**
 * Reading estimates, computed once at build time.
 *
 * The obvious alternative — importing each `.mdx` a second time with `?raw`
 * and counting words in the browser — works, but ships every article twice:
 * once compiled, once as source. Counting here keeps ~27 kB of duplicated
 * prose out of the bundle.
 *
 * Keys are `folder/slug`, matching what lib/content.ts derives from the glob.
 */

/** Frontmatter is metadata, not prose; it must not count towards the estimate. */
export const stripFrontmatter = (raw: string): string =>
  raw.replace(/^---\r?\n[\s\S]*?\r?\n---\r?\n?/, '')

export const buildReadingTimes = (root: string): Record<string, number> => {
  const contentRoot = path.join(root, 'src', 'content')
  const table: Record<string, number> = {}

  for (const folder of readdirSync(contentRoot)) {
    const directory = path.join(contentRoot, folder)
    if (!statSync(directory).isDirectory()) continue

    for (const file of readdirSync(directory)) {
      if (!file.endsWith('.mdx')) continue
      const raw = readFileSync(path.join(directory, file), 'utf8')
      table[`${folder}/${file.replace(/\.mdx$/, '')}`] = readingMinutes(stripFrontmatter(raw))
    }
  }

  return table
}
