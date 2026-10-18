using System.ComponentModel.DataAnnotations;

namespace VisualSync.Api.Models;

public class ArchivoGaleria
{
    [Key]
    public int IdArchivo { get; set; }

    public int IdReserva { get; set; }
    public Reserva Reserva { get; set; } = null!;

    [Required]
    [MaxLength(300)]
    public string RutaBajaRes { get; set; } = null!;

    [Required]
    [MaxLength(300)]
    public string RutaAltaRes { get; set; } = null!;

    [Required]
    [MaxLength(10)]
    public string TipoArchivo { get; set; } = null!;

    public bool SeleccionadaPorCliente { get; set; } = false;
}