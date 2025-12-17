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
    public class PersonRepository : IPersonRepository
    {
        private readonly TradeMarketDbContext _dbcontext;

        public PersonRepository(TradeMarketDbContext dbcontext)
        {
            _dbcontext = dbcontext;
        }

        public async Task<IEnumerable<Person>> GetAllAsync()
        {
            return await _dbcontext.Persons.ToListAsync();
        }

        public async Task<Person> GetByIdAsync(int id)
        {
            return await _dbcontext.Persons.FindAsync(id);
        }

        public async Task AddAsync(Person entity)
        {
            await _dbcontext.Persons.AddAsync(entity);
        }

        public void Delete(Person entity)
        {
            _dbcontext.Persons.Remove(entity);
        }

        public async Task DeleteByIdAsync(int id)
        {
            var PersonToDelete = await _dbcontext.Persons.FindAsync(id);
            _dbcontext.Persons.Remove(PersonToDelete);
        }

        public void Update(Person entity)
        {
            _dbcontext.Persons.Update(entity);
        }
    }
}
