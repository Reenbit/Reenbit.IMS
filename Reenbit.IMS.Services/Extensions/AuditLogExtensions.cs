using Reenbit.IMS.Domain.Auditing;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Reenbit.IMS.Services.Extensions
{
    public static class AuditLogExtensions
    {
        public static AuditLog Create(this AuditLog target, Guid userId, IReadOnlyCollection<AuditLogUpdate> updates = null)
        {
            return new AuditLog
            {
                UserId = userId.ToString(),
                Date = DateTime.UtcNow,
                Updates = updates?.ToList() ?? new List<AuditLogUpdate>()
            };
        }
    }
}
