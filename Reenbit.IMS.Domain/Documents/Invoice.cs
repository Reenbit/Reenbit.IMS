using System;
using Newtonsoft.Json;
using Reenbit.IMS.Domain.Attributes;
using Reenbit.IMS.Domain.Auditing;

namespace Reenbit.IMS.Domain.Documents
{
    public class Invoice : IDocument
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [ChangeTrackable]
        [JsonProperty("name")]
        public string Name { get; set; }

        [ChangeTrackable]
        [JsonProperty("description")]
        public string Description { get; set; }

        [ChangeTrackable]
        [JsonProperty("totalAmount")]
        public decimal TotalAmount { get; set; }

        [ChangeTrackable]
        [JsonProperty("dateSent")]
        public DateTime DateSent { get; set; }
        
        [JsonProperty("_ts")]
        public int Timestamp { get; set; }

        [JsonProperty("createLog")]
        public AuditLog CreateLog { get; set; }

        [JsonProperty("lastUpdateLog")]
        public AuditLog LastUpdateLog { get; set; }
    }
}
