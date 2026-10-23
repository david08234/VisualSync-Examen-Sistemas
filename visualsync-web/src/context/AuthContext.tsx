import { createContext, useContext, useState } from 'react';
import type { ReactNode } from 'react';

export type Rol = 'Admin' | 'Cliente';

export interface Usuario {
  idUsuario: number;
  nombreCompleto: string;
  rol: Rol;
}

interface AuthState {
  usuario: Usuario | null;
  login: (token: string, usuario: Usuario) => void;
  logout: () => void;
}

const AuthContext = createContext<AuthState | null>(null);

export function AuthProvider({ children }: { children: ReactNode }) {
  const [usuario, setUsuario] = useState<Usuario | null>(() => {
    try {
      const guardado = localStorage.getItem('usuario');
      return guardado ? (JSON.parse(guardado) as Usuario) : null;
    } catch {
      return null;
    }
  });

  const login = (token: string, u: Usuario) => {
    localStorage.setItem('token', token);
    localStorage.setItem('usuario', JSON.stringify(u));
    setUsuario(u);
  };

  const logout = () => {
    localStorage.removeItem('token');
    localStorage.removeItem('usuario');
    setUsuario(null);
  };

  return (
    <AuthContext.Provider value={{ usuario, login, logout }}>
      {children}
    </AuthContext.Provider>
  );
}

export function useAuth() {
  const ctx = useContext(AuthContext);
  if (!ctx) throw new Error('useAuth debe usarse dentro de AuthProvider');
  return ctx;
}