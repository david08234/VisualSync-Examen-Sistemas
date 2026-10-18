using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VisualSync.Api.Data;
using VisualSync.Api.Models;

namespace VisualSync.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ArchivosGaleriaController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public ArchivosGaleriaController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ArchivoGaleria>>> GetArchivos()
    {
        return await _context.ArchivosGaleria.ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ArchivoGaleria>> GetArchivo(int id)
    {
        var archivo = await _context.ArchivosGaleria.FindAsync(id);
        if (archivo == null) return NotFound();
        return archivo;
    }

    [HttpPost]
    public async Task<ActionResult<ArchivoGaleria>> PostArchivo(ArchivoGaleria archivoGaleria)
    {
        _context.ArchivosGaleria.Add(archivoGaleria);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetArchivo), new { id = archivoGaleria.IdArchivo }, archivoGaleria);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> PutArchivo(int id, ArchivoGaleria archivoGaleria)
    {
        if (id != archivoGaleria.IdArchivo) return BadRequest();

        _context.Entry(archivoGaleria).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!_context.ArchivosGaleria.Any(e => e.IdArchivo == id)) return NotFound();
            else throw;
        }

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteArchivo(int id)
    {
        var archivo = await _context.ArchivosGaleria.FindAsync(id);
        if (archivo == null) return NotFound();

        _context.ArchivosGaleria.Remove(archivo);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}