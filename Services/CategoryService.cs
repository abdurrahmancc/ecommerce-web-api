using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Ecommerce_web_api.Controllers;
using Ecommerce_web_api.data;
using Ecommerce_web_api.DTOs;
using Ecommerce_web_api.Enums;
using Ecommerce_web_api.Interfaces;
using Ecommerce_web_api.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce_web_api.Services
{
    public class CategoryService : ICategoryServices
    {
        private readonly AppDbContext _appDbContext;
        private readonly IMapper _mapper;

        public CategoryService(IMapper mapper, AppDbContext appDbContext)
        {
            _mapper = mapper;
            _appDbContext = appDbContext;
        }

        public async Task<PaginatedResult<CategoryReadDto>> GetAllCategoriesService(int pageNumber, int pageSize, string search, string sortOrder)
        {
            IQueryable<Category> query = _appDbContext.Categories;

            if (!string.IsNullOrWhiteSpace(search))
            {
                var formattedSearch = $"%{search.Trim()}%";
                query = query.Where(C => EF.Functions.ILike(C.Name, formattedSearch) || EF.Functions.ILike(C.Description, formattedSearch));
            }

            if (!string.IsNullOrWhiteSpace(sortOrder))
            {
                var formattedSortOrder = sortOrder.Trim().ToLowerInvariant();
                if (Enum.TryParse<SortOrder>(formattedSortOrder, true, out var parseFormattedSortOrder))
                {
                    query = parseFormattedSortOrder switch
                    {
                        SortOrder.name_asc => query.OrderBy(C => C.Name),
                        SortOrder.name_desc => query.OrderByDescending(C => C.Name),
                        SortOrder.createdAt_asc => query.OrderBy(C => C.CreateAt),
                        SortOrder.createdAt_desc => query.OrderByDescending(C => C.CreateAt),
                        _ => query.OrderBy(C => C.Name)
                    };
                }

            }

            var totalCount = await query.CountAsync();
            var items = await query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();

            var result = _mapper.Map<List<CategoryReadDto>>(items);

            return new PaginatedResult<CategoryReadDto>
            {
                Items = result,
                TotalCount = totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize,
            };

        }


        public async Task<CategoryReadDto> CreateCategoriesServices(CategoryCreateDto categoryData)
        {
            var newCategory = _mapper.Map<Category>(categoryData);
            newCategory.CategoryId = Guid.NewGuid();
            newCategory.CreateAt = DateTime.UtcNow;
            await _appDbContext.Categories.AddAsync(newCategory);
            await _appDbContext.SaveChangesAsync();
            return _mapper.Map<CategoryReadDto>(newCategory);
        }


        public async Task<CategoryReadDto?> GetCategoryByIdServices(Guid id)
        {
            var foundCategory = await _appDbContext.Categories.FindAsync(id);

            return foundCategory == null ? null : _mapper.Map<CategoryReadDto>(foundCategory);


        }


        public async Task<int> DeleteCategoryByIdServices(Guid id)
        {
            var foundCategory = await _appDbContext.Categories.FindAsync(id);
            if (foundCategory == null)
            {
                return 404;
            };

            _appDbContext.Categories.Remove(foundCategory);
            await _appDbContext.SaveChangesAsync();
            return 204;
        }

        public async Task<CategoryReadDto?> UpdateCategoryByIdServices(Guid Id, CategoryUpdateDto category)
        {
            var foundCategory = await _appDbContext.Categories.FindAsync(Id);
            if (foundCategory == null)
            {
                return null;
            }
            _mapper.Map(category, foundCategory);
            await _appDbContext.SaveChangesAsync();
            return _mapper.Map<CategoryReadDto>(foundCategory);
        }
    }
}