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
    public class StatisticsController : ControllerBase
    {
        private readonly IStatisticService _ss;

        public StatisticsController(IStatisticService ss)
        {
            _ss = ss;
        }
        
        //GET/api/statistic/popularProducts?productCount=2
        [HttpGet("popularProducts")]
        public async Task<ActionResult<IEnumerable<ProductModel>>> GetPopularProducts([FromQuery] int productCount)
        {
            var products = await _ss.GetMostPopularProductsAsync(productCount);
            if (!products.Any())
            {
                return NoContent();
            }
            return Ok(products);
        }
        
        //GET/api/statistic/customer/{id}/{productCount}
        [HttpGet("customer/{id}/{productCount}")]
        public async Task<ActionResult<IEnumerable<ProductModel>>> GetPopularProductsByCustomer(int id, int productCount)
        {
            var products = await _ss.GetCustomersMostPopularProductsAsync(id,productCount);
            if (!products.Any())
            {
                return NoContent();
            }
            return Ok(products);
        }
        
        //GET/api/statistic/activity/{customerCount}?startDate=2020-7-21&endDate=2020-7-22
        [HttpGet("activity/{customerCount}")]
        public async Task<ActionResult<IEnumerable<ProductModel>>> GetsMostActiveCustomers(int customerCount, [FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            var customers = await _ss.GetMostValuableCustomersAsync(customerCount, startDate, endDate);
            if (!customers.Any())
            {
                return NoContent();
            }
            return Ok(customers);
        }
        
        //GET/api/statistic/income/{categoryId}?startDate= 2020-7-21&endDate= 2020-7-22
        [HttpGet("income/{categoryId}")]
        public async Task<ActionResult<IEnumerable<ProductModel>>> GetsIncomeOfCategory(int categoryId, [FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            var income = await _ss.GetIncomeOfCategoryInPeriod(categoryId, startDate, endDate);
            return Ok(income);
        }
    }
}