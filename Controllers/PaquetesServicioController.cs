using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VisualSync.Api.Data;
using VisualSync.Api.DTOs;
using VisualSync.Api.Models;

namespace VisualSync.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PaquetesServicioController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public PaquetesServicioController(ApplicationDbContext context) => _context = context;

    // PÚBLICO: catálogo de paquetes activos
    [HttpGet]
    public async Task<ActionResult<IEnumerable<PaqueteServicio>>> GetPaquetes() =>
        await _context.PaquetesServicio.Where(p => p.Activo).ToListAsync();

    // PÚBLICO: detalle de un paquete activo
    [HttpGet("{id:int}")]
    public async Task<ActionResult<PaqueteServicio>> GetPaquete(int id)
    {
        var paquete = await _context.PaquetesServicio.FindAsync(id);
        if (paquete == null || !paquete.Activo) return NotFound();
        return paquete;
    }

    // ADMIN: crear
    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<ActionResult<PaqueteServicio>> PostPaquete(PaqueteCrearDto dto)
    {
        var paquete = new PaqueteServicio
        {
            Nombre = dto.Nombre,
            Descripcion = dto.Descripcion,
            Precio = dto.Precio,
            Duracion = dto.Duracion,
            CantidadFotos = dto.CantidadFotos,
            IncluyeVideo = dto.IncluyeVideo,
            Activo = true
        };
        _context.PaquetesServicio.Add(paquete);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetPaquete), new { id = paquete.IdPaquete }, paquete);
    }

    // ADMIN: actualizar
    [Authorize(Roles = "Admin")]
    [HttpPut("{id:int}")]
    public async Task<ActionResult<PaqueteServicio>> PutPaquete(int id, PaqueteCrearDto dto)
    {
        var paquete = await _context.PaquetesServicio.FindAsync(id);
        if (paquete == null) return NotFound();

        paquete.Nombre = dto.Nombre;
        paquete.Descripcion = dto.Descripcion;
        paquete.Precio = dto.Precio;
        paquete.Duracion = dto.Duracion;
        paquete.CantidadFotos = dto.CantidadFotos;
        paquete.IncluyeVideo = dto.IncluyeVideo;

        await _context.SaveChangesAsync();
        return paquete;
    }

    // ADMIN: desactivar (baja lógica)
    [Authorize(Roles = "Admin")]
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeletePaquete(int id)
    {
        var paquete = await _context.PaquetesServicio.FindAsync(id);
        if (paquete == null) return NotFound();

        paquete.Activo = false;
        await _context.SaveChangesAsync();
        return NoContent();
    }
}