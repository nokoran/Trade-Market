using System.Collections.Generic;
using System.Threading.Tasks;
using Business.Interfaces;
using Business.Models;
using Business.Validation;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        private readonly ICustomerService _cs;

        public CustomersController(ICustomerService cs)
        {
            _cs = cs;
        }

        // GET: api/customers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CustomerModel>>> Get()
        {
            return Ok(await _cs.GetAllAsync());
        }

        //GET: api/customers/1
        [HttpGet("{id}")]
        public async Task<ActionResult<CustomerModel>> GetById(int id)
        {
            var customer = await _cs.GetByIdAsync(id);
            if (customer == null)
            {
                return NotFound();
            }
            return Ok(customer);
        }
        
        //GET: api/customers/products/1
        [HttpGet("products/{id}")]
        public async Task<ActionResult<CustomerModel>> GetByProductId(int id)
        {
            var customers = await _cs.GetCustomersByProductIdAsync(id);
            if (customers == null)
            {
                return NotFound();
            }
            return Ok(customers);
        }

        // POST: api/customers
        [HttpPost]
        public async Task<ActionResult> Add([FromBody] CustomerModel value)
        {
            try
            {
                await _cs.AddAsync(value);
            }
            catch (MarketException ex)
            {
                return BadRequest(ex.Message);
            }
            
            return Created("api/customers", value);
        }

        // PUT: api/customers/1
        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int Id, [FromBody] CustomerModel value)
        {
            try
            {
                await _cs.UpdateAsync(value);
                var result = await _cs.GetByIdAsync(Id);
                return Ok(result);
            }
            catch (MarketException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // DELETE: api/customers/1
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var customer = await _cs.GetByIdAsync(id);
            if (customer == null)
            {
                return NotFound();
            }
            await _cs.DeleteAsync(id);
            return NoContent();
        }
    }
}
