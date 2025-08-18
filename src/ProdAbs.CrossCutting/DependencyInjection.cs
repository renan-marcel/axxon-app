
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ProdAbs.Application.Interfaces;
using ProdAbs.Domain.Interfaces;
using ProdAbs.Infrastructure.Data;
using ProdAbs.Infrastructure.Data.Repositories;
using ProdAbs.Infrastructure.Services;

namespace ProdAbs.CrossCutting
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            // Para o MVP, usando banco de dados em memória (SQLite)
            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlite("Data Source=ProdAbs.db"));

            services.AddScoped<IFileStorageService, LocalFileStorageService>();

            services.AddScoped<IDocumentoRepository, DocumentoRepository>();
            services.AddScoped<ITipoDeDocumentoRepository, TipoDeDocumentoRepository>();
            services.AddScoped<IProntuarioRepository, ProntuarioRepository>();

            return services;
        }
    }
}
