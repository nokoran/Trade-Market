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
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _ps;

        public ProductsController(IProductService ps)
        {
            _ps = ps;
        }

        //GET: api/products/1
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductModel>> GetById(int id)
        {
            var product  = await _ps.GetByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            return Ok(product);
        }
        
        //GET: api/products?categoryId=1&minPrice=20&maxPrice=50
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductModel>>> GetWithFilter([FromQuery] FilterSearchModel filter)
        {
            var products = await _ps.GetByFilterAsync(filter);
            if (products == null)
            {
                return NotFound();
            }
            return Ok(products);
        }
        
        //POST: api/products
        [HttpPost]
        public async Task<ActionResult> Add([FromBody] ProductModel value)
        {
            try
            {
                await _ps.AddAsync(value);
            }
            catch (MarketException ex)
            {
                return BadRequest(ex.Message);
            }
            
            return Created("api/products", value);
        }
        
        //PUT: api/products/{id}
        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int Id, [FromBody] ProductModel value)
        {
            try
            {
                await _ps.UpdateAsync(value);
                var result = await _ps.GetByIdAsync(Id);
                return Ok(result);
            }
            catch (MarketException ex)
            {
                return BadRequest(ex.Message);
            }
        }
        
        //DELETE: api/products/{id}
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var product = await _ps.GetByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            await _ps.DeleteAsync(id);
            return NoContent();
        }
        
        //GET: api/products/categories
        [HttpGet("categories/")]
        public async Task<ActionResult<IEnumerable<ProductCategoryModel>>> GetCategories()
        {
            return Ok(await _ps.GetAllProductCategoriesAsync());
        }
        
        //POST: api/products/categories
        [HttpPost("categories/")]
        public async Task<ActionResult> AddCategory([FromBody] ProductCategoryModel value)
        {
            try
            {
                await _ps.AddCategoryAsync(value);
            }
            catch (MarketException ex)
            {
                return BadRequest(ex.Message);
            }
            
            return Created("api/products/categories", value);
        }
        
        //PUT: api/products/categories/{id}
        [HttpPut("categories/{id}")]
        public async Task<ActionResult> UpdateCategory(int Id, [FromBody] ProductCategoryModel value)
        {
            try
            {
                await _ps.UpdateCategoryAsync(value);
                var result = await _ps.GetByIdProductCategoryAsync(Id);
                return Ok(result);
            }
            catch (MarketException ex)
            {
                return BadRequest(ex.Message);
            }
        }
        
        //DELETE: api/products/categories/{id}
        [HttpDelete("categories/{id}")]
        public async Task<ActionResult> DeleteCategory(int id)
        {
            var productCategory = await _ps.GetByIdProductCategoryAsync(id);
            if (productCategory == null)
            {
                return NotFound();
            }
            await _ps.RemoveCategoryAsync(id);
            return NoContent();
        }
    }
}