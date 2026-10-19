export default function EstadoPagoBadge({ estado }: { estado: string }) {
  const badgeClass = () => {
    switch (estado) {
      case 'Aprobado': return 'bg-success';
      case 'Rechazado': return 'bg-danger';
      case 'PendienteValidacion': return 'bg-warning text-dark';
      default: return 'bg-secondary';
    }
  };

  const etiqueta = estado.replace(/([A-Z])/g, ' $1').trim(); // Separa "PendienteValidacion"

  return <span className={`badge ${badgeClass()}`}>{etiqueta}</span>;
}