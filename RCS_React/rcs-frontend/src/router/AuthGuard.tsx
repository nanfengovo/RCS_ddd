import React from 'react';
import { Navigate, useLocation } from 'react-router-dom';

interface AuthGuardProps {
  children: React.ReactNode;
}

const AuthGuard: React.FC<AuthGuardProps> = ({ children }) => {
  const token = localStorage.getItem('accessToken');
  const location = useLocation();

  if (!token) {
    // 🚀 优雅细节：记录用户本来想去哪里，登录后可以直接跳回
    return <Navigate to="/login" state={{ from: location }} replace />;
  }

  return <>{children}</>;
};

export default AuthGuard;