using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System.IO;
using System.Text.Json;

namespace ProdAbs.Infrastructure.Data;

// Factory usada em design-time por ferramentas EF (migrations)
public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
{
    public AppDbContext CreateDbContext(string[] args)
    {
        // Tentativa simples: ler appsettings.Development.json ou appsettings.json e extrair connection string 'gedDb'
        string basePath = Directory.GetCurrentDirectory();
        string[] candidates = new[] { "appsettings.Development.json", "appsettings.json" };
        string? json = null;

        foreach (var c in candidates)
        {
            var path = Path.Combine(basePath, c);
            if (File.Exists(path))
            {
                json = File.ReadAllText(path);
                break;
            }
        }

        string? conn = null;
        if (!string.IsNullOrEmpty(json))
        {
            using var doc = JsonDocument.Parse(json);
            if (doc.RootElement.TryGetProperty("ConnectionStrings", out var cs) &&
                cs.ValueKind == JsonValueKind.Object &&
                cs.TryGetProperty("gedDb", out var ged))
            {
                conn = ged.GetString();
            }
        }

        // fallback to environment variable
        conn ??= Environment.GetEnvironmentVariable("GEDDB_CONNECTION") ?? Environment.GetEnvironmentVariable("gedDb");

        if (string.IsNullOrEmpty(conn))
            throw new InvalidOperationException("Connection string 'gedDb' not found (checked appsettings and environment variables).");

        var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
        optionsBuilder.UseNpgsql(conn);

        return new AppDbContext(optionsBuilder.Options);
    }
}
