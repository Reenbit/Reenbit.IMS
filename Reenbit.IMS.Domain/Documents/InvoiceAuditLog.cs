using Newtonsoft.Json;
using Reenbit.IMS.Domain.Auditing;

namespace Reenbit.IMS.Domain.Documents
{
    public class InvoiceAuditLog : AuditLog, IDocument
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("invoiceId")]
        public string InvoiceId { get; set; }

        [JsonProperty("_ts")]
        public int Timestamp { get; set; }
    }
}
