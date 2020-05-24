using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Reenbit.IMS.Domain.Auditing
{
    public class AuditLog
    {
        [JsonProperty("userId")]
        public string UserId { get; set; }

        [JsonProperty("date")]
        public DateTime Date { get; set; }

        [JsonProperty("updates")]
        public List<AuditLogUpdate> Updates { get; set; }

        public static AuditLog Create(string userId, IReadOnlyCollection<AuditLogUpdate> updates = null)
        {
            return new AuditLog
            {
                UserId = userId,
                Date = DateTime.UtcNow,
                Updates = updates?.ToList() ?? new List<AuditLogUpdate>()
            };
        }
    }
}
