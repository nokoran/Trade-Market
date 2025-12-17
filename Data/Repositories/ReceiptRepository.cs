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
    public class ReceiptRepository : IReceiptRepository
    {
        private readonly TradeMarketDbContext _dbcontext;

        public ReceiptRepository(TradeMarketDbContext dbcontext)
        {
            _dbcontext = dbcontext;
        }

        public async Task<IEnumerable<Receipt>> GetAllAsync()
        {
            return await _dbcontext.Receipts.ToListAsync();
        }

        public async Task<Receipt> GetByIdAsync(int id)
        {
            return await _dbcontext.Receipts.FindAsync(id);
        }

        public async Task AddAsync(Receipt entity)
        {
            await _dbcontext.Receipts.AddAsync(entity);
        }

        public void Delete(Receipt entity)
        {
            _dbcontext.Receipts.Remove(entity);
        }

        public async Task DeleteByIdAsync(int id)
        {
            var ReceiptToDelete = await _dbcontext.Receipts.FindAsync(id);
            _dbcontext.Receipts.Remove(ReceiptToDelete);
        }

        public void Update(Receipt entity)
        {
            _dbcontext.Receipts.Update(entity);
        }

        public async Task<IEnumerable<Receipt>> GetAllWithDetailsAsync()
        {
            return await _dbcontext.Receipts
                .Include("ReceiptDetails.Product.Category")
                .Include("Customer.Person")
                .ToListAsync();
        }

        public async Task<Receipt> GetByIdWithDetailsAsync(int id)
        {
            return await _dbcontext.Receipts
                .Include("ReceiptDetails.Product.Category")
                .Include("Customer")
                .FirstOrDefaultAsync(c => c.Id == id);
        }
    }
}
