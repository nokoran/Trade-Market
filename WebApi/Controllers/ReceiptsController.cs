using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Business.Interfaces;
using Business.Models;
using Business.Validation;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReceiptsController : ControllerBase
    {
        private readonly IReceiptService _rs;

        public ReceiptsController(IReceiptService rs)
        {
            _rs = rs;
        }
        
        //GET/api/receipts
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ReceiptModel>>> Get()
        {
            return Ok(await _rs.GetAllAsync());
        }
        
        //GET/api/receipts/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<ReceiptModel>> GetById(int id)
        {
            var receipt = await _rs.GetByIdAsync(id);
            if (receipt == null)
            {
                return NotFound();
            }
            return Ok(receipt);
        }
        
        //GET/api/receipts/{id}/details
        [HttpGet("{id}/details")]
        public async Task<ActionResult<ReceiptDetailModel>> GetByIdWithDetails(int id)
        {
            var receipt = await _rs.GetReceiptDetailsAsync(id);
            if (receipt == null)
            {
                return NotFound();
            }
            return Ok(receipt);
        }
        
        //GET/api/receipts/{id}/sum
        [HttpGet("{id}/sum")]
        public async Task<ActionResult<decimal>> GetSumById(int id)
        {
            var receipt = await _rs.GetByIdAsync(id);
            if (receipt == null)
            {
                return NotFound();
            }
            return Ok(await _rs.ToPayAsync(id));
        }
        
        //GET/api/receipts/period?startDate=2021-12-1&endDate=2020-12-31
        [HttpGet("period")]
        public async Task<ActionResult<IEnumerable<ReceiptModel>>> GetByPeriod([FromQuery] string startDate, [FromQuery] string endDate)
        {
            var receipts = await _rs.GetReceiptsByPeriodAsync(DateTime.Parse(startDate), DateTime.Parse(endDate));
            if (!receipts.Any())
            {
                return NoContent();
            }
            return Ok(receipts);
        }
        
        //POST/api/receipts
        [HttpPost]
        public async Task<ActionResult> Add([FromBody] ReceiptModel model)
        {
            try
            {
                await _rs.AddAsync(model);
            }
            catch (MarketException ex)
            {
                return BadRequest(ex.Message);
            }
            
            return Created("api/customers", model);
        }
        
        //PUT/api/receipts/{id}
        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, ReceiptModel model)
        {
            try
            {
                await _rs.UpdateAsync(model);
                var result = await _rs.GetByIdAsync(id);
                return Ok(result);
            }
            catch (MarketException ex)
            {
                return BadRequest(ex.Message);
            }
        }
        
        //PUT/api/receipts/{id}/products/add/{productId}/{quantity}
        [HttpPut("{id}/products/add/{productId}/{quantity}")]
        public async Task<ActionResult> AddProductToReceipt(int id, int productId, int quantity)
        {
            var receipt = await _rs.GetByIdAsync(id);
            if (receipt == null)
            {
                return NotFound();
            }
            await _rs.AddProductAsync(productId, id, quantity);
            return Ok();
        }
        
        //PUT/api/receipts/{id}/products/remove/{productId}/{quantity}
        [HttpPut("{id}/products/remove/{productId}/{quantity}")]
        public async Task<ActionResult> RemoveProductFromReceipt(int id, int productId, int quantity)
        {
            var receipt = await _rs.GetByIdAsync(id);
            if (receipt == null)
            {
                return NotFound();
            }
            await _rs.RemoveProductAsync(productId, id, quantity);
            return Ok();
        }
        
        //PUT/api/receipts/{id}/checkout
        [HttpPut("{id}/checkout")]
        public async Task<ActionResult> ReceiptCheckout(int id)
        {
            var receipt = await _rs.GetByIdAsync(id);
            if (receipt == null)
            {
                return NotFound();
            }
            await _rs.CheckOutAsync(id);
            return Ok();
        }
        
        //DELETE/api/receipts/{id}
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteReceipt(int id)
        {
            var receipt = await _rs.GetByIdAsync(id);
            if (receipt == null)
            {
                return NotFound();
            }
            await _rs.DeleteAsync(id);
            return Ok();
        }
    }
}