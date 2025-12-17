using System.Collections.Generic;
using System.Threading.Tasks;
using Data.Data;
using Data.Entities;
using Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly TradeMarketDbContext _dbcontext;

        public CustomerRepository(TradeMarketDbContext dbcontext)
        {
            _dbcontext = dbcontext;
        }

        public async Task<IEnumerable<Customer>> GetAllAsync()
        {
            return await _dbcontext.Customers.ToListAsync();
        }

        public async Task<Customer> GetByIdAsync(int id)
        {
            return await _dbcontext.Customers.FindAsync(id);
        }

        public async Task AddAsync(Customer entity)
        {
            await _dbcontext.Customers.AddAsync(entity);
        }

        public void Delete(Customer entity)
        {
            _dbcontext.Customers.Remove(entity);
        }

        public async Task DeleteByIdAsync(int id)
        {
            var customerToDelete = await _dbcontext.Customers.FindAsync(id);
            _dbcontext.Customers.Remove(customerToDelete);
        }

        public void Update(Customer entity)
        {
            _dbcontext.Customers.Update(entity);
        }

        public async Task<IEnumerable<Customer>> GetAllWithDetailsAsync()
        {
            var customersDetails = await _dbcontext.Customers
                .Include(c => c.Receipts)
                .ThenInclude(r => r.ReceiptDetails)
                .Include(c => c.Person)
                .ToListAsync();
            return customersDetails;
        }

        public async Task<Customer> GetByIdWithDetailsAsync(int id)
        {
            var customersDetails = await _dbcontext.Customers
                .Include(c => c.Receipts)
                .ThenInclude(r => r.ReceiptDetails)
                .Include(c => c.Person)
                .ToListAsync();
            return customersDetails.Find(c => c.Id == id);
        }
    }
}
