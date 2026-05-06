using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MecanicoApi.Models;

public class Auto
{
    public int Id { get; set; }

    [Required]
    public int ClienteId { get; set; }

    [Required, MaxLength(60)]
    public string Marca { get; set; } = string.Empty;

    [Required, MaxLength(60)]
    public string Modelo { get; set; } = string.Empty;

    [Range(1900, 2100)]
    public int Anio { get; set; }

    [MaxLength(30)]
    public string? Color { get; set; }

    [MaxLength(20)]
    public string? Placa { get; set; }

    [MaxLength(50)]
    public string? Vin { get; set; }

    [Column(TypeName = "decimal(10,1)")]
    public decimal? Kilometraje { get; set; }

    [MaxLength(500)]
    public string? Notas { get; set; }

    public DateTime CreadoEn { get; set; } = DateTime.UtcNow;

    // Navegación
    [ForeignKey(nameof(ClienteId))]
    public Cliente? Cliente { get; set; }

    public ICollection<Servicio> Servicios { get; set; } = new List<Servicio>();
}
