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
    public class ProductCategoryRepository : IProductCategoryRepository
    {
        private readonly TradeMarketDbContext _dbcontext;

        public ProductCategoryRepository(TradeMarketDbContext dbcontext)
        {
            _dbcontext = dbcontext;
        }

        public async Task<IEnumerable<ProductCategory>> GetAllAsync()
        {
            return await _dbcontext.ProductCategories.ToListAsync();
        }

        public async Task<ProductCategory> GetByIdAsync(int id)
        {
            return await _dbcontext.ProductCategories.FindAsync(id);
        }

        public async Task AddAsync(ProductCategory entity)
        {
            await _dbcontext.ProductCategories.AddAsync(entity);
        }

        public void Delete(ProductCategory entity)
        {
            _dbcontext.ProductCategories.Remove(entity);
        }

        public async Task DeleteByIdAsync(int id)
        {
            var ProductCategoryToDelete = await _dbcontext.ProductCategories.FindAsync(id);
            _dbcontext.ProductCategories.Remove(ProductCategoryToDelete);
        }

        public void Update(ProductCategory entity)
        {
            _dbcontext.ProductCategories.Update(entity);
        }

        public async Task<IEnumerable<ProductCategory>> GetAllWithDetailsAsync()
        {
            var ProductCategoryDetails = await _dbcontext.ProductCategories
                .Include(c => c.Products)
                .ToListAsync();
            return ProductCategoryDetails;
        }

        public async Task<ProductCategory> GetByIdWithDetailsAsync(int id)
        {
            var ProductCategoryDetails = await _dbcontext.ProductCategories
                .Include(c => c.Products)
                .ToListAsync();
            return ProductCategoryDetails.Find(c => c.Id == id);
        }
    }
}
