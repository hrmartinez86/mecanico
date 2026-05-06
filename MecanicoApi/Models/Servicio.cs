using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MecanicoApi.Models;

public enum TipoServicio
{
    Servicio,
    Reparacion,
    Revision,
    Diagnostico,
    Preventivo,
    Otro
}

public enum EstadoServicio
{
    Pendiente,
    EnProceso,
    Completado,
    Cancelado
}

public class Servicio
{
    public int Id { get; set; }

    [Required]
    public int AutoId { get; set; }

    public TipoServicio Tipo { get; set; } = TipoServicio.Servicio;

    [Required, MaxLength(150)]
    public string Titulo { get; set; } = string.Empty;

    [MaxLength(1000)]
    public string? Descripcion { get; set; }

    [MaxLength(1000)]
    public string? Diagnostico { get; set; }

    [MaxLength(1000)]
    public string? TrabajoRealizado { get; set; }

    /// <summary>
    /// Lista de piezas/refacciones usadas, almacenada como JSON
    /// Ejemplo: [{"nombre":"Filtro de aceite","cantidad":1,"precioUnitario":150.00}]
    /// </summary>
    public string? PiezasJson { get; set; }

    [Column(TypeName = "decimal(10,2)")]
    public decimal? CostoManoObra { get; set; }

    [Column(TypeName = "decimal(10,2)")]
    public decimal? CostoPiezas { get; set; }

    [Column(TypeName = "decimal(10,2)")]
    public decimal? CostoTotal { get; set; }

    [Required]
    public DateOnly Fecha { get; set; }

    public DateOnly? FechaTermino { get; set; }

    [MaxLength(100)]
    public string? Mecanico { get; set; }

    public EstadoServicio Estado { get; set; } = EstadoServicio.Pendiente;

    [Column(TypeName = "decimal(10,1)")]
    public decimal? KilometrajeEntrada { get; set; }

    [MaxLength(500)]
    public string? Observaciones { get; set; }

    public DateTime CreadoEn { get; set; } = DateTime.UtcNow;

    // Navegación
    [ForeignKey(nameof(AutoId))]
    public Auto? Auto { get; set; }
}
