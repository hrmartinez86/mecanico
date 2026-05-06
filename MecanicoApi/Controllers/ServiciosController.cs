using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MecanicoApi.Data;
using MecanicoApi.DTOs;
using MecanicoApi.Models;

namespace MecanicoApi.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class ServiciosController : ControllerBase
{
    private readonly AppDbContext _db;

    public ServiciosController(AppDbContext db) => _db = db;

    // GET api/servicios?autoId=1&estado=EnProceso&tipo=Reparacion
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ServicioResponseDto>>> GetAll(
        [FromQuery] int? autoId,
        [FromQuery] EstadoServicio? estado,
        [FromQuery] TipoServicio? tipo)
    {
        var query = _db.Servicios
            .Include(s => s.Auto)
                .ThenInclude(a => a!.Cliente)
            .AsQueryable();

        if (autoId.HasValue)
            query = query.Where(s => s.AutoId == autoId.Value);

        if (estado.HasValue)
            query = query.Where(s => s.Estado == estado.Value);

        if (tipo.HasValue)
            query = query.Where(s => s.Tipo == tipo.Value);

        var servicios = await query
            .OrderByDescending(s => s.Fecha)
            .Select(s => ToDto(s))
            .ToListAsync();

        return Ok(servicios);
    }

    // GET api/servicios/5
    [HttpGet("{id:int}")]
    public async Task<ActionResult<ServicioResponseDto>> GetById(int id)
    {
        var servicio = await _db.Servicios
            .Include(s => s.Auto)
                .ThenInclude(a => a!.Cliente)
            .FirstOrDefaultAsync(s => s.Id == id);

        if (servicio is null) return NotFound(new { message = "Servicio no encontrado" });
        return Ok(ToDto(servicio));
    }

    // GET api/servicios/auto/{autoId}/historial
    [HttpGet("auto/{autoId:int}/historial")]
    public async Task<ActionResult<IEnumerable<ServicioResponseDto>>> GetHistorialByAuto(int autoId)
    {
        if (!await _db.Autos.AnyAsync(a => a.Id == autoId))
            return NotFound(new { message = "Auto no encontrado" });

        var servicios = await _db.Servicios
            .Include(s => s.Auto)
                .ThenInclude(a => a!.Cliente)
            .Where(s => s.AutoId == autoId)
            .OrderByDescending(s => s.Fecha)
            .Select(s => ToDto(s))
            .ToListAsync();

        return Ok(servicios);
    }

    // POST api/servicios
    [HttpPost]
    public async Task<ActionResult<ServicioResponseDto>> Create(ServicioCreateDto dto)
    {
        if (!await _db.Autos.AnyAsync(a => a.Id == dto.AutoId))
            return BadRequest(new { message = "El auto indicado no existe" });

        var servicio = new Servicio
        {
            AutoId = dto.AutoId,
            Tipo = dto.Tipo,
            Titulo = dto.Titulo,
            Descripcion = dto.Descripcion,
            Diagnostico = dto.Diagnostico,
            TrabajoRealizado = dto.TrabajoRealizado,
            PiezasJson = dto.PiezasJson,
            CostoManoObra = dto.CostoManoObra,
            CostoPiezas = dto.CostoPiezas,
            CostoTotal = dto.CostoTotal,
            Fecha = dto.Fecha,
            FechaTermino = dto.FechaTermino,
            Mecanico = dto.Mecanico,
            Estado = dto.Estado,
            KilometrajeEntrada = dto.KilometrajeEntrada,
            Observaciones = dto.Observaciones
        };

        _db.Servicios.Add(servicio);
        await _db.SaveChangesAsync();

        await _db.Entry(servicio).Reference(s => s.Auto).LoadAsync();
        await _db.Entry(servicio.Auto!).Reference(a => a.Cliente).LoadAsync();

        return CreatedAtAction(nameof(GetById), new { id = servicio.Id }, ToDto(servicio));
    }

    // PUT api/servicios/5
    [HttpPut("{id:int}")]
    public async Task<ActionResult<ServicioResponseDto>> Update(int id, ServicioUpdateDto dto)
    {
        var servicio = await _db.Servicios
            .Include(s => s.Auto)
                .ThenInclude(a => a!.Cliente)
            .FirstOrDefaultAsync(s => s.Id == id);

        if (servicio is null) return NotFound(new { message = "Servicio no encontrado" });

        servicio.Tipo = dto.Tipo;
        servicio.Titulo = dto.Titulo;
        servicio.Descripcion = dto.Descripcion;
        servicio.Diagnostico = dto.Diagnostico;
        servicio.TrabajoRealizado = dto.TrabajoRealizado;
        servicio.PiezasJson = dto.PiezasJson;
        servicio.CostoManoObra = dto.CostoManoObra;
        servicio.CostoPiezas = dto.CostoPiezas;
        servicio.CostoTotal = dto.CostoTotal;
        servicio.Fecha = dto.Fecha;
        servicio.FechaTermino = dto.FechaTermino;
        servicio.Mecanico = dto.Mecanico;
        servicio.Estado = dto.Estado;
        servicio.KilometrajeEntrada = dto.KilometrajeEntrada;
        servicio.Observaciones = dto.Observaciones;

        await _db.SaveChangesAsync();
        return Ok(ToDto(servicio));
    }

    // DELETE api/servicios/5
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var servicio = await _db.Servicios.FindAsync(id);
        if (servicio is null) return NotFound(new { message = "Servicio no encontrado" });

        _db.Servicios.Remove(servicio);
        await _db.SaveChangesAsync();
        return NoContent();
    }

    // ─── Mapeo ───────────────────────────────────────────────────────────────

    private static ServicioResponseDto ToDto(Servicio s) => new(
        s.Id,
        s.AutoId,
        s.Auto is not null ? $"{s.Auto.Anio} {s.Auto.Marca} {s.Auto.Modelo}" : string.Empty,
        s.Auto?.Cliente is not null ? $"{s.Auto.Cliente.Nombre} {s.Auto.Cliente.Apellido}" : string.Empty,
        s.Tipo,
        s.Titulo,
        s.Descripcion,
        s.Diagnostico,
        s.TrabajoRealizado,
        s.PiezasJson,
        s.CostoManoObra,
        s.CostoPiezas,
        s.CostoTotal,
        s.Fecha,
        s.FechaTermino,
        s.Mecanico,
        s.Estado,
        s.KilometrajeEntrada,
        s.Observaciones,
        s.CreadoEn
    );
}
