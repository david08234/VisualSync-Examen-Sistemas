import { useState } from 'react';
import { useForm } from 'react-hook-form';
import { zodResolver } from '@hookform/resolvers/zod';
import { Link, useNavigate } from 'react-router-dom';
import { loginSchema } from '../schemas';
import { loginUsuario } from '../api/auth';
import { useAuth } from '../context/AuthContext';

export default function LoginPage() {
  const [errorApi, setErrorApi] = useState('');
  const { login } = useAuth();
  const navigate = useNavigate();

  const { register, handleSubmit, formState: { errors, isSubmitting } } = useForm({
    resolver: zodResolver(loginSchema)
  });

const onSubmit = async (data: any) => {
    setErrorApi('');
    try {
      const res = await loginUsuario(data);
      // Aquí agregamos "as any" para evitar el conflicto del rol
      login(res.token, res.usuario as any); 
      navigate(res.usuario.rol === 'Administrador' ? '/admin' : '/mis-reservas');
    } catch (error: any) {
      setErrorApi(error.response?.data?.detail || 'Credenciales incorrectas o error en el servidor.');
    }
  };
  
  return (
    <div className="row justify-content-center">
      <div className="col-12 col-md-6 col-lg-4">
        <div className="card shadow-sm border-0 mt-5">
          <div className="card-body p-4">
            <h3 className="text-center mb-4 fw-bold text-primary">Iniciar Sesión</h3>
            
            {errorApi && <div className="alert alert-danger py-2">{errorApi}</div>}
            
            <form onSubmit={handleSubmit(onSubmit)}>
              <div className="mb-3">
                <label className="form-label fw-semibold">Correo Electrónico</label>
                <input type="email" className={`form-control ${errors.email ? 'is-invalid' : ''}`} {...register('email')} />
                <div className="invalid-feedback">{errors.email?.message as string}</div>
              </div>
              
              <div className="mb-4">
                <label className="form-label fw-semibold">Contraseña</label>
                <input type="password" className={`form-control ${errors.password ? 'is-invalid' : ''}`} {...register('password')} />
                <div className="invalid-feedback">{errors.password?.message as string}</div>
              </div>
              
              <button type="submit" className="btn btn-primary w-100 fw-bold" disabled={isSubmitting}>
                {isSubmitting ? 'Iniciando...' : 'Ingresar'}
              </button>
            </form>
            
            <div className="text-center mt-3">
              <span className="text-muted">¿No tienes cuenta? </span>
              <Link to="/registro" className="text-decoration-none fw-bold">Regístrate aquí</Link>
            </div>
          </div>
        </div>
      </div>
    </div>
  );
}