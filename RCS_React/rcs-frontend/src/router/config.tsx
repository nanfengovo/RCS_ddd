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
    path: '/agv',
    name: 'menu.agvManage',
    icon: <CarOutlined />,
    policyCode: 'agv:manage', // 🚀 主菜单权限
    children: [
      {
        path: '/agv/list',
        name: 'menu.agvList',
        policyCode: 'agv:list:view', // 子菜单精确权限
        // element: <AgvList />,
      },
      {
        path: '/agv/detail/:id', // 动态路由
        name: 'menu.agvDetail',
        hideInMenu: true, // 🚀 详情页不需要出现在侧边栏菜单中
        policyCode: 'agv:detail:view',
      }
    ],
  },
  {
    path: '/system',
    name: 'menu.sysManage',
    icon: <SettingOutlined />,
    policyCode: 'sys:manage',
    // ... children
  }
];