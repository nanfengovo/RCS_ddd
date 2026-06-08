import type { AppRoute } from './types';
import { DashboardOutlined, CarOutlined, SettingOutlined } from '@ant-design/icons';
// 假设你有一些业务页面组件
// import Dashboard from '@/pages/Dashboard';
// import AgvList from '@/pages/Agv/List';

export const superRoutes: AppRoute[] = [
  {
    path: '/dashboard',
    name: 'menu.dashboard', 
    icon: <DashboardOutlined />,
    // element: <Dashboard />,
    // 无 policyCode，登录了就能看
  },
  {
    path: '/system',
    name: 'menu.sysManage',
    icon: <SettingOutlined />,
    policyCode: 'sys:manage',
    // ... children
  }
];