using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ecommerce_web_api.DTOs;
using Ecommerce_web_api.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce_web_api.Services
{
    public class CategoryService
    {
        private static readonly List<Category> categories = new List<Category>();

        public List<CategoryReadDto> GetAllCategories()
        {
            return categories.Select(c => new CategoryReadDto
            {
                CategoryId = c.CategoryId,
                Name = c.Name,
                Description = c.Description,
                CreateAt = c.CreateAt
            }).ToList();
        }


        public CategoryReadDto CreateCategoriesServices([FromBody] CategoryCreateDto categoryData)
        {
            var newCategory = new Category
            {
                CategoryId = Guid.NewGuid(),
                Name = categoryData.Name,
                Description = categoryData.Description,
                CreateAt = DateTime.UtcNow
            };

            var categoryReadDto = new CategoryReadDto
            {
                CategoryId = newCategory.CategoryId,
                Name = newCategory.Name,
                Description = newCategory.Description,
                CreateAt = newCategory.CreateAt
            };

            categories.Add(newCategory);

            return categoryReadDto;
        }


        public CategoryReadDto? GetCategoryByIdServices(Guid id){
             var foundCategory = categories.FirstOrDefault(category => category.CategoryId == id);

            if (foundCategory == null)
            {
                return null;
            }

            var categoryReadDto = new CategoryReadDto
            {
                CategoryId = foundCategory.CategoryId,
                Name = foundCategory.Name,
                Description = foundCategory.Description,
                CreateAt = foundCategory.CreateAt
            };

            return categoryReadDto;
        }


        public int DeleteCategoryByIdServices(Guid id){
            var foundCategory = categories.FirstOrDefault(category=>category.CategoryId == id ); 
            if(foundCategory == null){
                return 404;
            };

            categories.Remove(foundCategory);
            return 204;
        }

        public CategoryUpdateDto? UpdateCategoryByIdServices(Guid Id, [FromBody] CategoryUpdateDto category){
           var foundCategory =  categories.FirstOrDefault(category=>category.CategoryId == Id);
           if(foundCategory == null ){
            return null;
           }

           foundCategory.Name = category.Name ?? foundCategory.Name;
    foundCategory.Description = category.Description ?? foundCategory.Description;

           var updateCategory = new CategoryUpdateDto{
            Name = foundCategory.Name,
            Description = foundCategory.Description,
           };

           return updateCategory;
        }
    }
}