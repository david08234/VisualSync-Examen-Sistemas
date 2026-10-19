using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VisualSync.Api.Data;
using VisualSync.Api.Models;

namespace VisualSync.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ReservasController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public ReservasController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Reserva>>> GetReservas()
    {
        // Incluimos los datos del Usuario y el Paquete para que la respuesta sea más completa
        return await _context.Reservas
            .Include(r => r.Usuario)
            .Include(r => r.Paquete)
            .ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Reserva>> GetReserva(int id)
    {
        var reserva = await _context.Reservas
            .Include(r => r.Usuario)
            .Include(r => r.Paquete)
            .FirstOrDefaultAsync(r => r.IdReserva == id);

        if (reserva == null) return NotFound();
        return reserva;
    }

    [HttpPost]
    public async Task<ActionResult<Reserva>> PostReserva(Reserva reserva)
    {
        // 1. Validar si la fecha y hora ya están ocupadas por otra reserva activa
        bool fechaOcupada = await _context.Reservas
            .AnyAsync(r => r.FechaEvento == reserva.FechaEvento && r.EstadoReserva != "Cancelada");

        if (fechaOcupada)
        {
            // 2. Retorna el código 409 (Conflicto) que el frontend de React está esperando
            return Conflict(new { detail = "Ya existe una reserva para esta fecha y hora. Por favor, elige otro horario." });
        }

        // 3. Si la fecha está libre, guarda la reserva normalmente
        _context.Reservas.Add(reserva);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetReserva), new { id = reserva.IdReserva }, reserva);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> PutReserva(int id, Reserva reserva)
    {
        if (id != reserva.IdReserva) return BadRequest();

        _context.Entry(reserva).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!_context.Reservas.Any(e => e.IdReserva == id)) return NotFound();
            else throw;
        }

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteReserva(int id)
    {
        var reserva = await _context.Reservas.FindAsync(id);
        if (reserva == null) return NotFound();

        _context.Reservas.Remove(reserva);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}