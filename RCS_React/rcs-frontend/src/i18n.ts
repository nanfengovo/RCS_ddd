import i18n from 'i18next';
import { initReactI18next } from 'react-i18next';

// 1. 引入你刚才建好的字典
import zh from './locales/zh.json';
import en from './locales/en.json';

// 2. 初始化引擎
i18n
  .use(initReactI18next) // 绑定给 React
  .init({
    resources: {
      zh: { translation: zh },
      en: { translation: en }
    },
    lng: 'zh', // 默认语言设定为中文
    fallbackLng: 'en', // 如果找不到中文翻译，兜底用英文
    interpolation: {
      escapeValue: false // React 已经防范了 XSS，这里关掉即可
    }
  });

export default i18n;