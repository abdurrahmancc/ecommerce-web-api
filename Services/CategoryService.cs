using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Ecommerce_web_api.data;
using Ecommerce_web_api.DTOs;
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

        public async Task<List<CategoryReadDto>> GetAllCategoriesService()
        {
            var categories = await _appDbContext.Categories.ToListAsync();
            return _mapper.Map<List<CategoryReadDto>>(categories);
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