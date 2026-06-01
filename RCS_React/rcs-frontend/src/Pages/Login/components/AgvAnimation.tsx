import React from 'react';

export default function AgvAnimation() {
  return (
    <div className="relative w-full h-[260px] rounded-xl overflow-hidden border border-gray-200 dark:border-white/10 bg-white/50 dark:bg-white/5 backdrop-blur-md flex flex-col justify-center px-10">
      {/* Grid Background */}
      <div className="absolute inset-0 z-0 pointer-events-none bg-[linear-gradient(to_right,#8080801a_1px,transparent_1px),linear-gradient(to_bottom,#8080801a_1px,transparent_1px)] bg-[size:32px_32px] [background-position:center_center]"></div>
      
      <style>{`
        @keyframes scanline {
          0% { transform: translateX(-200%); }
          100% { transform: translateX(300%); }
        }
        @keyframes move-agv-1 {
          0%, 100% { left: 10%; }
          50% { left: 80%; }
        }
        @keyframes move-agv-2 {
          0%, 100% { left: 85%; }
          50% { left: 15%; }
        }
        @keyframes move-agv-3 {
          0%, 100% { left: 40%; }
          50% { left: 90%; }
        }
        @keyframes move-agv-4 {
          0%, 100% { left: 20%; }
          50% { left: 70%; }
        }
        @keyframes move-agv-5 {
          0%, 100% { left: 75%; }
          50% { left: 25%; }
        }
      `}</style>

      {/* Track 1 */}
      <div className="relative w-full h-[1px] bg-gray-300 dark:bg-gray-600/50 my-5 z-10">
        <div className="absolute top-0 h-full w-[150px] bg-gradient-to-r from-transparent via-cyan-500 to-transparent animate-[scanline_4s_linear_infinite]"></div>
        <div className="absolute -top-[9px] w-5 h-5 border border-cyan-400/50 rounded flex items-center justify-center bg-white dark:bg-[#151b28]" style={{ animation: 'move-agv-1 8s ease-in-out infinite' }}>
            <div className="w-2 h-2 bg-orange-500 rounded-[2px]"></div>
        </div>
        <div className="absolute -top-[6px] w-14 h-[14px] bg-gradient-to-r from-orange-400 to-yellow-400 rounded-full shadow-[0_0_10px_rgba(245,158,11,0.5)]" style={{ animation: 'move-agv-2 10s ease-in-out infinite' }}></div>
      </div>

      {/* Track 2 */}
      <div className="relative w-full h-[1px] bg-gray-300 dark:bg-gray-600/50 my-5 z-10">
        <div className="absolute top-0 h-full w-[120px] bg-gradient-to-r from-transparent via-cyan-500 to-transparent animate-[scanline_3s_linear_infinite_0.5s]"></div>
        <div className="absolute -top-[9px] w-5 h-5 border border-cyan-400/50 rounded flex items-center justify-center bg-white dark:bg-[#151b28]" style={{ animation: 'move-agv-3 7s ease-in-out infinite' }}>
            <div className="w-2 h-2 bg-orange-500 rounded-[2px]"></div>
        </div>
      </div>

      {/* Track 3 */}
      <div className="relative w-full h-[1px] bg-gray-300 dark:bg-gray-600/50 my-5 z-10">
        <div className="absolute top-0 h-full w-[180px] bg-gradient-to-r from-transparent via-cyan-500 to-transparent animate-[scanline_5s_linear_infinite_1s]"></div>
        <div className="absolute -top-[6px] w-14 h-[14px] bg-gradient-to-r from-orange-400 to-yellow-400 rounded-full shadow-[0_0_10px_rgba(245,158,11,0.5)]" style={{ animation: 'move-agv-4 9s ease-in-out infinite' }}></div>
        <div className="absolute -top-[9px] w-5 h-5 border border-cyan-400/50 rounded flex items-center justify-center bg-white dark:bg-[#151b28]" style={{ animation: 'move-agv-5 6s ease-in-out infinite' }}>
            <div className="w-2 h-2 bg-orange-500 rounded-[2px]"></div>
        </div>
      </div>
    </div>
  );
}