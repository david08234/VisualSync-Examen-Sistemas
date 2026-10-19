import { client } from './client';
import type { Reserva } from '../types';

export const listarReservas = () => client.get<Reserva[]>('/Reservas').then(r => r.data);

export const obtenerReserva = (id: number) => client.get<Reserva>(`/Reservas/${id}`).then(r => r.data);

export const crearReserva = (datos: any) => client.post<Reserva>('/Reservas', datos).then(r => r.data);

// En la API actual, actualizamos la reserva completa para cambiar su estado
export const actualizarReserva = (id: number, datos: Reserva) => client.put(`/Reservas/${id}`, datos).then(r => r.data);

export const eliminarReserva = (id: number) => client.delete(`/Reservas/${id}`);