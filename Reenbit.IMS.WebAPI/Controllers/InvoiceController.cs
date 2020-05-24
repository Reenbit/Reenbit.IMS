using System;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Reenbit.IMS.Domain.DTOs;
using Reenbit.IMS.Services.Abstraction;

namespace Reenbit.IMS.WebAPI.Controllers
{
    [ApiController]
    [Route("api/v1/invoices")]
    public class InvoiceController : ControllerBase
    {
        private readonly IInvoiceService invoiceService;

        public InvoiceController(IInvoiceService invoiceService)
        {
            this.invoiceService = invoiceService;
        }
        
        [HttpGet("{id}", Name = InvoiceActionNames.GetById)]
        [Produces(typeof(InvoiceViewDTO))]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(InvoiceViewDTO))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Get(string id)
        {
            InvoiceViewDTO invoice = await this.invoiceService.GetInvoice(id);

            return invoice != null
                ? this.Ok(invoice)
                : (IActionResult)this.NotFound();
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<IActionResult> Post(
            [FromBody] InvoiceAddEditDTO invoiceDto,
            [FromHeader(Name = CustomRequestHeaders.Actor)] [Required] string actorId)
        {
            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            string invoiceId = await this.invoiceService.CreateInvoice(invoiceDto, actorId);

            return this.Created(this.Url.Link(InvoiceActionNames.GetById, new { id = invoiceId }), invoiceId);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> Put(
            string id, 
            [FromBody] InvoiceAddEditDTO invoiceDto,
            [FromHeader(Name = CustomRequestHeaders.Actor)] [Required] string actorId)
        {
            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            await this.invoiceService.UpdateInvoice(id, invoiceDto, actorId);

            return this.Ok();
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> Delete(string id)
        {
            bool result = await this.invoiceService.DeleteInvoice(id);

            return this.Ok(result);
        }

        private static class InvoiceActionNames
        {
            public const string GetById = "GetById";
        }
    }
}
