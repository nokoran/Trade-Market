using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Data.Data;
using Data.Entities;
using Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly TradeMarketDbContext _dbcontext;

        public ProductRepository(TradeMarketDbContext dbcontext)
        {
            _dbcontext = dbcontext;
        }

        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            return await _dbcontext.Products.ToListAsync();
        }

        public async Task<Product> GetByIdAsync(int id)
        {
            return await _dbcontext.Products.FindAsync(id);
        }

        public async Task AddAsync(Product entity)
        {
            await _dbcontext.Products.AddAsync(entity);
        }

        public void Delete(Product entity)
        {
            _dbcontext.Products.Remove(entity);
        }

        public async Task DeleteByIdAsync(int id)
        {
            var ProductToDelete = await _dbcontext.Products.FindAsync(id);
            _dbcontext.Products.Remove(ProductToDelete);
        }

        public void Update(Product entity)
        {
            _dbcontext.Products.Update(entity);
        }

        public async Task<IEnumerable<Product>> GetAllWithDetailsAsync()
        {
            var ProductDetails = await _dbcontext.Products
                .Include(c => c.Category)
                .Include(c => c.ReceiptDetails)
                .ToListAsync();
            return ProductDetails;
        }

        public async Task<Product> GetByIdWithDetailsAsync(int id)
        {
            var ProductDetails = await _dbcontext.Products
                .Include(c => c.Category)
                .Include(c => c.ReceiptDetails)
                .ToListAsync();
            return ProductDetails.Find(c => c.Id == id);
        }
    }
}
