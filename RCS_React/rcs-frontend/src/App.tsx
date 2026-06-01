// 引入刚刚写好的独立登录组件
import Login from './Pages/Login';

export default function App() {
  // 目前我们只渲染 Login，后续加了 React Router，这里就会变成一堆 <Route> 标签
  return (
    <Login />
  );
}