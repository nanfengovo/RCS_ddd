// src/utils/request.ts

import { client } from '../api/client/client.gen'; 
import { message } from 'antd';

// 🚀 核心黑科技：我们自己定义一个拦截引擎，完全接管底层请求
const customFetch = async (input: RequestInfo | URL, init?: RequestInit): Promise<Response> => {
    
    // 🛡️ 1. 请求拦截：自动塞入 Token
    const token = localStorage.getItem('accessToken');
    const headers = new Headers(init?.headers); // 继承原本的请求头
    if (token) {
        headers.set('Authorization', `Bearer ${token}`);
    }
    
    // 🚀 2. 执行真正的网络请求
    const response = await fetch(input, { ...init, headers });

    // 🛡️ 3. 响应拦截：全局捕获 401 过期
    if (response.status === 401) {
        localStorage.removeItem('accessToken');
        message.error('登录已过期，请重新登录');
        setTimeout(() => {
            window.location.href = '/login'; 
        }, 1000);
    }

    // 将响应原封不动交还给 SDK 去解析
    return response;
};

// 导出统一配置函数
export const setupClient = () => {
    client.setConfig({
        baseURL: 'http://localhost:5188', // 你之前处理双斜杠的逻辑也统一收口到这里！
        // @ts-ignore
        fetch: customFetch                // 🃏 掀桌子：直接替换它的底层请求引擎
    });
};