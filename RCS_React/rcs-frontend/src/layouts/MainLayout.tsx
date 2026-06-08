import React, { useState, useMemo } from 'react';
import { Layout, Menu, Button, ConfigProvider, Avatar, Dropdown, Breadcrumb } from 'antd';
import { useNavigate, useLocation, Outlet } from 'react-router-dom';
import { useTranslation } from 'react-i18next';
import { SunOutlined, MoonOutlined, MoreOutlined, UserOutlined, LogoutOutlined, SettingOutlined } from '@ant-design/icons';

import { superRoutes } from '../router/config';
import { filterRoutes } from '../utils/permission';
import type { AppRoute } from '../router/types';
import { useTheme } from '../contexts/ThemeContext'; 
import ThemeSettingDrawer from '../components/ThemeSettingDrawer'; 

const { Header, Sider, Content } = Layout;

const MainLayout: React.FC = () => {
  // 🚀 将状态移入组件内部
  const [settingOpen, setSettingOpen] = useState(false);
  
  const { t, i18n } = useTranslation();
  const { isDark, toggleTheme, colorPrimary } = useTheme(); 
  const navigate = useNavigate();
  const location = useLocation();
  const [collapsed, setCollapsed] = useState(false);
  
  const mockUserPolicies = ['agv:manage', 'agv:list:view']; 

  const authorizedRoutes = useMemo(() => {
    return filterRoutes(superRoutes, mockUserPolicies);
  }, [mockUserPolicies]);

  const getMenuItems = (routes: AppRoute[]): any[] => {
    return routes
      .filter(r => !r.hideInMenu)
      .map(route => ({
        key: route.path,
        icon: route.icon,
        label: t(route.name),
        children: route.children ? getMenuItems(route.children) : undefined,
      }));
  };

  const menuItems = getMenuItems(authorizedRoutes);

  const handleMenuClick = ({ key }: { key: string }) => {
    navigate(key);
  };

  const handleLogout = () => {
    localStorage.removeItem('accessToken');
    navigate('/login');
  };

  const siderBg = isDark ? '#000000' : '#ffffff';
  const siderBorder = isDark ? '#1f1f1f' : '#f0f0f0';
  const headerBg = isDark ? '#141414' : '#f5f5f5'; 
  const contentBg = isDark ? '#141414' : '#f5f5f5';

  const generateBreadcrumbs = () => {
    const pathSnippets = location.pathname.split('/').filter(i => i);

    const breadcrumbItems : { title: React.ReactNode }[] = [
        { title: <span className={isDark ? "text-gray-400" : "text-gray-500"}>{t('layout.rcsConsole')}</span> }
    ];

    if (pathSnippets[0] === 'dashboard') {
      breadcrumbItems.push({ title: t('menu.dashboard') });
    } else if (pathSnippets[0] === 'agv') {
      breadcrumbItems.push({ title: t('menu.agvManage') });
      if (pathSnippets[1] === 'list') {
        breadcrumbItems.push({ title: t('menu.agvList') });
      }
    }
    
    return breadcrumbItems;
  };

  return (
    <Layout style={{ minHeight: '100vh', background: contentBg }}>
      {/* 侧边栏 */}
      <Sider 
        collapsible 
        collapsed={collapsed} 
        onCollapse={(value) => setCollapsed(value)}
        theme={isDark ? "dark" : "light"}
        width={220}
        style={{
          background: siderBg,
          borderRight: `1px solid ${siderBorder}`,
          position: 'sticky',
          top: 0,
          left: 0,
          height: '100vh',
          zIndex: 10
        }}
        trigger={null} 
      >
        <div className="flex flex-col h-full">
          {/* Logo 区域 */}
          <div 
            className="flex items-center justify-center gap-3 cursor-pointer" 
            style={{ height: 64, borderBottom: `1px solid ${siderBorder}` }}
          >
            {/* 🚀 修复了 boxShadow 的反引号模板字符串 */}
            <div className="w-8 h-8 rounded-lg flex items-center justify-center text-white font-bold text-xl shadow-lg"
                style={{ background: colorPrimary, boxShadow: `0 4px 14px 0 ${colorPrimary}40` }}
            >
              R
            </div>
            {!collapsed && (
              <span className={`text-xl font-bold tracking-wide ${isDark ? 'text-white' : 'text-gray-800'}`}>
                RCS
              </span>
            )}
          </div>

          {/* 菜单区域 */}
          <div className="flex-1 overflow-y-auto py-4 custom-scrollbar">
            <ConfigProvider
              theme={{
                components: {
                  Menu: {
                    itemBg: 'transparent',
                    itemSelectedBg: isDark ? 'rgba(255, 255, 255, 0.08)' : '#eef2ff',
                    itemSelectedColor: colorPrimary, // 🚀 菜单选中文字颜色跟随主题色
                    itemActiveBg: isDark ? 'rgba(255, 255, 255, 0.04)' : '#f3f4f6',
                    itemHoverBg: isDark ? 'rgba(255, 255, 255, 0.08)' : '#f3f4f6',
                    itemHoverColor: isDark ? '#ffffff' : '#111827',
                    itemColor: isDark ? '#9ca3af' : '#4b5563',
                    activeBarBorderWidth: 3,
                    activeBarWidth: 3,
                  },
                },
              }}
            >
              <Menu
                theme={isDark ? "dark" : "light"}
                mode="inline"
                selectedKeys={[location.pathname]} 
                items={menuItems}
                onClick={handleMenuClick}
                style={{ borderRight: 'none', background: 'transparent' }}
              />
            </ConfigProvider>
          </div>

          {/* 底部用户信息 */}
          <div 
            className={`p-4 flex items-center gap-3 transition-colors ${isDark ? 'hover:bg-white/5' : 'hover:bg-gray-50'}`}
            style={{ borderTop: `1px solid ${siderBorder}` }}
          >
            <Avatar size={collapsed ? 32 : 40} icon={<UserOutlined />} style={{ backgroundColor: colorPrimary }} className="flex-shrink-0" />
            {!collapsed && (
              <div className="flex-1 flex items-center justify-between overflow-hidden">
                <div className="flex flex-col">
                  <span className={`text-sm font-medium ${isDark ? 'text-gray-200' : 'text-gray-800'} truncate`}>
                    admin
                  </span>
                  <span className={`text-xs ${isDark ? 'text-gray-500' : 'text-gray-400'} truncate`}>
                    {t('layout.superAdmin') || '超级管理员'}
                  </span>
                </div>
                <Dropdown
                  menu={{
                    items: [
                      { key: 'logout', danger: true, icon: <LogoutOutlined />, label: t('layout.logout') || '退出', onClick: handleLogout }
                    ]
                  }}
                  placement="topRight"
                  trigger={['click']}
                >
                  <Button type="text" icon={<MoreOutlined />} size="small" className={isDark ? "text-gray-400" : "text-gray-500"} />
                </Dropdown>
              </div>
            )}
          </div>
        </div>
      </Sider>
      
      {/* 右侧主区域 */}
      <Layout style={{ background: contentBg }}>
        <Header 
          style={{ 
            height: 64,
            padding: '0 24px', 
            background: headerBg,
            display: 'flex', 
            justifyContent: 'space-between', 
            alignItems: 'center',
            borderBottom: `1px solid ${siderBorder}`
          }}
        >
          {/* 左侧 面包屑导航 */}
          <div className="flex items-center">
            <Breadcrumb items={generateBreadcrumbs()} />
          </div>

          {/* 右侧 操作按钮 */}
          <div className="flex items-center gap-4">
            {/* 中英文切换 */}
            <Button 
              type="text"
              className={isDark ? "text-gray-300 font-bold hover:text-white" : "text-gray-600 font-bold hover:text-black"}
              onClick={() => {
                const nextLang = i18n.language === 'zh' ? 'en' : 'zh';
                i18n.changeLanguage(nextLang);
              }}
            >
              {i18n.language === 'zh' ? 'EN' : '中'}
            </Button>

            {/* 设置抽屉的唤起按钮 */}
            <Button 
              type="text"
              size="large"
              onClick={() => setSettingOpen(true)}
              icon={<SettingOutlined className={isDark ? "text-gray-300" : "text-gray-600"} />}
            />
          </div>
        </Header>
        
        {/* 内容区插槽 */}
        <Content 
          style={{ 
            margin: '16px', 
            padding: 24, 
            background: isDark ? '#141414' : '#ffffff',
            borderRadius: 12, 
            overflow: 'auto',
            border: `1px solid ${siderBorder}`,
            display: 'flex',
            flexDirection: 'column'
          }}
        >
            {/* 路由子页面注入处 */}
            <div className="flex-1">
              <Outlet /> 
            </div>
        </Content>
      </Layout>

      {/* 🚀 挂载主题设置抽屉 */}
      <ThemeSettingDrawer open={settingOpen} onClose={() => setSettingOpen(false)} />
    </Layout>
  );
};

export default MainLayout;