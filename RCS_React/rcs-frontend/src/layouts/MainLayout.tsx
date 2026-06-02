import React, { useState, useMemo } from 'react';
import { Layout, Menu, Button, ConfigProvider, Avatar, Dropdown } from 'antd';
import { useNavigate, useLocation, Outlet } from 'react-router-dom';
import { useTranslation } from 'react-i18next';
import { SunOutlined, MoonOutlined, MoreOutlined, UserOutlined, LogoutOutlined } from '@ant-design/icons';

import { superRoutes } from '../router/config';
import { filterRoutes } from '../utils/permission';
import type { AppRoute } from '../router/types';
import { useTheme } from '../contexts/ThemeContext'; // 🚀 引入全局主题 Hook

const { Header, Sider, Content } = Layout;

const MainLayout: React.FC = () => {
  const { t, i18n } = useTranslation();
  const { isDark, toggleTheme } = useTheme(); // 🚀 接管全局深浅色状态
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

  // 左侧菜单与布局的颜色变量
  const siderBg = isDark ? '#000000' : '#ffffff';
  const siderBorder = isDark ? '#1f1f1f' : '#f0f0f0';
  const headerBg = isDark ? '#141414' : '#f5f5f5'; // 让 Header 融入背景
  const contentBg = isDark ? '#141414' : '#f5f5f5';

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
        trigger={null} // 隐藏默认 trigger，我们可以自己写或者直接不要
      >
        <div className="flex flex-col h-full">
          {/* Logo 区域 */}
          <div 
            className="flex items-center justify-center gap-3 cursor-pointer" 
            style={{ height: 64, borderBottom: `1px solid ${siderBorder}` }}
          >
            <div className="w-8 h-8 rounded-lg bg-indigo-600 flex items-center justify-center text-white font-bold text-xl shadow-lg shadow-indigo-500/30">
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
                    itemSelectedBg: isDark ? 'rgba(99, 102, 241, 0.15)' : '#eef2ff',
                    itemSelectedColor: '#6366f1',
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
            <Avatar size={collapsed ? 32 : 40} icon={<UserOutlined />} className="bg-indigo-500 flex-shrink-0" />
            {!collapsed && (
              <div className="flex-1 flex items-center justify-between overflow-hidden">
                <div className="flex flex-col">
                  <span className={`text-sm font-medium ${isDark ? 'text-gray-200' : 'text-gray-800'} truncate`}>
                    admin
                  </span>
                  <span className={`text-xs ${isDark ? 'text-gray-500' : 'text-gray-400'} truncate`}>
                    {t('layout.superAdmin')}
                  </span>
                </div>
                <Dropdown
                  menu={{
                    items: [
                      { key: 'logout', danger: true, icon: <LogoutOutlined />, label: t('layout.logout'), onClick: handleLogout }
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
        {/* 全局 Header (可根据需要放一些全局控件，如截图中的控件多在页面内) */}
        <Header 
          style={{ 
            height: 64,
            padding: '0 24px', 
            background: headerBg,
            display: 'flex', 
            justifyContent: 'flex-end', 
            alignItems: 'center',
            gap: '16px',
            borderBottom: `1px solid ${siderBorder}`
          }}
        >
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

          {/* 深浅色切换 */}
          <Button 
            type="text"
            size="large"
            onClick={toggleTheme} // 🚀 触发全局改变
            icon={isDark ? <SunOutlined className='text-yellow-400' /> : <MoonOutlined className='text-gray-600' />}
          />
        </Header>
        
        {/* 内容区插槽 */}
        <Content 
          style={{ 
            margin: '16px', 
            padding: 24, 
            background: isDark ? '#141414' : '#ffffff', // 实际页面内容的卡片背景
            borderRadius: 12, 
            overflow: 'auto',
            border: `1px solid ${siderBorder}`
          }}
        >
          <Outlet /> 
        </Content>
      </Layout>
    </Layout>
  );
};

export default MainLayout;