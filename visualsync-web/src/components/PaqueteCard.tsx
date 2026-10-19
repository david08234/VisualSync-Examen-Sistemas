import { Link } from 'react-router-dom';
import type { PaqueteServicio } from '../types';

export default function PaqueteCard({ paquete }: { paquete: PaqueteServicio }) {
  return (
    <div className="card h-100 shadow-sm border-0">
      <div className="card-body">
        <h5 className="card-title fw-bold text-primary">{paquete.nombre}</h5>
        <p className="card-text text-secondary">{paquete.descripcion}</p>
        <ul className="list-unstyled mb-0">
          <li className="mb-2">💰 <strong>Precio:</strong> Bs. {paquete.precio}</li>
          <li className="mb-2">⏱️ <strong>Duración:</strong> {paquete.duracion} horas</li>
          <li className="mb-2">📸 <strong>Fotos:</strong> {paquete.cantidadFotos} editadas</li>
          <li>🎥 <strong>Video:</strong> {paquete.incluyeVideo ? 'Incluido' : 'No incluido'}</li>
        </ul>
      </div>
      <div className="card-footer bg-white border-0 pb-3 pt-0">
        <Link to={`/reservar/${paquete.idPaquete}`} className="btn btn-primary w-100 fw-bold">
          Reservar
        </Link>
      </div>
    </div>
  );
}