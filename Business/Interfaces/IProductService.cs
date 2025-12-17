using Business.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Business.Interfaces
{
    public interface IProductService : ICrud<ProductModel>
    {
        Task<IEnumerable<ProductModel>> GetByFilterAsync(FilterSearchModel filterSearch);
        Task<IEnumerable<ProductCategoryModel>> GetAllProductCategoriesAsync();
        Task<ProductCategoryModel> GetByIdProductCategoryAsync(int id);
        Task AddCategoryAsync(ProductCategoryModel categoryModel);
        Task UpdateCategoryAsync(ProductCategoryModel categoryModel);
        Task RemoveCategoryAsync(int categoryId);
    }
}
