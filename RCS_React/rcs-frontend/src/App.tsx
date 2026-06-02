import React, { useMemo } from 'react';
import { RouterProvider } from 'react-router-dom';
import { ConfigProvider, theme } from 'antd';
import zhCN from 'antd/locale/zh_CN';
import enUS from 'antd/locale/en_US';
import { useTranslation } from 'react-i18next';
import { router } from './router'; // 引入你的路由实例
import { ThemeProvider, useTheme } from './contexts/ThemeContext';

// 🚀 内部组件：负责消费 Context，并将深浅色参数传递给 Ant Design 的配置引擎
const AppConfig: React.FC = () => {
  const { isDark } = useTheme();
  const { i18n } = useTranslation();

  const locale = useMemo(() => {
    return i18n.language === 'en' ? enUS : zhCN;
  }, [i18n.language]);

  return (
    <ConfigProvider 
      locale={locale}
      theme={{ 
        // 这里会自动根据全局 isDark 切换 Antd 的底层算法
        algorithm: isDark ? theme.darkAlgorithm : theme.defaultAlgorithm,
        token: {
          colorPrimary: '#4f46e5', // 科技蓝紫主色调
          borderRadius: 6, 
        }
      }}
    >
      <RouterProvider router={router} />
    </ConfigProvider>
  );
};

// 🚀 全局入口：极其纯粹，只负责挂载 Context
const App: React.FC = () => {
  return (
    <ThemeProvider>
      <AppConfig />
    </ThemeProvider>
  );
};

export default App;