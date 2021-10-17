import path from 'path';
import { defineConfig } from 'vite';
import { svelte } from '@sveltejs/vite-plugin-svelte';
import { readFileSync } from 'fs';

const cwd = process.cwd();
const tsconfig = JSON.parse(
  readFileSync(path.resolve(cwd, 'tsconfig.json'), 'utf-8')
    .replace(/\\"|"(?:\\"|[^"])*"|(\/\/.*|\/\*[\s\S]*?\*\/)/g, (m, g) => (g ? '' : m))
    .replace(/,(?!\s*?[{["\w])|(?<=[{[]\s*?),/g, '')
);

export default defineConfig(({ mode }) => {
  const production = mode === 'production';

  return {
    publicDir: 'static',
    resolve: {
      alias: Object.fromEntries(
        Object.entries(tsconfig.compilerOptions.paths)
          .filter(([k, _]) => !k.endsWith('*'))
          .map(([k, v]) => [k, path.resolve(cwd, v[0])])
      ),
    },
    plugins: [svelte()],
    build: {
      minify: production,
    },
  };
});
