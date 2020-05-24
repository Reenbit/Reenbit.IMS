using AutoMapper;
using Reenbit.IMS.DataAccess.Abstraction;
using Reenbit.IMS.Domain.Auditing;
using Reenbit.IMS.Domain.Documents;
using Reenbit.IMS.Domain.DTOs;
using Reenbit.IMS.Services.Abstraction;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Reenbit.IMS.Services
{
    public class InvoiceService : IInvoiceService
    {
        private readonly IInvoiceRepository invoiceRepo;

        private readonly IDocumentChangeDetector changeDetector;

        private readonly IMapper mapper;

        public InvoiceService(
            IInvoiceRepository invoiceRepo,
            IDocumentChangeDetector changeDetector,
            IMapper mapper)
        {
            this.invoiceRepo = invoiceRepo ?? throw new ArgumentNullException(nameof(invoiceRepo));
            this.changeDetector = changeDetector ?? throw new ArgumentNullException(nameof(changeDetector));
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }
        
        public async Task<InvoiceViewDTO> GetInvoice(string invoiceId)
        {
            Invoice invoice = await this.invoiceRepo.GetById(invoiceId);
            if (invoice == null)
            {
                return null;
            }

            InvoiceViewDTO invoiceViewDto = this.mapper.Map<InvoiceViewDTO>(invoice);

            return invoiceViewDto;
        }

        public async Task<string> CreateInvoice(InvoiceAddEditDTO invoiceDto, string actorId)
        {
            if (invoiceDto == null)
            {
                throw new ArgumentNullException(nameof(invoiceDto));
            }

            Invoice invoice = this.mapper.Map<Invoice>(invoiceDto);
            invoice.CreateLog = AuditLog.Create(actorId);

            await this.invoiceRepo.Create(invoice);

            return invoice.Id;
        }

        public async Task UpdateInvoice(string invoiceId, InvoiceAddEditDTO invoiceDto, string actorId)
        {
            if (string.IsNullOrWhiteSpace(invoiceId))
            {
                throw new ArgumentException(nameof(invoiceId));
            }

            if (invoiceDto == null)
            {
                throw new ArgumentNullException(nameof(invoiceDto));
            }

            Invoice invoice = await this.invoiceRepo.GetById(invoiceId);
            if (invoice == null)
            {
                throw new InvalidOperationException($"Invoice with id='{invoice.Id}' doesn't exist.");
            }

            Invoice updatedInvoice = this.mapper.Map<Invoice>(invoiceDto);
            IReadOnlyCollection<AuditLogUpdate> invoiceChanges = this.changeDetector.DetectChanges(invoice, updatedInvoice);
            if (invoiceChanges.Count > 0)
            {
                updatedInvoice.Id = invoice.Id;
                updatedInvoice.LastUpdateLog = AuditLog.Create(actorId, invoiceChanges);

                await this.invoiceRepo.Update(updatedInvoice);
            }
        }

        public async Task<bool> DeleteInvoice(string invoiceId)
        {
            Invoice invoice = await this.invoiceRepo.GetById(invoiceId);
            if (invoice == null)
            {
                return false;
            }

            return await this.invoiceRepo.Delete(invoice.Id);
        }
    }
}
