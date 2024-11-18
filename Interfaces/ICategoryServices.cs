using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ecommerce_web_api.Controllers;
using Ecommerce_web_api.DTOs;

namespace Ecommerce_web_api.Interfaces
{
    public interface ICategoryServices
    {
        Task<PaginatedResult<CategoryReadDto>> GetAllCategoriesService(int pageNumber, int pageSize, string? search=null, string? sortOrder=null);

        Task<CategoryReadDto> CreateCategoriesServices(CategoryCreateDto categoryData);
        Task<CategoryReadDto?> GetCategoryByIdServices(Guid id);

        Task<int> DeleteCategoryByIdServices(Guid id);

        Task<CategoryReadDto?> UpdateCategoryByIdServices(Guid Id, CategoryUpdateDto category);
    }
}