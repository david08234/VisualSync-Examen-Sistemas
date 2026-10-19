import { Link, useNavigate } from 'react-router-dom';
import { useAuth } from '../context/AuthContext';

export default function Navbar() {
  const { usuario, logout } = useAuth();
  const navigate = useNavigate();

  const handleLogout = () => {
    logout();
    navigate('/login');
  };

  return (
    <nav className="navbar navbar-expand-lg navbar-dark shadow-sm" style={{ backgroundColor: '#14213D' }}>
      <div className="container">
        <Link className="navbar-brand fw-bold" to="/">VisualSync</Link>
        <button className="navbar-toggler" type="button" data-bs-toggle="collapse" data-bs-target="#navbarNav">
          <span className="navbar-toggler-icon"></span>
        </button>
        
        <div className="collapse navbar-collapse" id="navbarNav">
          <ul className="navbar-nav ms-auto align-items-center">
            {!usuario && (
              <>
                <li className="nav-item"><Link className="nav-link" to="/">Catálogo</Link></li>
                <li className="nav-item"><Link className="nav-link" to="/login">Iniciar Sesión</Link></li>
                <li className="nav-item ms-2"><Link className="btn btn-primary fw-bold px-4" to="/registro">Registrarse</Link></li>
              </>
            )}
            
            {usuario?.rol === 'Cliente' && (
              <>
                <li className="nav-item"><Link className="nav-link" to="/">Inicio</Link></li>
                <li className="nav-item"><Link className="nav-link" to="/mis-reservas">Mis reservas</Link></li>
                <li className="nav-item ms-2">
                  <button className="btn btn-outline-light btn-sm" onClick={handleLogout}>Cerrar sesión</button>
                </li>
              </>
            )}

            {usuario?.rol === 'Admin' && (
              <>
                <li className="nav-item"><Link className="nav-link" to="/admin">Panel general</Link></li>
                <li className="nav-item"><Link className="nav-link" to="/admin/paquetes">Paquetes</Link></li>
                <li className="nav-item ms-2">
                  <button className="btn btn-outline-light btn-sm" onClick={handleLogout}>Cerrar sesión</button>
                </li>
              </>
            )}
          </ul>
        </div>
      </div>
    </nav>
  );
}