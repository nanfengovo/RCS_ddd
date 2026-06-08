import React from 'react';
import { Drawer, Divider, Tooltip, Space } from 'antd';
import { useTheme } from '../contexts/ThemeContext';
import { 
  CheckOutlined, 
  FormatPainterOutlined,
  SunOutlined,
  MoonOutlined
} from '@ant-design/icons';

interface Props {
  open: boolean;
  onClose: () => void;
}

const themeColors = [
  { name: '靛青', color: '#4f46e5' },
  { name: '极客蓝', color: '#1677ff' },
  { name: '翠绿', color: '#10b981' },
  { name: '琥珀', color: '#f59e0b' },
  { name: '粉红', color: '#ec4899' },
  { name: '青蓝', color: '#06b6d4' },
  { name: '紫罗兰', color: '#722ed1' },
  { name: '火山红', color: '#f5222d' }
];

const stylesList = [
  {
    key: 'default',
    title: '默认风格',
    desc: '适中圆角，柔和阴影，标准后台体验'
  },
  {
    key: 'mui',
    title: '类 MUI 风格',
    desc: '经典 Material Design，拟物阴影'
  },
  {
    key: 'shadcn',
    title: '类 shadcn 风格',
    desc: '极简线框，硬朗微圆角，大留白'
  },
  {
    key: 'cartoon',
    title: '卡通风格',
    desc: '粗线条描边，大圆角，沉重实心阴影'
  },
  {
    key: 'illustration',
    title: '插画风格',
    desc: '清新扁平插画风，大面积柔和阴影'
  },
  {
    key: 'bootstrap',
    title: '类 Bootstrap 拟物化风格',
    desc: '去除扁平，找回经典的按钮触感'
  },
  {
    key: 'glass',
    title: '玻璃风格',
    desc: '半透明容器，毛玻璃光影质感'
  },
  {
    key: 'compact',
    title: '紧凑数据风',
    desc: '更小字体与间距，一屏容纳更多内容'
  }
];

const ThemeSettingDrawer: React.FC<Props> = ({ open, onClose }) => {
  const { isDark, toggleTheme, colorPrimary, setColorPrimary, themeStyle, setThemeStyle } = useTheme();

  return (
    <Drawer 
      title={
        <Space className="text-base font-semibold">
          <FormatPainterOutlined /> 外观定制
        </Space>
      } 
      placement="right" 
      onClose={onClose} 
      open={open}
      width={320}
      styles={{ body: { paddingBottom: 80 } }}
    >
      {/* 1. 整体风格 (明暗) */}
      <div className="mb-8">
        <div className="text-sm font-semibold mb-4 text-gray-800 dark:text-gray-200">整体风格设置</div>
        <div className="flex gap-4">
          <div 
            onClick={() => isDark && toggleTheme()}
            className="flex-1 cursor-pointer group"
          >
            <div 
              className="h-14 rounded-lg bg-gray-50 flex items-center justify-center border-2 transition-all duration-300"
              style={{ 
                borderColor: !isDark ? colorPrimary : 'transparent',
                backgroundColor: !isDark ? `${colorPrimary}10` : undefined
              }}
            >
              <SunOutlined 
                className="text-xl transition-colors duration-300" 
                style={{ color: !isDark ? colorPrimary : '#9ca3af' }} 
              />
            </div>
            <div className="text-center mt-2 text-sm font-medium dark:text-gray-400">亮色模式</div>
          </div>
          
          <div 
            onClick={() => !isDark && toggleTheme()}
            className="flex-1 cursor-pointer group"
          >
            <div 
              className="h-14 rounded-lg bg-gray-900 flex items-center justify-center border-2 transition-all duration-300"
              style={{ 
                borderColor: isDark ? colorPrimary : 'transparent',
                backgroundColor: isDark ? `${colorPrimary}20` : undefined
              }}
            >
              <MoonOutlined 
                className="text-xl transition-colors duration-300" 
                style={{ color: isDark ? colorPrimary : '#6b7280' }} 
              />
            </div>
            <div className="text-center mt-2 text-sm font-medium dark:text-gray-400">暗黑模式</div>
          </div>
        </div>
      </div>

      <Divider />

      {/* 2. 主题色 */}
      <div className="mb-8">
        <div className="text-sm font-semibold mb-4 text-gray-800 dark:text-gray-200">系统主题色</div>
        <div className="flex flex-wrap gap-4">
          {themeColors.map(item => (
            <Tooltip key={item.color} title={item.name} placement="top">
              <div
                onClick={() => setColorPrimary(item.color)}
                className="w-7 h-7 rounded-full flex items-center justify-center cursor-pointer transition-all duration-300 hover:scale-110"
                style={{ 
                  backgroundColor: item.color,
                  boxShadow: colorPrimary === item.color 
                    ? `0 0 0 2px ${isDark ? '#141414' : '#fff'}, 0 0 0 4px ${item.color}` 
                    : 'none'
                }}
              >
                {colorPrimary === item.color && (
                  <CheckOutlined style={{ color: '#fff', fontSize: 12, fontWeight: 'bold' }} />
                )}
              </div>
            </Tooltip>
          ))}
        </div>
      </div>

      <Divider />

      {/* 3. UI 风格套餐 */}
      <div>
        <div className="text-sm font-semibold mb-4 text-gray-800 dark:text-gray-200">组件风格定制</div>
        <div className="flex flex-col gap-4">
          {stylesList.map(style => {
            const isSelected = themeStyle === style.key;
            return (
              <div 
                key={style.key}
                onClick={() => setThemeStyle(style.key)}
                className={`
                  p-4 rounded-lg cursor-pointer transition-all duration-300 border-2
                  ${!isSelected ? 'border-transparent bg-gray-50 dark:bg-gray-800/50 hover:bg-gray-100 dark:hover:bg-gray-800' : ''}
                `}
                style={isSelected ? {
                  borderColor: colorPrimary,
                  backgroundColor: isDark ? `${colorPrimary}15` : `${colorPrimary}0A`
                } : {}}
              >
                <div className="flex justify-between items-center mb-1">
                  <span 
                    className="font-medium text-sm transition-colors" 
                    style={isSelected ? { color: colorPrimary } : {}}
                  >
                    {style.title}
                  </span>
                  {isSelected && <CheckOutlined style={{ color: colorPrimary }} />}
                </div>
                <div 
                  className={`text-xs transition-colors ${!isSelected ? 'text-gray-500 dark:text-gray-400' : ''}`}
                  style={isSelected ? { color: colorPrimary, opacity: 0.8 } : {}}
                >
                  {style.desc}
                </div>
              </div>
            );
          })}
        </div>
      </div>
    </Drawer>
  );
};

export default ThemeSettingDrawer;