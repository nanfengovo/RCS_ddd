import React from 'react';
import { RouterProvider } from 'react-router-dom';
import { ConfigProvider, theme } from 'antd';
import zhCN from 'antd/locale/zh_CN';
import { router } from './router'; 
import { ThemeProvider, useTheme } from './contexts/ThemeContext';
import { themePresets } from './theme/presets'; // 🚀 引入你的军火库

const AppConfig: React.FC = () => {
  // 🚀 把 themeStyle 也解构出来
  const { isDark, colorPrimary, themeStyle } = useTheme(); 
  
  // 🚀 根据用户选择，抽出对应的预设配置
  const baseStyle = themePresets[themeStyle] || themePresets.default;

  return (
    <ConfigProvider 
      locale={zhCN}
      theme={{ 
        // 1. 展开预设套餐里的所有配置 (比如 borderRadius, components 重写)
        ...baseStyle, 
        
        // 2. 强行注入深浅色算法
        algorithm: isDark ? theme.darkAlgorithm : theme.defaultAlgorithm,
        
        // 3. 强行注入用户选中的主色调 (这会覆盖套餐里的 token，优先级最高)
        token: {
          ...baseStyle.token,
          colorPrimary: colorPrimary, 
        }
      }}
    >
      <RouterProvider router={router} />
    </ConfigProvider>
  );
};

const App: React.FC = () => {
  return (
    <ThemeProvider>
      <AppConfig />
    </ThemeProvider>
  );
};

export default App;