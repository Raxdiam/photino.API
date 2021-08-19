import path from 'path';
import { defineConfig } from 'vite';
import { svelte } from '@sveltejs/vite-plugin-svelte';
import { viteSingleFile } from 'vite-plugin-singlefile';

const cwd = process.cwd();

export default defineConfig(({ mode }) => {
  const production = mode === 'production';

  return {
    publicDir: 'static',
    resolve: {
      alias: {
        $lib: path.resolve(cwd, './src/lib'),
        $components: path.resolve(cwd, './src/components'),
        $assets: path.resolve(cwd, './src/assets'),
        $stores: path.resolve(cwd, './src/stores'),
      },
    },
    plugins: [svelte(), viteSingleFile()],
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
