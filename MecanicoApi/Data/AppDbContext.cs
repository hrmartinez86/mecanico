using Microsoft.EntityFrameworkCore;
using MecanicoApi.Models;

namespace MecanicoApi.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Cliente> Clientes => Set<Cliente>();
    public DbSet<Auto> Autos => Set<Auto>();
    public DbSet<Servicio> Servicios => Set<Servicio>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Cliente: email único
        modelBuilder.Entity<Cliente>()
            .HasIndex(c => c.Email)
            .IsUnique();

        // Auto: placa única (cuando no es null)
        modelBuilder.Entity<Auto>()
            .HasIndex(a => a.Placa)
            .IsUnique()
            .HasFilter("Placa IS NOT NULL");

        // Auto → Cliente
        modelBuilder.Entity<Auto>()
            .HasOne(a => a.Cliente)
            .WithMany(c => c.Autos)
            .HasForeignKey(a => a.ClienteId)
            .OnDelete(DeleteBehavior.Cascade);

        // Servicio → Auto
        modelBuilder.Entity<Servicio>()
            .HasOne(s => s.Auto)
            .WithMany(a => a.Servicios)
            .HasForeignKey(s => s.AutoId)
            .OnDelete(DeleteBehavior.Cascade);

        // Enums como strings en SQLite
        modelBuilder.Entity<Servicio>()
            .Property(s => s.Tipo)
            .HasConversion<string>();

        modelBuilder.Entity<Servicio>()
            .Property(s => s.Estado)
            .HasConversion<string>();
    }
}
