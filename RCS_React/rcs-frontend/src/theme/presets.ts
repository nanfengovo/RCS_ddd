import type { ThemeConfig }  from 'antd'

// 1. 默认风格（适中的圆角，柔和的阴影）
export const preseDefault: ThemeConfig ={
    token:{
        borderRadius: 6,
        wireframe: false,
    },
};

// 2. 类 shadcn 风格（更小的圆角，几乎没有阴影，清晰线框）
export const preseShadcn: ThemeConfig = {
    token:{
        borderRadius: 4,
        wireframe: true,
    },
    components:{
        Card:{
            boxShadow: 'none',
            boxShadowTertiary:'none',
        },
        Button:{
            controlHeight: 36,
        }
    }
};

// 3. 紧凑数据风
export const presetCompact: ThemeConfig = {
  token: {
    borderRadius: 2,
    controlHeight: 28,
    fontSize: 12,
  }
};

// 4. 类 MUI 风格 (Material Design)
export const presetMUI: ThemeConfig = {
    token: {
        borderRadius: 4,
        wireframe: false,
    },
    components: {
        Button: {
            boxShadow: '0px 3px 1px -2px rgba(0,0,0,0.2), 0px 2px 2px 0px rgba(0,0,0,0.14), 0px 1px 5px 0px rgba(0,0,0,0.12)',
        },
        Card: {
            boxShadow: '0px 2px 1px -1px rgba(0,0,0,0.2), 0px 1px 1px 0px rgba(0,0,0,0.14), 0px 1px 3px 0px rgba(0,0,0,0.12)',
        }
    }
};

// 5. 卡通风格 (加粗描边，大圆角，重阴影)
export const presetCartoon: ThemeConfig = {
    token: {
        borderRadius: 12,
        lineWidth: 2,
    },
    components: {
        Button: {
            boxShadow: '0 4px 0 rgba(0,0,0,0.2)',
        },
        Card: {
            boxShadow: '4px 4px 0 rgba(0,0,0,1)',
            borderRadius: 16,
        }
    }
};

// 6. 插画风格
export const presetIllustration: ThemeConfig = {
    token: {
        borderRadius: 8,
    },
    components: {
        Card: {
            boxShadow: '0 8px 24px rgba(0,0,0,0.05)',
        }
    }
};

// 7. 类 Bootstrap 拟物化风格
export const presetBootstrap: ThemeConfig = {
    token: {
        borderRadius: 4,
        wireframe: true,
    },
    components: {
        Button: {
            primaryShadow: 'none',
        }
    }
};

// 8. 玻璃风格 (结合了透明度和毛玻璃效果的模拟)
export const presetGlass: ThemeConfig = {
    token: {
        borderRadius: 12,
        colorBgContainer: 'rgba(255, 255, 255, 0.4)',
        colorBgElevated: 'rgba(255, 255, 255, 0.6)',
        colorBgLayout: 'transparent',
    },
    components: {
        Card: {
            boxShadow: '0 4px 30px rgba(0, 0, 0, 0.1)',
        }
    }
};

// 导出一个字典，方便后续通过 key 查找
export const themePresets: Record<string, ThemeConfig> = {
  default: preseDefault,
  shadcn: preseShadcn,
  compact: presetCompact,
  mui: presetMUI,
  cartoon: presetCartoon,
  illustration: presetIllustration,
  bootstrap: presetBootstrap,
  glass: presetGlass,
};