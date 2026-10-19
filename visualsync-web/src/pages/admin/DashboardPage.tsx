import { useEffect, useState } from 'react';
import { Link } from 'react-router-dom';
import { listarReservas } from '../../api/reservas';
import type { Reserva } from '../../types';
import EstadoPagoBadge from '../../components/EstadoPagoBadge';
import Loading from '../../components/Loading';
import ErrorAlert from '../../components/ErrorAlert';

export default function DashboardPage() {
  const [reservas, setReservas] = useState<Reserva[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState('');

  useEffect(() => {
    listarReservas()
      .then(setReservas)
      .catch(() => setError('Error al cargar las reservas desde el servidor.'))
      .finally(() => setLoading(false));
  }, []);

  return (
    <div className="container py-4">
      <h2 className="fw-bold mb-4" style={{ color: '#14213D' }}>Panel de Administración</h2>
      
      {loading && <Loading />}
      <ErrorAlert mensaje={error} />
      
      {!loading && reservas.length === 0 && (
        <div className="alert alert-info shadow-sm border-0">No hay reservas registradas en el sistema.</div>
      )}

      {!loading && reservas.length > 0 && (
        <div className="table-responsive bg-white rounded shadow-sm">
          <table className="table table-hover align-middle mb-0">
            <thead className="table-light">
              <tr>
                <th>ID</th>
                <th>Fecha Evento</th>
                <th>Estado</th>
                <th>Pago</th>
                <th className="text-end">Acciones</th>
              </tr>
            </thead>
            <tbody>
              {reservas.map(r => (
                <tr key={r.idReserva}>
                  <td className="fw-bold text-muted">#{r.idReserva}</td>
                  <td>{new Date(r.fechaEvento).toLocaleDateString()}</td>
                  <td><span className="badge bg-light text-dark border">{r.estadoReserva}</span></td>
                  <td><EstadoPagoBadge estado={r.estadoPago} /></td>
                  <td className="text-end">
                    <Link to={`/admin/reservas/${r.idReserva}`} className="btn btn-sm text-white" style={{ backgroundColor: '#14213D' }}>
                      Gestionar
                    </Link>
                  </td>
                </tr>
              ))}
            </tbody>
          </table>
        </div>
      )}
    </div>
  );
}