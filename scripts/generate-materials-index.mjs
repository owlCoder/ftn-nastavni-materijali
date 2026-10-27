import { execFileSync } from 'node:child_process'
import { mkdirSync, writeFileSync } from 'node:fs'

// The index points to the source commit, so file links and the list stay consistent.
const revision = execFileSync('git', ['rev-parse', 'HEAD'], { encoding: 'utf8' }).trim()
const output = execFileSync('git', ['ls-tree', '-r', '-t', '-l', '-z', revision], { maxBuffer: 64 * 1024 * 1024 })
const roots = new Set([
  'Elementi razvoja softvera (ERS)', 'Osnove informacione bezbednosti (OIB)',
  'Osnove distribuiranog programiranja (ODP)', 'Informaciona bezbednost infrastrukturnih sistema (IBIS)',
])
const tree = output.toString('utf8').split('\0').filter(Boolean).flatMap(line => {
  const match = line.match(/^\d+ (blob|tree) [a-f0-9]+\s+(-|\d+)\t([\s\S]+)$/)
  if (!match) throw new Error('Nije moguće pročitati Git stablo.')
  const [, type, size, path] = match
  if (!roots.has(path.split('/')[0].normalize('NFC'))) return []
  return [{ path, type, ...(type === 'blob' ? { size: Number(size) } : {}) }]
})
mkdirSync('.github', { recursive: true })
writeFileSync('.github/materials-index.json', JSON.stringify({ revision, tree }) + '\n')
console.log(`Indeks sadrži ${tree.length} stavki iz verzije ${revision.slice(0, 7)}.`)
