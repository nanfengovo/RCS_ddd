import type { ReactNode } from 'react';

export interface AppRoute {
  path: string;            // 路由路径 (作为菜单的 key)
  name: string;            // 菜单显示的名称 (结合 i18n 使用)
  element?: ReactNode;     // 对应的 React 组件
  icon?: ReactNode;        // Ant Design 的图标
  policyCode?: string;     // 🚀 权限码：没有则公开，有则需校验
  hideInMenu?: boolean;    // 是否在左侧菜单中隐藏 (例如 404 页面或详情页)
  children?: AppRoute[];   // 子路由
}