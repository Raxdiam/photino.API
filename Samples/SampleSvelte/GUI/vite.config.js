import path from 'path';
import { defineConfig } from 'vite';
import { svelte } from '@sveltejs/vite-plugin-svelte';
import { viteSingleFile } from 'vite-plugin-singlefile';
import { readFileSync, rmSync } from 'fs';

const cwd = process.cwd();
const tsconfig = JSON.parse(
  readFileSync(path.resolve(cwd, 'tsconfig.json'), 'utf-8')
    .replace(/\\"|"(?:\\"|[^"])*"|(\/\/.*|\/\*[\s\S]*?\*\/)/g, (m, g) => (g ? '' : m))
    .replace(/,(?!\s*?[{["\w])|(?<=[{[]\s*?),/g, '')
);

function cleanLeftovers() {
  return {
    name: 'clean-leftovers',

    closeBundle() {
      rmSync('../wwwroot/assets', { recursive: true, force: true });
    },
  };
}

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
    plugins: [svelte(), viteSingleFile(), cleanLeftovers()],
    build: {
      minify: production,
      outDir: '../wwwroot',
      cssCodeSplit: false,
      assetsInlineLimit: 100000000,
      rollupOptions: {
        output: {
          format: 'iife',
          inlineDynamicImports: true,
        },
      },
    },
  };
});
