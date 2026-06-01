import { defineConfig } from '@hey-api/openapi-ts';

export default defineConfig({
  // 关键点：这里必须指向那个 .json 后缀的文档源
  input: 'http://localhost:5188/openapi/v1.json', 
  
  output: {
    path: 'src/api/client',
    format: 'prettier',
  },
  
  plugins: [
    '@hey-api/client-fetch', 
    '@hey-api/typescript',
    '@hey-api/sdk'
  ],
});