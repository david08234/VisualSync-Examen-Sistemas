import { client } from './client'; // Asegúrate de que apunte a donde configuraste Axios (Punto 8)

// Obtener la URL temporal para mostrar la miniatura en la galería
export async function obtenerMiniatura(id: number) {
  const { data } = await client.get(`/archivos/${id}/miniatura`, { responseType: 'blob' });
  return URL.createObjectURL(data);
}

// Descargar la foto original en alta resolución
export async function descargarFoto(id: number, nombreArchivo: string) {
  const { data } = await client.get(`/archivos/${id}/descargar`, { responseType: 'blob' });
  
  // Magia del navegador para forzar la descarga del archivo
  const url = URL.createObjectURL(data);
  const link = document.createElement('a');
  link.href = url;
  link.setAttribute('download', nombreArchivo);
  document.body.appendChild(link);
  link.click();
  document.body.removeChild(link);
  URL.revokeObjectURL(url);
}