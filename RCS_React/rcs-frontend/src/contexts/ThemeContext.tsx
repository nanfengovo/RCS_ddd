import React, { createContext, useContext, useState, useEffect } from 'react';

interface ThemeContextType {
  isDark: boolean;
  toggleTheme: () => void;
  colorPrimary: string;
  setColorPrimary: (color: string) => void;
  // 🚀 新增：风格套餐控制
  themeStyle: string; 
  setThemeStyle: (styleName: string) => void; 
}

const ThemeContext = createContext<ThemeContextType>({
  isDark: true,
  toggleTheme: () => {},
  colorPrimary: '#4f46e5',
  setColorPrimary: () => {},
  themeStyle: 'default',
  setThemeStyle: () => {},
});

export const useTheme = () => useContext(ThemeContext);

export const ThemeProvider: React.FC<{ children: React.ReactNode }> = ({ children }) => {
  const [isDark, setIsDark] = useState<boolean>(() => localStorage.getItem('rcs_isDark') !== 'false');
  const [colorPrimary, setPrimary] = useState<string>(() => localStorage.getItem('rcs_color') || '#4f46e5');
  
  // 🚀 新增：初始化风格套餐
  const [themeStyle, setStyle] = useState<string>(() => localStorage.getItem('rcs_style') || 'default');

  const toggleTheme = () => {
    setIsDark((prev) => {
      localStorage.setItem('rcs_isDark', String(!prev));
      return !prev;
    });
  };

  const setColorPrimary = (color: string) => {
    setPrimary(color);
    localStorage.setItem('rcs_color', color);
  };

  // 🚀 新增：保存风格套餐
  const setThemeStyle = (styleName: string) => {
    setStyle(styleName);
    localStorage.setItem('rcs_style', styleName);
  };

  useEffect(() => {
    if (isDark) document.documentElement.classList.add('dark');
    else document.documentElement.classList.remove('dark');
  }, [isDark]);

  return (
    <ThemeContext.Provider value={{ isDark, toggleTheme, colorPrimary, setColorPrimary, themeStyle, setThemeStyle }}>
      {children}
    </ThemeContext.Provider>
  );
};