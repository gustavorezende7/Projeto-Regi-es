using Microsoft.EntityFrameworkCore;
using RegioesApi.Models;

namespace RegioesApi.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Regiao> Regioes { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Regiao>(entity =>
            {
                entity.HasKey(r => r.Id);
                entity.Property(r => r.UF).IsRequired().HasMaxLength(2);
                entity.Property(r => r.Nome).IsRequired().HasMaxLength(200);
                entity.Property(r => r.Ativo).HasDefaultValue(true);
                entity.HasIndex(r => new { r.UF, r.Nome }).IsUnique();
            });
        }
    }
}
