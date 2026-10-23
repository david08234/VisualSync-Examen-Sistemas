import { Routes, Route } from 'react-router-dom';
import Navbar from './components/Navbar';
import ProtectedRoute from './components/ProtectedRoute';

import CatalogoPage from './pages/CatalogoPage';
import LoginPage from './pages/LoginPage';
import RegistroPage from './pages/RegistroPage';
import SolicitarReservaPage from './pages/SolicitarReservaPage';
import MisReservasPage from './pages/MisReservasPage';
import GaleriaPage from './pages/GaleriaPage';
import DashboardPage from './pages/admin/DashboardPage';
import PaquetesPage from './pages/admin/PaquetesPage';
import ReservaDetallePage from './pages/admin/ReservaDetallePage';

export default function App() {
  return (
    <>
      <Navbar />
      <main className="min-vh-100 bg-light pb-5" style={{ backgroundColor: '#F8FAFC' }}>
        <Routes>
          {/* Rutas Públicas */}
          <Route path="/" element={<CatalogoPage />} />
          <Route path="/login" element={<LoginPage />} />
          <Route path="/registro" element={<RegistroPage />} />
          
          {/* Rutas Cliente */}
          <Route path="/reservar/:idPaquete" element={<ProtectedRoute rol="Cliente"><SolicitarReservaPage /></ProtectedRoute>} />
          <Route path="/mis-reservas" element={<ProtectedRoute rol="Cliente"><MisReservasPage /></ProtectedRoute>} />
          <Route path="/reservas/:id/galeria" element={<ProtectedRoute rol="Cliente"><GaleriaPage /></ProtectedRoute>} />

          {/* Rutas Admin */}
          <Route path="/admin" element={<ProtectedRoute rol="Admin"><DashboardPage /></ProtectedRoute>} />
          <Route path="/admin/paquetes" element={<ProtectedRoute rol="Admin"><PaquetesPage /></ProtectedRoute>} />
          <Route path="/admin/reservas/:id" element={<ProtectedRoute rol="Admin"><ReservaDetallePage /></ProtectedRoute>} />
          
          {/* 404 */}
          <Route path="*" element={<div className="text-center mt-5 py-5"><h2 className="fw-bold text-muted">Error 404: Página no encontrada</h2></div>} />
        </Routes>
      </main>
    </>
  );
}