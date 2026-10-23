import type { ReactElement } from 'react';
import { Navigate, useLocation } from 'react-router-dom';
import { useAuth } from '../context/AuthContext';
import type { Rol } from '../context/AuthContext';

interface Props {
  rol?: Rol;
  children: ReactElement;
}

export default function ProtectedRoute({ rol, children }: Props) {
  const { usuario } = useAuth();
  const location = useLocation();

  if (!usuario) {
    return <Navigate to="/login" replace state={{ from: location }} />;
  }

  if (rol && usuario.rol !== rol) {
    return <Navigate to="/" replace />;
  }

  return children;
}