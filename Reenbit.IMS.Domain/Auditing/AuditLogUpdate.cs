namespace Reenbit.IMS.Domain.Auditing
{
    public class AuditLogUpdate
    {
        public string FieldName { get; set; }

        public object OldValue { get; set; }

        public object NewValue { get; set; }
    }
}
