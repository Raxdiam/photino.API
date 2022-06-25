import path from 'path';
import { defineConfig } from 'vite';

export default defineConfig(({ mode }) => {
  const prod = mode == 'productoin';

  return {
    build: {
      lib: {
        entry: path.resolve(__dirname, 'src/index.ts'),
        name: 'PhotinoAPI',
        fileName: (format) => `${format}/photino.js`,
      },
    },
  };
});
