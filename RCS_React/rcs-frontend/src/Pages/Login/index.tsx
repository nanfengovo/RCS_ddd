//1，引入 React 的核心钩子 useState
import {useState} from 'react';
//2. 引入Ant Design 的组件和主题引擎
import { ConfigProvider, theme, Button,Card,Form,Input,Flex,Checkbox} from 'antd';
//3， 引入小太阳和小月亮图标
import { SunOutlined, MoonOutlined,LockOutlined, UserOutlined} from '@ant-design/icons';
import { useTranslation } from 'react-i18next';
import AgvAnimation from './components/AgvAnimation';

    // 定义一个布尔值状态： isDark 默认是true(深色模式)
    //setIsDark 是唯一能修改 isDark 的遥控器

const Login = () => {
    const [isDark, setIsDark] = useState(true);
    const { t, i18n } = useTranslation();
    const onFinish = (values: any) => {
    console.log('Received values of form: ', values);
  };

  return (
  <ConfigProvider theme ={{ algorithm: isDark ? theme.darkAlgorithm : theme.defaultAlgorithm,
        // 🚀 2. 新增 token：强制全局的主题色变成参考图里的科技蓝紫！
        token: {
          colorPrimary: '#4f46e5', // 这是 Tailwind 的 indigo-600 颜色，非常契合你的设计图
          borderRadius: 6, // 顺手把组件的圆角改小一点，显得更硬朗现代
        }
  }}>
    <div className={`min-h-screen flex relative transition-colors duration-300 ${isDark ? 'bg-[#1C1C1C] text-white' : 'bg-gray-50 text-slate-900'}`}>
      {/* 🚀 注入灵魂：纯 CSS 绘制的科幻网格背景！
            原理：利用两个透明渐变色画出 1px 的横线和竖线，并设置 24px 的网格大小。
            pointer-events-none 确保这个网格只是装饰，不会阻挡鼠标点击！ */}
        <div className="absolute inset-0 z-0 pointer-events-none bg-[linear-gradient(to_right,#80808012_1px,transparent_1px),linear-gradient(to_bottom,#80808012_1px,transparent_1px)] bg-[size:24px_24px]"></div>
        <div className='absolute top-6 right-6 z-50'>
            <Button 
                type="text"
                className="text-gray-400 font-bold"
                onClick={() => {
                // 核心逻辑：如果当前是中文，就切英文；反之亦然
                const nextLang = i18n.language === 'zh' ? 'en' : 'zh';
                i18n.changeLanguage(nextLang);
                }}
            >
                {i18n.language === 'zh' ? 'EN' : '中'}
          </Button>
            <Button 
            type="text"
            size="large"
            onClick={() => setIsDark(!isDark)}
            icon={isDark? <SunOutlined className='text-yellow-400' /> : <MoonOutlined/>}
            />
        </div>

        {/* 左侧：AGV 动效与品牌宣传区 */}
        <div className="hidden md:flex md:w-1/2 flex-col items-center justify-center relative z-10 border-r border-white/5">
          <h1 className="text-4xl font-bold tracking-widest text-indigo-500">AGV SYSTEM</h1>
          <AgvAnimation />
        </div>

        {/* 右侧：核心登录表单区 */}
        <div className="flex-1 flex flex-col items-center justify-center relative z-10">
            <div className="relative overflow-hidden rounded-xl p-[2px]">
                {/* 旋转的发光层：用 Tailwind 的 animate-spin 配合自定义渐变 */}
                <div className="absolute inset-[-100%] animate-spin bg-[conic-gradient(from_90deg_at_50%_50%,#000000_0%,#4f46e5_50%,#000000_100%)]" />
                <Card className="w-[400px] shadow-2xl bg-[#1C1C1C] relative z-10 rounded-xl">
                    <div className="mb-8">
                        <div className="text-indigo-500 font-bold text-xs tracking-widest">RCS</div>
                        <h2 className="text-2xl font-bold mt-1">{t('login.title')}</h2>
                    </div>
                    <Form
                    name="login"
                    initialValues={{ remember: true }}
                    style={{ maxWidth: 360 }}
                    onFinish={onFinish}
                    >
                    <Form.Item
                        name="username"
                        rules={[{ required: true, message:  t('login.usernameMessage')  }]}
                    >
                        <Input prefix={<UserOutlined />} placeholder={t('login.username')} />
                    </Form.Item>
                    <Form.Item
                        name="password"
                        rules={[{ required: true, message: t('login.pwdMessage') } ]}>
                        <Input prefix={<LockOutlined />} type="password" placeholder={t('login.password')} />
                    </Form.Item>
                    <Form.Item>
                        <Flex justify="space-between" align="center">
                        <Form.Item name="remember" valuePropName="checked" noStyle>
                            <Checkbox>{t('login.remember')}</Checkbox>
                        </Form.Item>
                        <a href="">{t('login.forgetPassword')}</a>
                        </Flex>
                    </Form.Item>

                    <Form.Item>
                        <Button block type="primary" htmlType="submit">
                        {t('login.submit')}
                        </Button>
                    </Form.Item>
                    </Form>
                </Card>
            </div>
        </div>

    </div>
  </ConfigProvider>
  
  
);
}

export default Login;