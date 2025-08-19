using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
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
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<AppDbContext>(options =>
                options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

            services.AddScoped<ITipoDeDocumentoRepository, TipoDeDocumentoRepository>();
            services.AddScoped<IDocumentoRepository, DocumentoRepository>();
            services.AddScoped<IProntuarioRepository, ProntuarioRepository>();

            services.AddScoped<IFileStorageService, LocalFileStorageService>();

            return services;
        }
    }
}
