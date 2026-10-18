using System.ComponentModel.DataAnnotations;

namespace VisualSync.Api.Models;

public class PaqueteServicio
{
    [Key]
    public int IdPaquete { get; set; }

    [Required]
    [MaxLength(100)]
    public string Nombre { get; set; } = null!;

    [Required]
    [MaxLength(500)]
    public string Descripcion { get; set; } = null!;

    public decimal Precio { get; set; }
    public int Duracion { get; set; }
    public int CantidadFotos { get; set; }
    public bool IncluyeVideo { get; set; }
    public bool Activo { get; set; } = true;

    public ICollection<Reserva> Reservas { get; set; } = new List<Reserva>();
}