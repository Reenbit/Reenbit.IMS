using Microsoft.Azure.Documents;
using Microsoft.Extensions.Configuration;
using Reenbit.IMS.DataAccess.Abstraction;
using Reenbit.IMS.DataAccess.CosmosDb.CosmosDbClient;
using Reenbit.IMS.Domain.Documents;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace Reenbit.IMS.DataAccess.CosmosDb.Repositories
{
    public abstract class DocumentRepositoryBase<TDocument> : IRepository<TDocument>
        where TDocument : class, IDocument
    {
        protected DocumentRepositoryBase(
           IConfiguration configuration,
           ICosmosDbClient client)
        {
            this.Configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            this.Client = client ?? throw new ArgumentNullException(nameof(client));
        }

        protected IConfiguration Configuration { get; }

        protected ICosmosDbClient Client { get; }

        protected string DatabaseName => this.Configuration["CosmosDb:DatabaseName"];

        protected abstract string CollectionName { get; }

        public async Task<TDocument> GetById(string id)
        {
            try
            {
                return await this.Client.GetDocumentByIdAsync<TDocument>(this.DatabaseName, this.CollectionName, id, id);
            }
            catch (DocumentClientException e)
            {
                if (e.StatusCode == HttpStatusCode.NotFound)
                {
                    return null;
                }

                throw;
            }
        }

        public Task<List<TDocument>> GetList()
        {
            return this.Client.GetDocumentsAsync<TDocument>(this.DatabaseName, this.CollectionName);
        }

        public async Task Create(TDocument document)
        {
            string documentId = await this.Client.CreateDocumentAsync<TDocument>(this.DatabaseName, this.CollectionName, document);

            document.Id = documentId;
        }

        public Task Update(TDocument document)
        {
            return this.Client.ReplaceDocumentAsync<TDocument>(this.DatabaseName, this.CollectionName, document, d => d.Id);
        }

        public async Task<bool> Delete(string id)
        {
            try
            {
                await this.Client.DeleteDocumentAsync(this.DatabaseName, this.CollectionName, id);
                
                return true;
            }
            catch (DocumentClientException e)
            {
                if (e.StatusCode == HttpStatusCode.NotFound)
                {
                    return false;
                }

                throw;
            }
        }
    }
}
