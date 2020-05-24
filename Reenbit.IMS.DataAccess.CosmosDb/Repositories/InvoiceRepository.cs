using Microsoft.Extensions.Configuration;
using Reenbit.IMS.DataAccess.Abstraction;
using Reenbit.IMS.DataAccess.CosmosDb.CosmosDbClient;
using Reenbit.IMS.Domain.Documents;

namespace Reenbit.IMS.DataAccess.CosmosDb.Repositories
{
    public class InvoiceRepository : DocumentRepositoryBase<Invoice>, IInvoiceRepository
    {
        public InvoiceRepository(
           IConfiguration configuration,
           ICosmosDbClient client)
           : base(configuration, client)
        {
        }

        protected override string CollectionName => this.Configuration["CosmosDb:InvoicesCollectionName"];
    }
}