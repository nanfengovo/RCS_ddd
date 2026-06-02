import React, { createContext, useContext, useState, useEffect } from 'react';

// 1. 定义数据契约
interface ThemeContextType {
  isDark: boolean;
  toggleTheme: () => void;
}

// 2. 创建 Context (提供默认值兜底)
const ThemeContext = createContext<ThemeContextType>({
  isDark: true,
  toggleTheme: () => {},
});

// 3. 导出自定义 Hook 供业务组件随时调用
export const useTheme = () => useContext(ThemeContext);

// 4. 导出 Provider 组件包裹全局
export const ThemeProvider: React.FC<{ children: React.ReactNode }> = ({ children }) => {
  // 尝试从本地存储读取上次的主题偏好，默认深色
  const [isDark, setIsDark] = useState<boolean>(() => {
    const saved = localStorage.getItem('rcs_theme_isDark');
    return saved !== null ? saved === 'true' : true;
  });

  // 每次切换时，不仅改状态，还存入本地，并给 HTML 根节点打上 Tailwind 的 dark 标签
  const toggleTheme = () => {
    setIsDark((prev) => {
      const nextTheme = !prev;
      localStorage.setItem('rcs_theme_isDark', String(nextTheme));
      return nextTheme;
    });
  };

  useEffect(() => {
    if (isDark) {
      document.documentElement.classList.add('dark');
    } else {
      document.documentElement.classList.remove('dark');
    }
  }, [isDark]);

  return (
    <ThemeContext.Provider value={{ isDark, toggleTheme }}>
      {children}
    </ThemeContext.Provider>
  );
};