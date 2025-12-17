using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Business.Interfaces;
using Business.Models;
using Data.Interfaces;

namespace Business.Services
{
    public class StatisticService : IStatisticService
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;
        
        public StatisticService(IUnitOfWork context, IMapper mapper)
        {
            _uow = context;
            _mapper = mapper;
        }
        public async Task<IEnumerable<ProductModel>> GetMostPopularProductsAsync(int productCount)
        {
            var list = (await _uow.ReceiptDetailRepository.GetAllWithDetailsAsync())
                .GroupBy(x => x.Product)
                .Select(x => new
                {
                    product = x.Key,
                    quantity = x.Sum(y => y.Quantity)
                })
                .OrderByDescending(x => x.quantity)
                .Select(x => x.product)
                .Take(productCount);
            return _mapper.Map<IEnumerable<ProductModel>>(list);
        }

        public async Task<IEnumerable<ProductModel>> GetCustomersMostPopularProductsAsync(int productCount, int customerId)
        {
            var list = (await _uow.ReceiptRepository.GetAllWithDetailsAsync())
                .FirstOrDefault(l => l.CustomerId == customerId)
                ?.ReceiptDetails
                .OrderByDescending(x => x.Quantity)
                .Select(x => x.Product)
                .Take(productCount);
            return _mapper.Map<IEnumerable<ProductModel>>(list);
        }

        public async Task<IEnumerable<CustomerActivityModel>> GetMostValuableCustomersAsync(int customerCount, DateTime startDate, DateTime endDate)
        {
            var receipts = await _uow.ReceiptRepository.GetAllWithDetailsAsync();
            var list = receipts.Where(x => x.OperationDate >= startDate && x.OperationDate <= endDate)
                .GroupBy(x => x.Customer)
                .Select(x => new
                {
                    Customer = x.Key,
                    TotalAmount = x.Sum(t => t.ReceiptDetails.Sum(y => y.Quantity * y.DiscountUnitPrice))
                })
                .OrderByDescending(x => x.TotalAmount)
                .Take(customerCount)
                .Select(x => new CustomerActivityModel
                {
                    CustomerId = x.Customer.Id,
                    CustomerName = $"{x.Customer.Person.Name} {x.Customer.Person.Surname}",
                    ReceiptSum = x.TotalAmount
                });
                
            return _mapper.Map<IEnumerable<CustomerActivityModel>>(list);
        }

        public async Task<decimal> GetIncomeOfCategoryInPeriod(int categoryId, DateTime startDate, DateTime endDate)
        {
            var list = (await _uow.ReceiptRepository.GetAllWithDetailsAsync())
                .Where(x => x.OperationDate >= startDate && x.OperationDate <= endDate)
                .SelectMany(x => x.ReceiptDetails
                    .Where(t => t.Product.ProductCategoryId == categoryId)
                    .Select(t => t.Quantity * t.DiscountUnitPrice));
                
            return _mapper.Map<IEnumerable<decimal>>(list).Sum();
        }
    }
}