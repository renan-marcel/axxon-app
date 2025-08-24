using Microsoft.EntityFrameworkCore;
using ProdAbs.Domain.Entities;
using ProdAbs.Domain.ValueObjects;

namespace ProdAbs.Infrastructure.Data;

public class DbInitializer
{
    private readonly AppDbContext _context;

    public DbInitializer(AppDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task InitializeAsync()
    {
        // Apply pending migrations
        await _context.Database.MigrateAsync();

        // Seed minimal data if necessary
        if (!await _context.TiposDeDocumento.AnyAsync())
        {
            var tipo = new TipoDocumento(Guid.NewGuid(), "Documento Padrão", new List<CampoMetadata>
            {
                new("Campo1", new RegraValidacao(TipoDeDados.String, false, string.Empty), string.Empty)
            });

            _context.TiposDeDocumento.Add(tipo);
            await _context.SaveChangesAsync();
        }
    }
}