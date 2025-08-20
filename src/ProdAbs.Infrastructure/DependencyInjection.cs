using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ProdAbs.Application.Interfaces;
using ProdAbs.Domain.Interfaces;
using ProdAbs.Infrastructure.Data;
using ProdAbs.Infrastructure.Data.Repositories;
using ProdAbs.Infrastructure.Messaging;
using ProdAbs.Infrastructure.Services;
using ProdAbs.SharedKernel.Events;

namespace ProdAbs.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services,
        IConfiguration configuration)
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
                throw new InvalidOperationException("Storage provider is not configured.");

            switch (storageProvider)
            {
                case "Azure":
                    return new AzureBlobStorageService(configuration);
                default:
                    return new LocalFileStorageService(configuration);
            }
        });

        // Register application external services
        services.AddScoped<IEmailNotifier, LocalEmailNotifier>();
        services.AddScoped<IAuditLogger, SimpleAuditLogger>();

        services.AddMassTransit(busConfig =>
        {
            busConfig.UsingInMemory((context, cfg) => cfg.ConfigureEndpoints(context));

            busConfig.AddRider(rider =>
            {
                rider.AddProducer<Guid, IDocumentoCriadoEvent>("documento-criado-topic");
                rider.AddConsumer<DocumentoCriadoConsumer>();

                rider.UsingKafka((context, k) =>
                {
                    k.Host(configuration.GetConnectionString("kafka"));


                    k.TopicEndpoint<IDocumentoCriadoEvent>("documento-criado-topic", "consumer-group-name", e =>
                    {
                        e.ConfigureConsumer<DocumentoCriadoConsumer>(context);
                        e.AutoStart = true;
                    });
                });
            });
        });

        return services;
    }
}