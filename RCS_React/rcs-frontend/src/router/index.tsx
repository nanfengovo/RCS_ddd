import { createBrowserRouter, Navigate } from 'react-router-dom';
import Login from '../Pages/Login'; 
import MainLayout from '../layouts/MainLayout';
import AuthGuard from './AuthGuard';

// 🗺️ 实例化并导出路由器
export const router = createBrowserRouter([
  {
    path: '/login',
    element: <Login />,
  },
  {
    path: '/',
    // 主控台包裹在守卫中
    element: <AuthGuard><MainLayout /></AuthGuard>, 
    children: [
      { index: true, element: <Navigate to="/dashboard" replace /> },
      
      // 业务页面插槽
      { path: 'dashboard', element: <div>监控大盘组件加载处</div> },
      
      { path: '*', element: <div>404 不存在或无权限访问</div> }
    ]
  }
]);