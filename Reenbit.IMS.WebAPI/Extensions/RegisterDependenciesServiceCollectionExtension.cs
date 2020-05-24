using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Reenbit.IMS.DataAccess.Abstraction;
using Reenbit.IMS.DataAccess.CosmosDb.CosmosDbClient;
using Reenbit.IMS.DataAccess.CosmosDb.Repositories;
using Reenbit.IMS.Services;
using Reenbit.IMS.Services.Abstraction;

namespace Reenbit.IMS.WebAPI.Extensions
{
    public static class RegisterDependenciesServiceCollectionExtension
    {
        public static void RegisterDependencies(this IServiceCollection services, IConfiguration configuration)
        {
            RegisterDataAccess(services, configuration);

            RegisterBusinessServices(services, configuration);
        }

        private static void RegisterDataAccess(IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<ICosmosDbClient, CosmosDbClient>(
                serviceProvider => new CosmosDbClient(new DocumentClientConfiguration {
                    DocumentDbKey = configuration["CosmosDb:AccountKey"],
                    DocumentDbUri = configuration["CosmosDb:AccountUri"]
                }));
            
            services.AddTransient<IInvoiceRepository, InvoiceRepository>();
        }

        private static void RegisterBusinessServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient<IDocumentChangeDetector, DocumentChangeDetector>();
            services.AddTransient<IInvoiceService, InvoiceService>();
        }
    }
}
