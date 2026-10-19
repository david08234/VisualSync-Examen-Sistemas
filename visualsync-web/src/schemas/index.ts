import { z } from 'zod';

export const loginSchema = z.object({
  email: z.string().email('Correo inválido.'),
  password: z.string().min(1, 'La contraseña es obligatoria.')
});

export const registroSchema = z.object({
  nombreCompleto: z.string().min(3, 'Ingresa tu nombre completo.'),
  email: z.string().email('Correo inválido.'),
  password: z.string().min(8, 'Mínimo 8 caracteres.'),
  confirmar: z.string(),
}).refine((d) => d.password === d.confirmar, {
  message: 'Las contraseñas no coinciden.',
  path: ['confirmar'],
});