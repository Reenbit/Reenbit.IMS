using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Documents.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Reenbit.IMS.DataAccess.CosmosDb.CosmosDbClient
{
    public class CosmosDbClient : ICosmosDbClient
    {
        private readonly DocumentClient documentClient;

        public CosmosDbClient(DocumentClientConfiguration configuration)
        {
            if (configuration == null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }

            var connectionPolicy = new ConnectionPolicy
            {
                ConnectionMode = ConnectionMode.Direct,
                ConnectionProtocol = Protocol.Tcp
            };

            this.documentClient = new DocumentClient(
                 new Uri(configuration.DocumentDbUri),
                 configuration.DocumentDbKey,
                 connectionPolicy);
        }

        public async Task<string> CreateDocumentAsync<TDocument>(
            string dbName, string collName, TDocument document)
        {
            ResourceResponse<Document> response =
                await this.documentClient.CreateDocumentAsync(UriFactory.CreateDocumentCollectionUri(dbName, collName), document)
                .ConfigureAwait(false);

            return response.Resource.Id;
        }

        public Task ReplaceDocumentAsync<TDocument>(
            string dbName, string collName, TDocument document, Func<TDocument, string> idSelector, object partitionKey = null)
        {
            string documentId = idSelector(document);

            return this.documentClient.ReplaceDocumentAsync(
                UriFactory.CreateDocumentUri(dbName, collName, documentId),
                document,
                GetStandardRequestOptions(partitionKey));
        }

        public Task DeleteDocumentAsync(
            string dbName, string collName, string documentId, object partitionKey = null)
        {
            return this.documentClient.DeleteDocumentAsync(
              UriFactory.CreateDocumentUri(dbName, collName, documentId),
              GetStandardRequestOptions(partitionKey));
        }

        public async Task<TDocument> GetDocumentByIdAsync<TDocument>(
            string dbName, string collName, string documentId, object partitionKey = null)
        {
            Uri documentUri = UriFactory.CreateDocumentUri(dbName, collName, documentId);

            DocumentResponse<TDocument> response = await this.documentClient
                .ReadDocumentAsync<TDocument>(documentUri, GetStandardRequestOptions(partitionKey))
                .ConfigureAwait(false);

            return response.Document;
        }

        public async Task<List<TDocument>> GetDocumentsAsync<TDocument>(
            string dbName, string collName, object partitionKey = null)
        {
            IQueryable<TDocument> query = this.documentClient
               .CreateDocumentQuery<TDocument>(UriFactory.CreateDocumentCollectionUri(dbName, collName), GetStandardFeedOptions(partitionKey));

            List<TDocument> documents = await this.ExecuteQueryAsync(query).ConfigureAwait(false);

            return documents;
        }

        private async Task<List<TDocument>> ExecuteQueryAsync<TDocument>(IQueryable<TDocument> query)
        {
            var documents = new List<TDocument>();
            IDocumentQuery<TDocument> documentQuery = query.AsDocumentQuery();

            while (documentQuery.HasMoreResults)
            {
                FeedResponse<TDocument> nextResults = await documentQuery.ExecuteNextAsync<TDocument>().ConfigureAwait(false);
                documents.AddRange(nextResults);
            }

            return documents;
        }

        private static RequestOptions GetStandardRequestOptions(object partitionKey)
        {
            return partitionKey == null
                ? null
                : new RequestOptions { PartitionKey = new PartitionKey(partitionKey) };
        }

        private static FeedOptions GetStandardFeedOptions(object partitionKey)
        {
            return partitionKey == null
                ? new FeedOptions
                {
                    MaxItemCount = -1
                }
                : new FeedOptions { PartitionKey = new PartitionKey(partitionKey) };
        }
    }
}
