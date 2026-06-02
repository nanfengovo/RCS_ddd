import type { AppRoute } from '../router/types';

/**
 * 递归过滤路由树
 * @param routes 原始路由配置超集
 * @param userPolicies 当前用户拥有的策略码数组 (后端返回)
 */
export const filterRoutes = (routes: AppRoute[], userPolicies: string[]): AppRoute[] => {
  return routes
    .filter((route) => {
      // 1. 如果没有配置 policyCode，说明是基础公开路由，直接放行
      if (!route.policyCode) return true;
      
      // 2. 检查用户是否拥有该策略
      return userPolicies.includes(route.policyCode);
    })
    .map((route) => {
      // 3. 递归处理子菜单
      if (route.children) {
        return {
          ...route,
          children: filterRoutes(route.children, userPolicies),
        };
      }
      return route;
    })
    // 4. 清理掉没有子菜单但被错误定义为父节点的空壳
    .filter(route => !(route.children && route.children.length === 0));
};