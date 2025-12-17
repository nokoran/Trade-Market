using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Business.Interfaces;
using Business.Models;
using Business.Validation;
using Data.Data;
using Data.Entities;
using Data.Interfaces;


namespace Business.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;
        
        public CustomerService(IUnitOfWork uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }
        public async Task<IEnumerable<CustomerModel>> GetAllAsync()
        {
            return _mapper.Map<IEnumerable<CustomerModel>>(await _uow.CustomerRepository.GetAllWithDetailsAsync());
        }

        public async Task<CustomerModel> GetByIdAsync(int id)
        {
            return _mapper.Map<CustomerModel>(await _uow.CustomerRepository.GetByIdWithDetailsAsync(id));
        }

        public async Task AddAsync(CustomerModel model)
        {
            if (model == null)
                throw new MarketException(nameof(model));
            if (string.IsNullOrEmpty(model.Name) || string.IsNullOrEmpty(model.Surname))
                throw new MarketException("Name and Surname cannot be empty");
            if (model.BirthDate > DateTime.Now || model.BirthDate < new DateTime(1900, 1, 1))
            {
                throw new MarketException("BirthDate is not valid");
            }
            if (model.DiscountValue < 0)
                throw new MarketException("Discount value cannot be negative");
            
            await _uow.CustomerRepository.AddAsync(_mapper.Map<Customer>(model));
            await _uow.SaveAsync();
        }

        public async Task UpdateAsync(CustomerModel model)
        {
            if (model == null)
                throw new MarketException(nameof(model));
            if (string.IsNullOrEmpty(model.Name) || string.IsNullOrEmpty(model.Surname))
                throw new MarketException("Name and Surname cannot be empty");
            if (model.BirthDate > DateTime.Now || model.BirthDate < new DateTime(1900, 1, 1))
            {
                throw new MarketException("BirthDate is not valid");
            }
            if (model.DiscountValue < 0)
                throw new MarketException("Discount value cannot be negative");
            
            var customer = await _uow.CustomerRepository.GetByIdAsync(model.Id);
            var result = _mapper.Map(model, customer);
            result.Person.Id = result.PersonId;
            _uow.CustomerRepository.Update(result);
            await _uow.SaveAsync();
        }

        public async Task DeleteAsync(int modelId)
        { 
            await _uow.CustomerRepository.DeleteByIdAsync(modelId);
            await _uow.SaveAsync();
        }

        public async Task<IEnumerable<CustomerModel>> GetCustomersByProductIdAsync(int productId)
        {
            var customers = await _uow.CustomerRepository.GetAllWithDetailsAsync();
            return _mapper.Map<IEnumerable<CustomerModel>>(customers.Where(x => x.Receipts.
                    Any(v => v.ReceiptDetails.
                        Any(y => y.ProductId == productId))).
                ToList());
        }
    }
}