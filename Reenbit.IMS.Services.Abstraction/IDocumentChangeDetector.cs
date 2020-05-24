using Reenbit.IMS.Domain.Auditing;
using System.Collections.Generic;

namespace Reenbit.IMS.Services.Abstraction
{
    public interface IDocumentChangeDetector
    {
        IReadOnlyCollection<AuditLogUpdate> DetectChanges<TDocument>(TDocument oldDocument, TDocument newDocument);
    }
}
