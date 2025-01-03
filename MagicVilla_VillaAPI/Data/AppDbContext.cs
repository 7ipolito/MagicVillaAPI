using Microsoft.EntityFrameworkCore;
using MagicVilla_VillaApi.Models;

namespace MagicVilla_VillaApi.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    // DbSet para o modelo Villa
    public DbSet<Villa> Villas { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configuração adicional para o modelo Villa
        modelBuilder.Entity<Villa>(entity =>
        {
            // Configuração de valor padrão para CreateDate
            entity.Property(v => v.CreateDate)
                  .HasDefaultValueSql("NOW()"); // PostgreSQL: NOW() preenche com a data atual

            // Configuração de validações e limites
            entity.Property(v => v.Name)
                  .IsRequired()
                  .HasMaxLength(100); // Limite de 100 caracteres no nome da vila

            entity.Property(v => v.Sqft)
                  .IsRequired();

            entity.Property(v => v.Occupancy)
                  .IsRequired();
        });
    }
}
