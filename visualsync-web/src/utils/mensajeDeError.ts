import axios from 'axios';

export function mensajeDeError(e: unknown): string {
  if (axios.isAxiosError(e)) {
    if (!e.response) return 'No se pudo conectar con el servidor.';
    const d = e.response.data;
    if (d?.errors) return Object.values(d.errors).flat().join(' ');
    return d?.detail ?? 'Ocurrió un error inesperado.';
  }
  return 'Ocurrió un error inesperado.';
}

