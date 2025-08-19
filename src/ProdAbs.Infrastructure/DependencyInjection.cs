using System.Runtime;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using ProdAbs.Application.Interfaces;
using ProdAbs.Domain.Interfaces;
using ProdAbs.Infrastructure.Data;
using ProdAbs.Infrastructure.Data.Repositories;
using ProdAbs.Infrastructure.Services;

namespace ProdAbs.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
        {
            // Register Infrastructure Settings
            services.AddPooledDbContextFactory<AppDbContext>((serviceProvider, options) =>
            {
                var configuration = serviceProvider.GetRequiredService<IConfiguration>();
                var connectionStringSettings = configuration.GetConnectionString("ged_db");

                if (string.IsNullOrWhiteSpace(connectionStringSettings))
                {
                    throw new ArgumentException("Connection string for 'ged_db' is not configured.");
                }

                options.UseSqlite(connectionStringSettings);
            });

            services.AddScoped<IFileStorageService, LocalFileStorageService>();

            // Register Repositories
            services.AddScoped<ITipoDeDocumentoRepository, TipoDeDocumentoRepository>();
            services.AddScoped<IDocumentoRepository, DocumentoRepository>();
            services.AddScoped<IProntuarioRepository, ProntuarioRepository>();

            return services;
        }
    }
}