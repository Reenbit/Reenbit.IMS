using System;

namespace Reenbit.IMS.Domain.DTOs
{
    public class InvoiceViewDTO
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public DateTime DateSent { get; set; }

        public decimal TotalAmount { get; set; }
    }
}
