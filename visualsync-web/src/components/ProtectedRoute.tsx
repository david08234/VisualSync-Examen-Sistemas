import { Navigate } from 'react-router-dom';
import { useAuth } from '../context/AuthContext';
import type { ReactElement } from 'react';

export default function ProtectedRoute({ rol, children }: { rol?: string; children: ReactElement }) {
  const { usuario } = useAuth();
  
  if (!usuario) return <Navigate to='/login' replace />;
  if (rol && usuario.rol !== rol) return <Navigate to='/' replace />;
  
  return children;
}