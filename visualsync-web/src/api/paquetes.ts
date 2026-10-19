import { client } from './client';
import type { PaqueteServicio } from '../types';

export const listar = () => client.get<PaqueteServicio[]>('/PaquetesServicio').then(r => r.data);
export const obtener = (id: number) => client.get<PaqueteServicio>(`/PaquetesServicio/${id}`).then(r => r.data);
export const crear = (d: Omit<PaqueteServicio, 'idPaquete'>) => client.post<PaqueteServicio>('/PaquetesServicio', d).then(r => r.data);
export const actualizar = (id: number, d: Omit<PaqueteServicio, 'idPaquete'>) => client.put<PaqueteServicio>(`/PaquetesServicio/${id}`, d).then(r => r.data);
export const desactivar = (id: number) => client.delete(`/PaquetesServicio/${id}`);