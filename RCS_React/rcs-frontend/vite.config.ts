import { defineConfig } from 'vite';
import react from '@vitejs/plugin-react';
import tailwindcss from '@tailwindcss/vite'; // 🚀 1. 引入引擎

// https://vite.dev/config/
export default defineConfig({
  plugins: [
    react(), 
    tailwindcss()
  ],
  // 🚀 加上这段代理魔法：凡是 /api 开头的请求，统统转发给后端的 5188 端口
  server: {
    proxy: {
      '/api': {
        target: 'http://localhost:5188',
        changeOrigin: true
      }
    }
  }
})
