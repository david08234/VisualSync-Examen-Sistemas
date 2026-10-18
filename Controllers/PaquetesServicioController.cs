using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VisualSync.Api.Data;
using VisualSync.Api.Models;

namespace VisualSync.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PaquetesServicioController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public PaquetesServicioController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<PaqueteServicio>>> GetPaquetes()
    {
        return await _context.PaquetesServicio.ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<PaqueteServicio>> GetPaquete(int id)
    {
        var paquete = await _context.PaquetesServicio.FindAsync(id);
        if (paquete == null) return NotFound();
        return paquete;
    }

    [HttpPost]
    public async Task<ActionResult<PaqueteServicio>> PostPaquete(PaqueteServicio paquete)
    {
        _context.PaquetesServicio.Add(paquete);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetPaquete), new { id = paquete.IdPaquete }, paquete);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> PutPaquete(int id, PaqueteServicio paquete)
    {
        if (id != paquete.IdPaquete) return BadRequest();

        _context.Entry(paquete).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!_context.PaquetesServicio.Any(e => e.IdPaquete == id)) return NotFound();
            else throw;
        }

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletePaquete(int id)
    {
        var paquete = await _context.PaquetesServicio.FindAsync(id);
        if (paquete == null) return NotFound();

        _context.PaquetesServicio.Remove(paquete);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}