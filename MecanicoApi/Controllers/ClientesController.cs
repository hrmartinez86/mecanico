using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MecanicoApi.Data;
using MecanicoApi.DTOs;
using MecanicoApi.Models;

namespace MecanicoApi.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class ClientesController : ControllerBase
{
    private readonly AppDbContext _db;

    public ClientesController(AppDbContext db) => _db = db;

    // GET api/clientes?search=...
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ClienteResponseDto>>> GetAll([FromQuery] string? search)
    {
        var query = _db.Clientes.Include(c => c.Autos).AsQueryable();

        if (!string.IsNullOrWhiteSpace(search))
        {
            var s = search.Trim().ToLower();
            query = query.Where(c =>
                c.Nombre.ToLower().Contains(s) ||
                c.Apellido.ToLower().Contains(s) ||
                c.Email.ToLower().Contains(s) ||
                (c.Telefono != null && c.Telefono.Contains(s)));
        }

        var clientes = await query
            .OrderBy(c => c.Apellido)
            .ThenBy(c => c.Nombre)
            .Select(c => ToDto(c))
            .ToListAsync();

        return Ok(clientes);
    }

    // GET api/clientes/5
    [HttpGet("{id:int}")]
    public async Task<ActionResult<ClienteResponseDto>> GetById(int id)
    {
        var cliente = await _db.Clientes
            .Include(c => c.Autos)
            .FirstOrDefaultAsync(c => c.Id == id);

        if (cliente is null) return NotFound(new { message = "Cliente no encontrado" });
        return Ok(ToDto(cliente));
    }

    // POST api/clientes
    [HttpPost]
    public async Task<ActionResult<ClienteResponseDto>> Create(ClienteCreateDto dto)
    {
        if (await _db.Clientes.AnyAsync(c => c.Email == dto.Email))
            return Conflict(new { message = "Ya existe un cliente con ese email" });

        var cliente = new Cliente
        {
            Nombre = dto.Nombre,
            Apellido = dto.Apellido,
            Email = dto.Email,
            Telefono = dto.Telefono,
            Direccion = dto.Direccion
        };

        _db.Clientes.Add(cliente);
        await _db.SaveChangesAsync();

        return CreatedAtAction(nameof(GetById), new { id = cliente.Id }, ToDto(cliente));
    }

    // PUT api/clientes/5
    [HttpPut("{id:int}")]
    public async Task<ActionResult<ClienteResponseDto>> Update(int id, ClienteUpdateDto dto)
    {
        var cliente = await _db.Clientes.Include(c => c.Autos).FirstOrDefaultAsync(c => c.Id == id);
        if (cliente is null) return NotFound(new { message = "Cliente no encontrado" });

        if (await _db.Clientes.AnyAsync(c => c.Email == dto.Email && c.Id != id))
            return Conflict(new { message = "Ya existe otro cliente con ese email" });

        cliente.Nombre = dto.Nombre;
        cliente.Apellido = dto.Apellido;
        cliente.Email = dto.Email;
        cliente.Telefono = dto.Telefono;
        cliente.Direccion = dto.Direccion;

        await _db.SaveChangesAsync();
        return Ok(ToDto(cliente));
    }

    // DELETE api/clientes/5
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var cliente = await _db.Clientes.FindAsync(id);
        if (cliente is null) return NotFound(new { message = "Cliente no encontrado" });

        _db.Clientes.Remove(cliente);
        await _db.SaveChangesAsync();
        return NoContent();
    }

    // ─── Mapeo ───────────────────────────────────────────────────────────────

    private static ClienteResponseDto ToDto(Cliente c) => new(
        c.Id,
        c.Nombre,
        c.Apellido,
        c.Email,
        c.Telefono,
        c.Direccion,
        c.CreadoEn,
        c.Autos.Count
    );
}
