using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Ecommerce_web_api.DTOs;
using Ecommerce_web_api.Models;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce_web_api.Controllers
{
    [ApiController]
    [Route("v1/api/categories/")]
    public class CategoryControllers : ControllerBase
    {

        private static List<Category> categories = new List<Category>();



        //GET: /api/categories/ => Read categories
        [HttpGet]
        public IActionResult GetCategories([FromQuery] string searchValue = ""){

            var categoryList = categories.Select(c=> new CategoryReadDto{
                CategoryId = c.CategoryId,
                Name = c.Name,
                Description = c.Description,
                CreateAt = c.CreateAt
            }).ToList();

            return Ok(ApiResponse<List<CategoryReadDto>>.SuccessResponse(categoryList, 200, "Categories returned successfully"));
        }



               //GET: /api/categories/ => Read categories by id
        [HttpGet("{Id:Guid}")]
        public IActionResult GetCategoriesById( Guid id){
            Console.WriteLine("ffff");
            var foundCategory = categories.FirstOrDefault(category=>category.CategoryId == id ); 

            if(foundCategory == null){
                return NotFound(ApiResponse<object>.ErrorResponse(new List<string>{"Category with this id does not exist"}, 404, "Validation failed" ));
            }

            var categoryReadDto =  new CategoryReadDto{
                CategoryId = foundCategory.CategoryId,
                Name = foundCategory.Name,
                Description = foundCategory.Description,
                CreateAt = foundCategory.CreateAt
            };

            return Ok(ApiResponse<CategoryReadDto>.SuccessResponse(categoryReadDto, 200, "Category is returned successfully"));
        }
        

        //POST: /api/categories/ => Create categories
        [HttpPost]
        public IActionResult CreateCategories([FromBody] CategoryCreateDto categoryData){
            
            var newCategory = new Category{
                CategoryId = Guid.NewGuid(),
                Name = categoryData.Name,
                Description = categoryData.Description,
                CreateAt = DateTime.UtcNow
            };
            
            var CategoryReadDto = new CategoryReadDto{
                CategoryId = newCategory.CategoryId,
                Name = newCategory.Name,
                Description = newCategory.Description,
                CreateAt = newCategory.CreateAt
            };
            
            categories.Add(newCategory);
            return Created(nameof(GetCategoriesById), ApiResponse<CategoryReadDto>.SuccessResponse(CategoryReadDto, 201, "Categories Created successfully"));

        }


        //delete: /api/categories/{Id} => Delete a category
        [HttpDelete("{Id:Guid}")]
        public IActionResult DeleteCategories(Guid id){
            var foundCategory = categories.FirstOrDefault(category=>category.CategoryId == id ); 
            if(foundCategory == null){
                return NotFound(ApiResponse<object>.ErrorResponse(new List<string>{"Category with this id does not exist"}, 404, "Validation failed" ));
            };

            categories.Remove(foundCategory);
            return Ok(Ok(ApiResponse<object>.SuccessResponse(null, 204, "Category deleted successfully")));
        }

        //PUT: /api/categories/{Id} => update a category
        [HttpPut("{Id:Guid}")]
        public IActionResult UpdateCategoriesById(Guid Id,  CategoryUpdateDto category){

                var foundCategory = categories.FirstOrDefault(category=>category.CategoryId == Id); 
                if(foundCategory == null){
                   return NotFound(ApiResponse<object>.ErrorResponse(new List<string>{"Category with this id does not exist"}, 404, "Validation failed" ));
                };


                foundCategory.Name = category.Name ?? foundCategory.Name;
                foundCategory.Description = category.Description ?? foundCategory.Description;

            
                return Ok(ApiResponse<object>.SuccessResponse(null, 204, "Categories updated successfully"));
        }
    }
}