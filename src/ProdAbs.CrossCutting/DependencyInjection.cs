using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ProdAbs.Application;
using ProdAbs.Infrastructure;

namespace ProdAbs.CrossCutting
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddCrossCutting(this IServiceCollection services)
        {
            services.AddApplication();

            services.AddInfrastructureServices();

            return services;
        }
    }
}