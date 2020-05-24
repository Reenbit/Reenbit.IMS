using AutoMapper;
using Reenbit.IMS.Domain.Documents;
using Reenbit.IMS.Domain.DTOs;

namespace Reenbit.IMS.Services.Mapping
{
    public class InvoiceProfile : Profile
    {
        public InvoiceProfile()
        {
            // map for 'read invoice' scenarios 
            CreateMap<Invoice, InvoiceViewDTO>();

            // map for 'create/update invoice' scenarios
            CreateMap<InvoiceAddEditDTO, Invoice>();
        }
    }
}
