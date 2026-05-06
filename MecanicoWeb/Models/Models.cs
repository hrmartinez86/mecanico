using System.ComponentModel.DataAnnotations;

namespace MecanicoWeb.Models;

// ─── Enums ───────────────────────────────────────────────────────────────────

public enum TipoServicio
{
    Servicio, Reparacion, Revision, Diagnostico, Preventivo, Otro
}

public enum EstadoServicio
{
    Pendiente, EnProceso, Completado, Cancelado
}

// ─── Cliente ─────────────────────────────────────────────────────────────────

public class ClienteResponse
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string Apellido { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? Telefono { get; set; }
    public string? Direccion { get; set; }
    public DateTime CreadoEn { get; set; }
    public int TotalAutos { get; set; }
    public string NombreCompleto => $"{Nombre} {Apellido}";
}

public class ClienteForm
{
    [Required(ErrorMessage = "El nombre es requerido")]
    [MaxLength(100)]
    public string Nombre { get; set; } = string.Empty;

    [Required(ErrorMessage = "El apellido es requerido")]
    [MaxLength(100)]
    public string Apellido { get; set; } = string.Empty;

    [Required(ErrorMessage = "El email es requerido")]
    [EmailAddress(ErrorMessage = "Email no válido")]
    [MaxLength(150)]
    public string Email { get; set; } = string.Empty;

    [MaxLength(20)]
    public string? Telefono { get; set; }

    [MaxLength(250)]
    public string? Direccion { get; set; }
}

// ─── Auto ─────────────────────────────────────────────────────────────────────

public class AutoResponse
{
    public int Id { get; set; }
    public int ClienteId { get; set; }
    public string NombreCliente { get; set; } = string.Empty;
    public string Marca { get; set; } = string.Empty;
    public string Modelo { get; set; } = string.Empty;
    public int Anio { get; set; }
    public string? Color { get; set; }
    public string? Placa { get; set; }
    public string? Vin { get; set; }
    public decimal? Kilometraje { get; set; }
    public string? Notas { get; set; }
    public DateTime CreadoEn { get; set; }
    public int TotalServicios { get; set; }
    public string Descripcion => $"{Anio} {Marca} {Modelo}";
}

public class AutoForm
{
    [Required(ErrorMessage = "Seleccione un cliente")]
    [Range(1, int.MaxValue, ErrorMessage = "Seleccione un cliente")]
    public int ClienteId { get; set; }

    [Required(ErrorMessage = "La marca es requerida")]
    [MaxLength(60)]
    public string Marca { get; set; } = string.Empty;

    [Required(ErrorMessage = "El modelo es requerido")]
    [MaxLength(60)]
    public string Modelo { get; set; } = string.Empty;

    [Range(1900, 2100, ErrorMessage = "Año inválido")]
    public int Anio { get; set; } = DateTime.Now.Year;

    [MaxLength(30)]
    public string? Color { get; set; }

    [MaxLength(20)]
    public string? Placa { get; set; }

    [MaxLength(50)]
    public string? Vin { get; set; }

    public decimal? Kilometraje { get; set; }

    [MaxLength(500)]
    public string? Notas { get; set; }
}

// ─── Servicio ─────────────────────────────────────────────────────────────────

public class ServicioResponse
{
    public int Id { get; set; }
    public int AutoId { get; set; }
    public string AutoDescripcion { get; set; } = string.Empty;
    public string NombreCliente { get; set; } = string.Empty;
    public TipoServicio Tipo { get; set; }
    public string Titulo { get; set; } = string.Empty;
    public string? Descripcion { get; set; }
    public string? Diagnostico { get; set; }
    public string? TrabajoRealizado { get; set; }
    public string? PiezasJson { get; set; }
    public decimal? CostoManoObra { get; set; }
    public decimal? CostoPiezas { get; set; }
    public decimal? CostoTotal { get; set; }
    public DateOnly Fecha { get; set; }
    public DateOnly? FechaTermino { get; set; }
    public string? Mecanico { get; set; }
    public EstadoServicio Estado { get; set; }
    public decimal? KilometrajeEntrada { get; set; }
    public string? Observaciones { get; set; }
    public DateTime CreadoEn { get; set; }
}

public class ServicioForm
{
    [Required(ErrorMessage = "El auto es requerido")]
    [Range(1, int.MaxValue, ErrorMessage = "Seleccione un auto")]
    public int AutoId { get; set; }

    public TipoServicio Tipo { get; set; } = TipoServicio.Servicio;

    [Required(ErrorMessage = "El título es requerido")]
    [MaxLength(150)]
    public string Titulo { get; set; } = string.Empty;

    [MaxLength(1000)]
    public string? Descripcion { get; set; }

    [MaxLength(1000)]
    public string? Diagnostico { get; set; }

    [MaxLength(1000)]
    public string? TrabajoRealizado { get; set; }

    public decimal? CostoManoObra { get; set; }
    public decimal? CostoPiezas { get; set; }
    public decimal? CostoTotal { get; set; }

    [Required(ErrorMessage = "La fecha es requerida")]
    public string Fecha { get; set; } = DateOnly.FromDateTime(DateTime.Today).ToString("yyyy-MM-dd");

    public string? FechaTermino { get; set; }

    [MaxLength(100)]
    public string? Mecanico { get; set; }

    public EstadoServicio Estado { get; set; } = EstadoServicio.Pendiente;

    public decimal? KilometrajeEntrada { get; set; }

    [MaxLength(500)]
    public string? Observaciones { get; set; }
}
