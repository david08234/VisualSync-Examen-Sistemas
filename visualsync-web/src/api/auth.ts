import { client } from './client';
import type { Usuario } from '../types';

export interface LoginResponse {
  token: string;
  expiraEnMinutos: number;
  usuario: Usuario;
}

export const loginUsuario = async (datos: any): Promise<LoginResponse> => {
  // Obtenemos los usuarios reales de tu backend
  const response = await client.get<Usuario[]>('/Usuarios');
  const usuarios = response.data;
  
  // Simulamos la validación buscando coincidencias
  const usuario = usuarios.find(u => u.email === datos.email && u.passwordHash === datos.password);
  
  if (!usuario) {
    throw new Error('Credenciales incorrectas o usuario no encontrado.');
  }
  
  return {
    token: 'token-simulado-para-el-frontend',
    expiraEnMinutos: 120,
    usuario
  };
};

export const registrarUsuario = async (datos: any) => {
  const nuevoUsuario = {
    nombreCompleto: datos.nombreCompleto,
    email: datos.email,
    passwordHash: datos.password, // Se envía a la propiedad correcta del backend
    rol: 'Cliente'
  };
  return client.post<Usuario>('/Usuarios', nuevoUsuario).then(r => r.data);
};