import { useState, useEffect } from 'react';
import { useParams, useNavigate } from 'react-router-dom';
import { obtenerReserva, actualizarReserva } from '../../api/reservas';
import type { Reserva } from '../../types';
import EstadoPagoBadge from '../../components/EstadoPagoBadge';
import Loading from '../../components/Loading';

export default function ReservaDetallePage() {
  const { id } = useParams();
  const navigate = useNavigate();
  const [reserva, setReserva] = useState<Reserva | null>(null);
  const [loading, setLoading] = useState(true);

  const cargarReserva = () => {
    setLoading(true);
    obtenerReserva(Number(id))
      .then(setReserva)
      .catch(() => alert('Error al cargar la reserva.'))
      .finally(() => setLoading(false));
  };

  useEffect(() => { cargarReserva(); }, [id]);

  const cambiarEstadoPago = async (nuevoEstado: string) => {
    if (!reserva) return;
    const motivo = nuevoEstado === 'Rechazado' ? window.prompt('Escribe el motivo del rechazo:') : null;
    
    if (nuevoEstado === 'Rechazado' && !motivo) return; // Canceló el prompt

    try {
      const reservaActualizada = { ...reserva, estadoPago: nuevoEstado, motivoRechazo: motivo };
      if (nuevoEstado === 'Aprobado') reservaActualizada.estadoReserva = 'Confirmada';
      
      await actualizarReserva(reserva.idReserva!, reservaActualizada);
      cargarReserva();
    } catch {
      alert('Ocurrió un error al actualizar el estado.');
    }
  };

  if (loading) return <Loading />;
  if (!reserva) return <div className="alert alert-danger mt-4">Reserva no encontrada.</div>;

  return (
    <div className="container py-4">
      <button className="btn btn-outline-secondary mb-4" onClick={() => navigate('/admin')}>
        &larr; Volver al Dashboard
      </button>

      <div className="card shadow-sm border-0">
        <div className="card-body p-4">
          <h3 className="fw-bold mb-4" style={{ color: '#14213D' }}>Reserva #{reserva.idReserva}</h3>
          
          <div className="row mb-4">
            <div className="col-md-6 mb-3">
              <h6 className="text-muted text-uppercase small fw-bold">Lugar del Evento</h6>
              <p className="fs-5">{reserva.lugarEvento}</p>
            </div>
            <div className="col-md-6 mb-3">
              <h6 className="text-muted text-uppercase small fw-bold">Fecha programada</h6>
              <p className="fs-5">{new Date(reserva.fechaEvento).toLocaleString()}</p>
            </div>
            <div className="col-md-6 mb-3">
              <h6 className="text-muted text-uppercase small fw-bold">Estado General</h6>
              <p className="fs-5"><span className="badge bg-light text-dark border">{reserva.estadoReserva}</span></p>
            </div>
            <div className="col-md-6 mb-3">
              <h6 className="text-muted text-uppercase small fw-bold">Estado del Pago</h6>
              <p className="fs-5"><EstadoPagoBadge estado={reserva.estadoPago} /></p>
            </div>
          </div>

          <hr className="my-4" />

          <div className="d-flex gap-3">
            <button 
              className="btn btn-success fw-bold px-4" 
              onClick={() => cambiarEstadoPago('Aprobado')}
              disabled={reserva.estadoPago === 'Aprobado'}
            >
              ✓ Aprobar Pago
            </button>
            <button 
              className="btn btn-danger fw-bold px-4" 
              onClick={() => cambiarEstadoPago('Rechazado')}
              disabled={reserva.estadoPago === 'Rechazado'}
            >
              ✕ Rechazar Pago
            </button>
          </div>
        </div>
      </div>
    </div>
  );
}