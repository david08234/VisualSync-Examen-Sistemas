import axios from 'axios';

export function mensajeDeError(e: unknown): string {
  if (axios.isAxiosError(e)) {
    // Si la API está apagada o no hay internet
    if (!e.response) return 'No se pudo conectar con el servidor. Verifica tu conexión o intenta más tarde.';
    
    const d = e.response.data;
    
    // Si es un error 400 con múltiples fallos de validación (ProblemDetails)
    if (d?.errors) {
      return Object.values(d.errors).flat().join(' ');
    }
    
    // Si es un mensaje específico del backend (ej. "Credenciales incorrectas")
    return d?.detail ?? d?.message ?? 'Ocurrió un error inesperado al procesar la solicitud.';
  }
  
  // Si el error no viene de Axios (ej. un error de sintaxis en React)
  if (e instanceof Error) return e.message;
  
  return 'Ocurrió un error desconocido.';
}