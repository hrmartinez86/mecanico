using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MecanicoApi.Data;
using MecanicoApi.DTOs;
using MecanicoApi.Models;

namespace MecanicoApi.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class AutosController : ControllerBase
{
    private readonly AppDbContext _db;

    public AutosController(AppDbContext db) => _db = db;

    // GET api/autos?clienteId=1&search=civic
    [HttpGet]
    public async Task<ActionResult<IEnumerable<AutoResponseDto>>> GetAll(
        [FromQuery] int? clienteId,
        [FromQuery] string? search)
    {
        var query = _db.Autos
            .Include(a => a.Cliente)
            .Include(a => a.Servicios)
            .AsQueryable();

        if (clienteId.HasValue)
            query = query.Where(a => a.ClienteId == clienteId.Value);

        if (!string.IsNullOrWhiteSpace(search))
        {
            var s = search.Trim().ToLower();
            query = query.Where(a =>
                a.Marca.ToLower().Contains(s) ||
                a.Modelo.ToLower().Contains(s) ||
                (a.Placa != null && a.Placa.ToLower().Contains(s)) ||
                (a.Vin != null && a.Vin.ToLower().Contains(s)));
        }

        var autos = await query
            .OrderByDescending(a => a.CreadoEn)
            .Select(a => ToDto(a))
            .ToListAsync();

        return Ok(autos);
    }

    // GET api/autos/5
    [HttpGet("{id:int}")]
    public async Task<ActionResult<AutoResponseDto>> GetById(int id)
    {
        var auto = await _db.Autos
            .Include(a => a.Cliente)
            .Include(a => a.Servicios)
            .FirstOrDefaultAsync(a => a.Id == id);

        if (auto is null) return NotFound(new { message = "Auto no encontrado" });
        return Ok(ToDto(auto));
    }

    // POST api/autos
    [HttpPost]
    public async Task<ActionResult<AutoResponseDto>> Create(AutoCreateDto dto)
    {
        if (!await _db.Clientes.AnyAsync(c => c.Id == dto.ClienteId))
            return BadRequest(new { message = "El cliente indicado no existe" });

        if (dto.Placa is not null && await _db.Autos.AnyAsync(a => a.Placa == dto.Placa))
            return Conflict(new { message = "Ya existe un auto con esa placa" });

        var auto = new Auto
        {
            ClienteId = dto.ClienteId,
            Marca = dto.Marca,
            Modelo = dto.Modelo,
            Anio = dto.Anio,
            Color = dto.Color,
            Placa = dto.Placa,
            Vin = dto.Vin,
            Kilometraje = dto.Kilometraje,
            Notas = dto.Notas
        };

        _db.Autos.Add(auto);
        await _db.SaveChangesAsync();

        // Recargar con navegación para el response
        await _db.Entry(auto).Reference(a => a.Cliente).LoadAsync();

        return CreatedAtAction(nameof(GetById), new { id = auto.Id }, ToDto(auto));
    }

    // PUT api/autos/5
    [HttpPut("{id:int}")]
    public async Task<ActionResult<AutoResponseDto>> Update(int id, AutoUpdateDto dto)
    {
        var auto = await _db.Autos
            .Include(a => a.Cliente)
            .Include(a => a.Servicios)
            .FirstOrDefaultAsync(a => a.Id == id);

        if (auto is null) return NotFound(new { message = "Auto no encontrado" });

        if (!await _db.Clientes.AnyAsync(c => c.Id == dto.ClienteId))
            return BadRequest(new { message = "El cliente indicado no existe" });

        if (dto.Placa is not null && await _db.Autos.AnyAsync(a => a.Placa == dto.Placa && a.Id != id))
            return Conflict(new { message = "Ya existe otro auto con esa placa" });

        auto.ClienteId = dto.ClienteId;
        auto.Marca = dto.Marca;
        auto.Modelo = dto.Modelo;
        auto.Anio = dto.Anio;
        auto.Color = dto.Color;
        auto.Placa = dto.Placa;
        auto.Vin = dto.Vin;
        auto.Kilometraje = dto.Kilometraje;
        auto.Notas = dto.Notas;

        await _db.SaveChangesAsync();
        await _db.Entry(auto).Reference(a => a.Cliente).LoadAsync();

        return Ok(ToDto(auto));
    }

    // DELETE api/autos/5
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var auto = await _db.Autos.FindAsync(id);
        if (auto is null) return NotFound(new { message = "Auto no encontrado" });

        _db.Autos.Remove(auto);
        await _db.SaveChangesAsync();
        return NoContent();
    }

    // ─── Mapeo ───────────────────────────────────────────────────────────────

    private static AutoResponseDto ToDto(Auto a) => new(
        a.Id,
        a.ClienteId,
        a.Cliente is not null ? $"{a.Cliente.Nombre} {a.Cliente.Apellido}" : string.Empty,
        a.Marca,
        a.Modelo,
        a.Anio,
        a.Color,
        a.Placa,
        a.Vin,
        a.Kilometraje,
        a.Notas,
        a.CreadoEn,
        a.Servicios.Count
    );
}
