import { useState } from 'react';
import { Button } from 'antd';

export default function AgvAnimation() {
  // 🧠 核心：用状态控制滑块的位置
  const [position, setPosition] = useState(0);

  return (
    <div className="flex flex-col items-center gap-10 p-10">
      
      {/* 1. 轨道与滑块 */}
      <div className="relative w-[300px] h-[4px] bg-gray-700">
        {/* 🚀 这就是那个 AGV 小滑块 */}
        <div 
          className="absolute w-8 h-6 bg-indigo-500 rounded-sm transition-all duration-500 ease-in-out"
          style={{ left: `${position}px`, top: '-10px' }} 
        />
      </div>

      {/* 2. 手动控制器 */}
      <Button 
        onClick={() => setPosition(prev => (prev >= 270 ? 0 : prev + 50))}
      >
        模拟 AGV 移动 (点击前进)
      </Button>

      {/* 3. 状态呼吸灯 */}
      <div className="flex gap-4">
        <div className="w-4 h-4 bg-emerald-500 rounded-full animate-pulse" />
        <span className="text-sm">AMHS READY</span>
      </div>
    </div>
  );
}