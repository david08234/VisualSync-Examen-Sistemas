using System.ComponentModel.DataAnnotations;

namespace VisualSync.Api.Models;

public class Reserva
{
    [Key]
    public int IdReserva { get; set; }

    public int IdUsuario { get; set; }
    public Usuario? Usuario { get; set; } // <- Cambio aquí

    public int IdPaquete { get; set; }
    public PaqueteServicio? Paquete { get; set; } // <- Cambio aquí

    public DateTime FechaEvento { get; set; }

    [Required]
    [MaxLength(200)]
    public string LugarEvento { get; set; } = null!;

    [Required]
    [MaxLength(30)]
    public string EstadoReserva { get; set; } = "Solicitada";

    [Required]
    [MaxLength(30)]
    public string EstadoPago { get; set; } = "SinComprobante";

    [MaxLength(300)]
    public string? RutaComprobanteQR { get; set; }

    [MaxLength(300)]
    public string? MotivoRechazo { get; set; }

    public bool DescargasHabilitadas { get; set; } = false;

    public ICollection<ArchivoGaleria> Archivos { get; set; } = new List<ArchivoGaleria>();
}