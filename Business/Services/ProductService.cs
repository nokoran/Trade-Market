using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Business.Interfaces;
using Business.Models;
using Business.Validation;
using Data.Entities;
using Data.Interfaces;

namespace Business.Services
{
    public class ProductService : IProductService
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;
        
        public ProductService(IUnitOfWork context, IMapper mapper)
        {
            _uow = context;
            _mapper = mapper;
        }
        public async Task<IEnumerable<ProductModel>> GetAllAsync()
        {
            return _mapper.Map<IEnumerable<ProductModel>>(await _uow.ProductRepository.GetAllWithDetailsAsync());
        }

        public async Task<ProductModel> GetByIdAsync(int id)
        {
            return _mapper.Map<ProductModel>(await _uow.ProductRepository.GetByIdWithDetailsAsync(id));
        }

        public async Task AddAsync(ProductModel model)
        {
            if (model == null)
                throw new MarketException(nameof(model));
            if (string.IsNullOrEmpty(model.ProductName))
            {
                throw new MarketException("Name is Empty");
            }
            if (model.Price <= 0)
            {
                throw new MarketException("Price is Negative");
            }

            await _uow.ProductRepository.AddAsync(_mapper.Map<Product>(model));
            await _uow.SaveAsync();
        }

        public async Task UpdateAsync(ProductModel model)
        {
            if (string.IsNullOrEmpty(model.ProductName))
            {
                throw new MarketException("Surname is Empty");
            }

            _uow.ProductRepository.Update( _mapper.Map<Product>(model));
            await _uow.SaveAsync();
        }

        public async Task DeleteAsync(int modelId)
        {
            await _uow.ProductRepository.DeleteByIdAsync(modelId);
            await _uow.SaveAsync();
        }

        public async Task<IEnumerable<ProductModel>> GetByFilterAsync(FilterSearchModel filterSearch)
        {
            IEnumerable<Product> result = await _uow.ProductRepository.GetAllWithDetailsAsync();
            if(filterSearch.CategoryId != null)
            {
                result = result.Where(x => x.ProductCategoryId == filterSearch.CategoryId).Select(x => x);
            }
            if(filterSearch.MinPrice != null)
            {
                result = result.Where(x => x.Price >= filterSearch.MinPrice).Select(x => x);
            }
            if(filterSearch.MaxPrice != null)
            {
                result = result.Where(x => x.Price <= filterSearch.MaxPrice).Select(x => x);
            }
            return _mapper.Map<IEnumerable<ProductModel>>(result);
        }

        public async Task<IEnumerable<ProductCategoryModel>> GetAllProductCategoriesAsync()
        {
            return _mapper.Map<IEnumerable<ProductCategoryModel>>(await _uow.ProductCategoryRepository.GetAllAsync());
        }
        
        public async Task<ProductCategoryModel> GetByIdProductCategoryAsync(int id)
        {
            return _mapper.Map<ProductCategoryModel>(await _uow.ProductCategoryRepository.GetByIdAsync(id));
        }

        public async Task AddCategoryAsync(ProductCategoryModel categoryModel)
        {
            if (string.IsNullOrEmpty(categoryModel.CategoryName))
            {
                throw new MarketException("Category Name is Empty");
            }
            
            await _uow.ProductCategoryRepository.AddAsync(_mapper.Map<ProductCategory>(categoryModel));
            await _uow.SaveAsync();
        }

        public async Task UpdateCategoryAsync(ProductCategoryModel categoryModel)
        {
            if (string.IsNullOrEmpty(categoryModel.CategoryName))
            {
                throw new MarketException("Category Name is Empty");
            }
            _uow.ProductCategoryRepository.Update(_mapper.Map<ProductCategory>(categoryModel));

            await _uow.SaveAsync();
        }

        public async Task RemoveCategoryAsync(int categoryId)
        {
            await _uow.ProductCategoryRepository.DeleteByIdAsync(categoryId);
            await _uow.SaveAsync();
        }
    }
}