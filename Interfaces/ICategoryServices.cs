using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ecommerce_web_api.DTOs;

namespace Ecommerce_web_api.Interfaces
{
    public interface ICategoryServices
    {
        Task<List<CategoryReadDto>> GetAllCategoriesService();

        Task<CategoryReadDto> CreateCategoriesServices(CategoryCreateDto categoryData);
        Task<CategoryReadDto?> GetCategoryByIdServices(Guid id);

        Task<int> DeleteCategoryByIdServices(Guid id);

        Task<CategoryReadDto?> UpdateCategoryByIdServices(Guid Id, CategoryUpdateDto category);
    }
}