## Documentación técnica

**Requisitos:** .NET SDK 10, Node.js 22, Docker (opcional) y una base de datos PostgreSQL (Supabase).

### Ejecución local sin Docker

**Backend**
```bash
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "<cadena de Supabase>"
dotnet user-secrets set "Jwt:Key" "<clave de al menos 32 caracteres>"
dotnet run