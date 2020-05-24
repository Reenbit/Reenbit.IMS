using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Reenbit.IMS.Domain.Auditing;
using Reenbit.IMS.Domain.Documents;

namespace Reenbit.IMS.Functions
{
    public static class TrackInvoiceUpdates
    {
        private const string InputDbName = "invoicesDb";
        private const string InputCollectionName = "invoices";

        private const string LeaseCollectionName = "leases";

        private const string OutputDbName = "invoicesDb";
        private const string OutputCollectionName = "invoices-change-history";
        private static readonly Uri OutputCollectionUri = UriFactory.CreateDocumentCollectionUri(OutputDbName, OutputCollectionName);

        [FunctionName("TrackInvoiceUpdates")]
        public static async Task Run(
            [CosmosDBTrigger(
            databaseName: InputDbName,
            collectionName: InputCollectionName,
            ConnectionStringSetting = "CosmosDBConnectionString",
            LeaseCollectionName = LeaseCollectionName,
            CreateLeaseCollectionIfNotExists = true)]IReadOnlyList<Document> documents,
            ILogger log,
            [CosmosDB(ConnectionStringSetting = "CosmosDBConnectionString")] DocumentClient documentClient)
        {
            if (documents != null)
            {
                foreach (var document in documents)
                {
                    try
                    {
                        log.LogTrace($"Started handling invoice update. InvoiceId = '{document.Id}'.");
                        await HandleDocumentChange(documentClient, document);
                        log.LogTrace($"Finished handling invoice update. InvoiceId = '{document.Id}'.");
                    }
                    catch (Exception ex)
                    {
                        log.LogError(ex, $"Some error occurred when handling invoice update. InvoiceId = '{document.Id}'.");
                    }
                }
            }
        }

        private static async Task HandleDocumentChange(DocumentClient documentClient, Document document)
        {
            AuditLog lastUpdateLog = document.GetPropertyValue<AuditLog>("lastUpdateLog");
            if (lastUpdateLog != null)
            {
                // it was 'update' action
                string invoiceId = document.Id;

                bool auditLogExists = AuditLogExists(documentClient, invoiceId, lastUpdateLog.Date, lastUpdateLog.UserId);
                if (!auditLogExists)
                {
                    // no maching record in storage (so create new one)
                    var invoiceAuditLog = new InvoiceAuditLog
                    {
                        InvoiceId = invoiceId,
                        UserId = lastUpdateLog.UserId,
                        Date = lastUpdateLog.Date,
                        Updates = lastUpdateLog.Updates
                    };

                    await documentClient.CreateDocumentAsync(OutputCollectionUri, invoiceAuditLog);
                }
            }
        }

        private static bool AuditLogExists(DocumentClient documentClient, string invoiceId, DateTime date, string userId)
        {
            string auditLogId = documentClient.CreateDocumentQuery<InvoiceAuditLog>(
                        OutputCollectionUri,
                        new FeedOptions { PartitionKey = new PartitionKey(invoiceId) })
                .Where(log => log.Date == date && log.UserId == userId)
                .Select(log => log.Id)
                .AsEnumerable()
                .FirstOrDefault();

            return auditLogId != null;
        }
    }
}
