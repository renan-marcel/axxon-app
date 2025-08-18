
using Microsoft.EntityFrameworkCore;
using ProdAbs.Domain.Entities;

namespace ProdAbs.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<TipoDocumento> TiposDeDocumento { get; set; }
        public DbSet<Documento> Documentos { get; set; }
        public DbSet<Prontuario> Prontuarios { get; set; }
    }
}
