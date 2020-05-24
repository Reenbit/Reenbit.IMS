namespace Reenbit.IMS.Domain.Documents
{
    public interface IDocument
    {
        string Id { get; set; }

        int Timestamp { get; }
    }
}
