using System.ComponentModel.DataAnnotations;
using MecanicoApi.Models;

namespace MecanicoApi.DTOs;

// ─── Cliente ────────────────────────────────────────────────────────────────

public record ClienteCreateDto(
    [Required, MaxLength(100)] string Nombre,
    [Required, MaxLength(100)] string Apellido,
    [Required, EmailAddress, MaxLength(150)] string Email,
    [MaxLength(20)] string? Telefono,
    [MaxLength(250)] string? Direccion
);

public record ClienteUpdateDto(
    [Required, MaxLength(100)] string Nombre,
    [Required, MaxLength(100)] string Apellido,
    [Required, EmailAddress, MaxLength(150)] string Email,
    [MaxLength(20)] string? Telefono,
    [MaxLength(250)] string? Direccion
);

public record ClienteResponseDto(
    int Id,
    string Nombre,
    string Apellido,
    string Email,
    string? Telefono,
    string? Direccion,
    DateTime CreadoEn,
    int TotalAutos
);

// ─── Auto ────────────────────────────────────────────────────────────────────

public record AutoCreateDto(
    [Required] int ClienteId,
    [Required, MaxLength(60)] string Marca,
    [Required, MaxLength(60)] string Modelo,
    [Range(1900, 2100)] int Anio,
    [MaxLength(30)] string? Color,
    [MaxLength(20)] string? Placa,
    [MaxLength(50)] string? Vin,
    decimal? Kilometraje,
    [MaxLength(500)] string? Notas
);

public record AutoUpdateDto(
    [Required] int ClienteId,
    [Required, MaxLength(60)] string Marca,
    [Required, MaxLength(60)] string Modelo,
    [Range(1900, 2100)] int Anio,
    [MaxLength(30)] string? Color,
    [MaxLength(20)] string? Placa,
    [MaxLength(50)] string? Vin,
    decimal? Kilometraje,
    [MaxLength(500)] string? Notas
);

public record AutoResponseDto(
    int Id,
    int ClienteId,
    string NombreCliente,
    string Marca,
    string Modelo,
    int Anio,
    string? Color,
    string? Placa,
    string? Vin,
    decimal? Kilometraje,
    string? Notas,
    DateTime CreadoEn,
    int TotalServicios
);

// ─── Servicio ────────────────────────────────────────────────────────────────

public record ServicioCreateDto(
    [Required] int AutoId,
    TipoServicio Tipo,
    [Required, MaxLength(150)] string Titulo,
    [MaxLength(1000)] string? Descripcion,
    [MaxLength(1000)] string? Diagnostico,
    [MaxLength(1000)] string? TrabajoRealizado,
    string? PiezasJson,
    decimal? CostoManoObra,
    decimal? CostoPiezas,
    decimal? CostoTotal,
    [Required] DateOnly Fecha,
    DateOnly? FechaTermino,
    [MaxLength(100)] string? Mecanico,
    EstadoServicio Estado,
    decimal? KilometrajeEntrada,
    [MaxLength(500)] string? Observaciones
);

public record ServicioUpdateDto(
    TipoServicio Tipo,
    [Required, MaxLength(150)] string Titulo,
    [MaxLength(1000)] string? Descripcion,
    [MaxLength(1000)] string? Diagnostico,
    [MaxLength(1000)] string? TrabajoRealizado,
    string? PiezasJson,
    decimal? CostoManoObra,
    decimal? CostoPiezas,
    decimal? CostoTotal,
    [Required] DateOnly Fecha,
    DateOnly? FechaTermino,
    [MaxLength(100)] string? Mecanico,
    EstadoServicio Estado,
    decimal? KilometrajeEntrada,
    [MaxLength(500)] string? Observaciones
);

public record ServicioResponseDto(
    int Id,
    int AutoId,
    string AutoDescripcion,
    string NombreCliente,
    TipoServicio Tipo,
    string Titulo,
    string? Descripcion,
    string? Diagnostico,
    string? TrabajoRealizado,
    string? PiezasJson,
    decimal? CostoManoObra,
    decimal? CostoPiezas,
    decimal? CostoTotal,
    DateOnly Fecha,
    DateOnly? FechaTermino,
    string? Mecanico,
    EstadoServicio Estado,
    decimal? KilometrajeEntrada,
    string? Observaciones,
    DateTime CreadoEn
);
