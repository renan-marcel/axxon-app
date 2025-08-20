using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using ProdAbs.Domain.Entities;

namespace ProdAbs.Infrastructure.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<TipoDocumento> TiposDeDocumento { get; set; }
    public DbSet<Documento> Documentos { get; set; }
    public DbSet<Prontuario> Prontuarios { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Documento>()
            .Property(d => d.DicionarioDeCamposValores)
            .HasConversion(
                v => JsonSerializer.Serialize(v, (JsonSerializerOptions)null),
                v => JsonSerializer.Deserialize<Dictionary<string, string>>(v, (JsonSerializerOptions)null));

        modelBuilder.Entity<TipoDocumento>().OwnsMany(t => t.Campos, a =>
        {
            a.WithOwner().HasForeignKey("TipoDocumentoId");
            a.Property<int>("Id");
            a.HasKey("Id");
            a.OwnsOne(c => c.RegraDeValidacao);
        });
    }
}