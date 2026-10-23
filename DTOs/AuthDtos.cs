using System.ComponentModel.DataAnnotations;

namespace VisualSync.Api.DTOs;

public class RegistroDto
{
    [Required, StringLength(120, MinimumLength = 3)]
    public string NombreCompleto { get; set; } = "";

    [Required, EmailAddress, StringLength(150)]
    public string Email { get; set; } = "";

    // BCrypt solo usa los primeros 72 bytes de la contraseña
    [Required, MinLength(8), MaxLength(72)]
    public string Password { get; set; } = "";
}

public class LoginDto
{
    [Required, EmailAddress]
    public string Email { get; set; } = "";

    [Required]
    public string Password { get; set; } = "";
}