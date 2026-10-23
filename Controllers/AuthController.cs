using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using VisualSync.Api.Data;
using VisualSync.Api.DTOs;
using VisualSync.Api.Models;

namespace VisualSync.Api.Controllers;

[Route("api/auth")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly ApplicationDbContext _db;
    private readonly IConfiguration _config;

    public AuthController(ApplicationDbContext db, IConfiguration config)
    {
        _db = db;
        _config = config;
    }

    [HttpPost("registro")]
    public async Task<IActionResult> Registro(RegistroDto dto)
    {
        var email = dto.Email.Trim().ToLowerInvariant();

        if (await _db.Usuarios.AnyAsync(u => u.Email == email))
            return Conflict(new ProblemDetails
            {
                Status = 409, Title = "Conflict", Detail = "Ya existe una cuenta con ese correo."
            });

        var usuario = new Usuario
        {
            NombreCompleto = dto.NombreCompleto.Trim(),
            Email = email,
            // AQUÍ: se guarda el hash, nunca la contraseña
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password, 11),
            Rol = "Cliente" // el registro público siempre crea clientes
        };

        _db.Usuarios.Add(usuario);
        await _db.SaveChangesAsync();

        return StatusCode(201, new
        {
            usuario.IdUsuario, usuario.NombreCompleto, usuario.Email, usuario.Rol
        });
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginDto dto)
    {
        var email = dto.Email.Trim().ToLowerInvariant();
        var usuario = await _db.Usuarios.FirstOrDefaultAsync(u => u.Email == email);

        // AQUÍ: se compara la contraseña ingresada contra el hash guardado
        // Mismo mensaje si el correo no existe o la clave es incorrecta
        if (usuario == null || !BCrypt.Net.BCrypt.Verify(dto.Password, usuario.PasswordHash))
            return Unauthorized(new ProblemDetails
            {
                Status = 401, Title = "Unauthorized", Detail = "Credenciales incorrectas."
            });

        var minutos = int.TryParse(_config["Jwt:ExpiraMinutos"], out var m) ? m : 120;

        var descriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.NameIdentifier, usuario.IdUsuario.ToString()),
                new Claim(ClaimTypes.Email, usuario.Email),
                new Claim(ClaimTypes.Role, usuario.Rol)
            }),
            Issuer = "VisualSync.Api",
            Audience = "VisualSync.Web",
            Expires = DateTime.UtcNow.AddMinutes(minutos),
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!)),
                SecurityAlgorithms.HmacSha256)
        };

        var token = new JsonWebTokenHandler().CreateToken(descriptor);

        return Ok(new
        {
            token,
            expiraEnMinutos = minutos,
            usuario = new { usuario.IdUsuario, usuario.NombreCompleto, usuario.Rol }
        });
    }
}