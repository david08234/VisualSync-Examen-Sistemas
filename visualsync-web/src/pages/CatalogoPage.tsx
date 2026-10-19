import { useEffect, useState } from 'react';
import { listar } from '../api/paquetes';
import type { PaqueteServicio } from '../types';
import PaqueteCard from '../components/PaqueteCard';

export default function CatalogoPage() {
  const [paquetes, setPaquetes] = useState<PaqueteServicio[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState('');

  useEffect(() => {
    listar()
      // Filtramos para mostrar solo los que tienen activo: true o los que no tienen la propiedad definida (por defecto)
      .then(data => setPaquetes(data.filter(p => p.activo !== false)))
      .catch(() => setError('No se pudieron cargar los paquetes. Verifica que la API esté encendida.'))
      .finally(() => setLoading(false));
  }, []);

  return (
    <div className="container py-4">
      <div className="text-center mb-5">
        <h1 className="fw-bold">Nuestros Paquetes</h1>
        <p className="text-muted">Elige el servicio perfecto para tu sesión fotográfica.</p>
      </div>

      {loading && <div className="text-center"><div className="spinner-border text-primary" /></div>}
      {error && <div className="alert alert-danger text-center">{error}</div>}

      {!loading && !error && paquetes.length === 0 && (
        <div className="alert alert-info text-center">No hay paquetes disponibles en este momento.</div>
      )}

      <div className="row g-4">
        {paquetes.map(p => (
          <div key={p.idPaquete} className="col-12 col-md-6 col-lg-4">
            <PaqueteCard paquete={p} />
          </div>
        ))}
      </div>
    </div>
  );
}