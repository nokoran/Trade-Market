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
    public class ReceiptDetailRepository : IReceiptDetailRepository
    {
        private readonly TradeMarketDbContext _dbcontext;

        public ReceiptDetailRepository(TradeMarketDbContext dbcontext)
        {
            _dbcontext = dbcontext;
        }

        public async Task<IEnumerable<ReceiptDetail>> GetAllAsync()
        {
            return await _dbcontext.ReceiptsDetails.ToListAsync();
        }

        public async Task<ReceiptDetail> GetByIdAsync(int id)
        {
            return await _dbcontext.ReceiptsDetails.FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task AddAsync(ReceiptDetail entity)
        {
            await _dbcontext.ReceiptsDetails.AddAsync(entity);
        }

        public void Delete(ReceiptDetail entity)
        {
            _dbcontext.ReceiptsDetails.Remove(entity);
        }

        public async Task DeleteByIdAsync(int id)
        {
            var receiptDetailToDelete = await _dbcontext.ReceiptsDetails.FirstOrDefaultAsync(p => p.Id == id);
            _dbcontext.ReceiptsDetails.Remove(receiptDetailToDelete);
        }

        public void Update(ReceiptDetail entity)
        {
            _dbcontext.ReceiptsDetails.Update(entity);
        }

        public async Task<IEnumerable<ReceiptDetail>> GetAllWithDetailsAsync()
        {
            return await _dbcontext.ReceiptsDetails
                .Include("Product.Category")
                .Include("Receipt")
                .ToListAsync();
        }
    }
}
