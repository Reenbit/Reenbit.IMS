using Reenbit.IMS.Domain.DTOs;
using System;
using System.Threading.Tasks;

namespace Reenbit.IMS.Services.Abstraction
{
    public interface IInvoiceService
    {
        Task<InvoiceViewDTO> GetInvoice(string invoiceId);

        Task<string> CreateInvoice(InvoiceAddEditDTO invoiceDto, string actorId);

        Task UpdateInvoice(string invoiceId, InvoiceAddEditDTO invoiceDto, string actorId);

        Task<bool> DeleteInvoice(string invoiceId);
    }
}
