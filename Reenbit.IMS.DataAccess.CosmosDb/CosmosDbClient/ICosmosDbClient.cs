using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Reenbit.IMS.DataAccess.CosmosDb.CosmosDbClient
{
    public interface ICosmosDbClient
    {
        Task<TDocument> GetDocumentByIdAsync<TDocument>(
            string dbName, string collName, string documentId, object partitionKey = null);

        Task<List<TDocument>> GetDocumentsAsync<TDocument>(
            string dbName, string collName, object partitionKey = null);

        Task<string> CreateDocumentAsync<TDocument>(
            string dbName, string collName, TDocument document);

        Task ReplaceDocumentAsync<TDocument>(
            string dbName, string collName, TDocument document, Func<TDocument, string> idSelector, object partitionKey = null);

        Task DeleteDocumentAsync(
            string dbName, string collName, string documentId, object partitionKey = null);
    }
}
