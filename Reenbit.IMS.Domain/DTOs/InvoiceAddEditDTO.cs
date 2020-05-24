using System;

namespace Reenbit.IMS.Domain.DTOs
{
    public class InvoiceAddEditDTO
    {
        public string Name { get; set; }
        
        public string Description { get; set; }
        
        public DateTime DateSent { get; set; }
        
        public decimal TotalAmount { get; set; }
    }
}
