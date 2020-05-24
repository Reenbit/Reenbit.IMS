using Reenbit.IMS.Domain.Attributes;
using Reenbit.IMS.Domain.Auditing;
using Reenbit.IMS.Services.Abstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Reenbit.IMS.Services
{
    public class DocumentChangeDetector : IDocumentChangeDetector
    {
        public IReadOnlyCollection<AuditLogUpdate> DetectChanges<TDocument>(TDocument oldDocument, TDocument newDocument)
        {
            if (oldDocument == null)
            {
                throw new ArgumentNullException(nameof(oldDocument));
            }

            if (newDocument == null)
            {
                throw new ArgumentNullException(nameof(newDocument));
            }

            List<TrackablePropertyDescriptor> propertyDescriptors = GetTrackablePropertyDescriptors(typeof(TDocument));

            List<AuditLogUpdate> documentChanges = new List<AuditLogUpdate>();
            foreach (var descriptor in propertyDescriptors)
            {
                PropertyInfo property = descriptor.Property;

                object oldValue = property.GetValue(oldDocument);
                object newValue = property.GetValue(newDocument);
                
                if (oldValue?.ToString() != newValue?.ToString())
                {
                    documentChanges.Add(new AuditLogUpdate
                    {
                        FieldName = property.Name,
                        OldValue = oldValue,
                        NewValue = newValue
                    });
                }
            }

            return documentChanges;
        }

        private static List<TrackablePropertyDescriptor> GetTrackablePropertyDescriptors(Type documentType)
        {
            return documentType.GetProperties()
                  .Select(p => new TrackablePropertyDescriptor
                  {
                      Property = p,
                      ChangeTrackableAttribute = p.GetCustomAttribute<ChangeTrackableAttribute>()
                  })
                  .Where(desc => desc.ChangeTrackableAttribute != null)
                  .ToList();
        }

        private class TrackablePropertyDescriptor
        {
            public PropertyInfo Property { get; set; }

            public ChangeTrackableAttribute ChangeTrackableAttribute { get; set; }
        }
    }
}