using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ProdAbs.Application.Interfaces;
using ProdAbs.Domain.Interfaces;
using ProdAbs.Infrastructure.Data;
using ProdAbs.Infrastructure.Data.Repositories;
using ProdAbs.Infrastructure.Services;
using System;

namespace ProdAbs.CrossCutting
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<AppDbContext>(options =>
                options.UseNpgsql(configuration.GetConnectionString("gedDb")));

            services.AddScoped<ITipoDeDocumentoRepository, TipoDeDocumentoRepository>();
            services.AddScoped<IDocumentoRepository, DocumentoRepository>();
            services.AddScoped<IProntuarioRepository, ProntuarioRepository>();

            services.AddScoped<IFileStorageService>(provider =>
            {
                var storageProvider = configuration["StorageSettings:Provider"];
 
                if (string.IsNullOrEmpty(storageProvider))
                {
                    throw new InvalidOperationException("Storage provider is not configured.");
                }

                switch (storageProvider)
                {
                    case "Azure":
                        return new AzureBlobStorageService(configuration);
                    case "Local":
                    default:
                        return new LocalFileStorageService(configuration);
                }
            });

            return services;
        }
    }
}
