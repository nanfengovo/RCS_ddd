import React from 'react'; // 🚀 移除了 useState，我们不再需要局部状态了
// 🚀 移除了 ConfigProvider 和 theme，它们现在由 App.tsx 统一接管
import { Button, Card, Form, Input, Flex, Checkbox, message } from 'antd';
import { SunOutlined, MoonOutlined, LockOutlined, UserOutlined } from '@ant-design/icons';
import { useTranslation } from 'react-i18next';
import AgvAnimation from './components/AgvAnimation';
import { postApiSysAuthLogin, type ApiResponse } from '../../api/client/index';

// 🚀 引入我们刚刚创建的全局主题 Hook (请确保路径和你刚才创建 Context 文件的路径一致)
import { useTheme } from '../../contexts/ThemeContext'; 

const Login = () => {
    // 🚀 使用全局的主题状态和切换函数！
    const { isDark, toggleTheme } = useTheme(); 
    const { t, i18n } = useTranslation();
    
    const onFinish = async (values: any) => {
        try {
            const result = await postApiSysAuthLogin({
                body: {
                    username: values.username,
                    password: values.password
                }
            });
            const responseData = result.data as ApiResponse & { data?: { token: string } };
            console.log(responseData);
            
            if (responseData.code === 200 && responseData.isSuccess) {
                // 登录成功，拿到 token 了！
                const token = responseData.data?.token;
                console.log('登录成功，拿到 token 了！', token);
                
                if (token) {
                    // 1. 存入浏览器的本地存储
                    localStorage.setItem('accessToken', token);
                    
                    // 2. 给予用户成功反馈
                    message.success('登录成功，正在进入系统...');
                    
                    // 3. 页面跳转
                    window.location.href = '/';
                }
            } else {
                message.error(responseData.message || '登录失败');
            }
        } catch (error) {
            console.log(error);
        }
    };

    // 🚀 注意看这里：最外层直接是 div，再也没有 ConfigProvider 包裹了
    return (
        <div className={`min-h-screen flex relative transition-colors duration-300 overflow-hidden ${isDark ? 'bg-[#0f1219] text-white' : 'bg-[#eef2f6] text-slate-900'}`}>
            
            {/* Light mode diagonal split background */}
            {!isDark && (
              <div className="absolute inset-0 z-0 bg-white shadow-[0_0_50px_rgba(0,0,0,0.05)]" style={{ clipPath: 'polygon(60% 0, 100% 0, 100% 100%, 25% 100%)' }}></div>
            )}

            {/* Global Grid Background */}
            <div className="absolute inset-0 z-0 pointer-events-none bg-[linear-gradient(to_right,#80808012_1px,transparent_1px),linear-gradient(to_bottom,#80808012_1px,transparent_1px)] bg-[size:32px_32px]"></div>
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
                    onClick={toggleTheme} // 🚀 这里不再是 setIsDark(!isDark)，而是调用全局的 toggleTheme
                    icon={isDark ? <SunOutlined className='text-yellow-400' /> : <MoonOutlined />}
                />
            </div>

            {/* 左侧：品牌宣传与动效区 */}
            <div className="hidden md:flex md:w-1/2 flex-col justify-center pl-[10%] pr-10 relative z-10">
              
              {/* Logo & Title */}
              <div className="mb-10 flex items-center gap-4">
                 {/* Logo Icon */}
                 <div className="w-14 h-14 bg-gradient-to-br from-indigo-500 to-purple-600 rounded-2xl flex items-center justify-center shadow-lg shadow-indigo-500/30">
                    <div className="w-0 h-0 border-l-[10px] border-r-[10px] border-b-[16px] border-l-transparent border-r-transparent border-b-white/90 translate-y-[-2px]"></div>
                 </div>
                 <div>
                    <div className="text-[11px] font-bold text-cyan-600 dark:text-cyan-400 tracking-widest uppercase mb-1">RCS Control Suite</div>
                    <h1 className="text-3xl font-bold tracking-wider text-slate-800 dark:text-white">RCS 管理系统</h1>
                 </div>
              </div>

              {/* Animation Area */}
              <div className="w-full max-w-[500px]">
                <AgvAnimation />
              </div>

              {/* Status Cards */}
              <div className="flex gap-4 mt-6 w-full max-w-[500px]">
                 <div className="flex-1 p-4 rounded-xl border border-white/40 dark:border-white/10 bg-white/30 dark:bg-black/20 backdrop-blur-md shadow-lg shadow-black/5">
                    <div className="text-[10px] text-gray-500 dark:text-gray-400 font-bold mb-1">AMHS</div>
                    <div className="text-sm font-bold text-cyan-600 dark:text-cyan-400">READY</div>
                 </div>
                 <div className="flex-1 p-4 rounded-xl border border-white/40 dark:border-white/10 bg-white/30 dark:bg-black/20 backdrop-blur-md shadow-lg shadow-black/5">
                    <div className="text-[10px] text-gray-500 dark:text-gray-400 font-bold mb-1">WMS</div>
                    <div className="text-sm font-bold text-cyan-600 dark:text-cyan-400">ONLINE</div>
                 </div>
                 <div className="flex-1 p-4 rounded-xl border border-white/40 dark:border-white/10 bg-white/30 dark:bg-black/20 backdrop-blur-md shadow-lg shadow-black/5">
                    <div className="text-[10px] text-gray-500 dark:text-gray-400 font-bold mb-1">DEVICE</div>
                    <div className="text-sm font-bold text-cyan-600 dark:text-cyan-400">SYNC</div>
                 </div>
              </div>
            </div>

            {/* 右侧：核心登录表单区 */}
            <div className="flex-1 flex flex-col items-center justify-center relative z-10">
                <div className="relative overflow-hidden rounded-xl p-[2px]">
                    {/* 旋转的发光层 */}
                    <div className="absolute inset-[-100%] animate-[spin_4s_linear_infinite] bg-[conic-gradient(from_90deg_at_50%_50%,transparent_0%,#4f46e5_50%,transparent_100%)]" />
                    <Card className="w-[400px] shadow-2xl bg-white dark:bg-[#151b28] border-none relative z-10 rounded-xl">
                        <div className="mb-8">
                            <div className="text-cyan-600 dark:text-cyan-400 font-bold text-[10px] tracking-widest uppercase mb-1">OPERATOR ACCESS</div>
                            <h2 className="text-2xl font-bold mt-1 text-slate-800 dark:text-white">{t('login.title') || '密码登录'}</h2>
                        </div>
                        <Form
                        name="login"
                        initialValues={
                            { 
                                remember: true,
                                username: 'admin',
                                password: '123456'
                            }
                        }
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
                            <Input.Password prefix={<LockOutlined />} placeholder={t('login.password')} />
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
    );
}

export default Login;