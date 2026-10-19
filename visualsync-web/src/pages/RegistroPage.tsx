import { useState } from 'react';
import { useForm } from 'react-hook-form';
import { zodResolver } from '@hookform/resolvers/zod';
import { Link, useNavigate } from 'react-router-dom';
import { registroSchema } from '../schemas';
import { registrarUsuario } from '../api/auth';

export default function RegistroPage() {
  const [errorApi, setErrorApi] = useState('');
  const navigate = useNavigate();

  const { register, handleSubmit, formState: { errors, isSubmitting } } = useForm({
    resolver: zodResolver(registroSchema)
  });

  const onSubmit = async (data: any) => {
    setErrorApi('');
    try {
      await registrarUsuario(data);
      navigate('/login');
    } catch (error: any) {
      setErrorApi(error.response?.data?.detail || 'No se pudo completar el registro. Verifica los datos.');
    }
  };

  return (
    <div className="row justify-content-center">
      <div className="col-12 col-md-8 col-lg-6">
        <div className="card shadow-sm border-0 mt-5">
          <div className="card-body p-4">
            <h3 className="text-center mb-4 fw-bold text-primary">Crear Cuenta</h3>
            
            {errorApi && <div className="alert alert-danger py-2">{errorApi}</div>}
            
            <form onSubmit={handleSubmit(onSubmit)}>
              <div className="mb-3">
                <label className="form-label fw-semibold">Nombre Completo</label>
                <input type="text" className={`form-control ${errors.nombreCompleto ? 'is-invalid' : ''}`} {...register('nombreCompleto')} />
                <div className="invalid-feedback">{errors.nombreCompleto?.message as string}</div>
              </div>
              
              <div className="mb-3">
                <label className="form-label fw-semibold">Correo Electrónico</label>
                <input type="email" className={`form-control ${errors.email ? 'is-invalid' : ''}`} {...register('email')} />
                <div className="invalid-feedback">{errors.email?.message as string}</div>
              </div>
              
              <div className="row">
                <div className="col-md-6 mb-3">
                  <label className="form-label fw-semibold">Contraseña</label>
                  <input type="password" className={`form-control ${errors.password ? 'is-invalid' : ''}`} {...register('password')} />
                  <div className="invalid-feedback">{errors.password?.message as string}</div>
                </div>
                
                <div className="col-md-6 mb-4">
                  <label className="form-label fw-semibold">Confirmar Contraseña</label>
                  <input type="password" className={`form-control ${errors.confirmar ? 'is-invalid' : ''}`} {...register('confirmar')} />
                  <div className="invalid-feedback">{errors.confirmar?.message as string}</div>
                </div>
              </div>
              
              <button type="submit" className="btn btn-primary w-100 fw-bold" disabled={isSubmitting}>
                {isSubmitting ? 'Registrando...' : 'Registrarse'}
              </button>
            </form>
            
            <div className="text-center mt-3">
              <span className="text-muted">¿Ya tienes cuenta? </span>
              <Link to="/login" className="text-decoration-none fw-bold">Inicia sesión</Link>
            </div>
          </div>
        </div>
      </div>
    </div>
  );
}