using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Ecommerce_web_api.DTOs;
using Ecommerce_web_api.Models;
using Ecommerce_web_api.Services;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce_web_api.Controllers
{
    [ApiController]
    [Route("v1/api/categories/")]
    public class CategoryControllers : ControllerBase
    {


        private CategoryService _categoryService;

        public CategoryControllers(CategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        //GET: /api/categories/ => Read categories
        [HttpGet]
        public IActionResult GetCategories()
        {
            var categoryList = _categoryService.GetAllCategories();

            return Ok(ApiResponse<List<CategoryReadDto>>.SuccessResponse(categoryList, 200, "Categories returned successfully"));
        }



        //GET: /api/categories/ => Read categories by id
        [HttpGet("{Id:Guid}")]
        public IActionResult GetCategoriesById(Guid id)
        {
            var categoryReadDto = _categoryService.GetCategoryByIdServices(id);

            if (categoryReadDto == null)
            {
                return NotFound(ApiResponse<object>.ErrorResponse(new List<string> { "Category with this id does not exist" }, 404, "Validation failed"));
            }

            return Ok(ApiResponse<CategoryReadDto>.SuccessResponse(categoryReadDto, 200, "Category is returned successfully"));
        }


        //POST: /api/categories/ => Create categories
        [HttpPost]
        public IActionResult CreateCategories([FromBody] CategoryCreateDto categoryData)
        {
            var categoryReadDto = _categoryService.CreateCategoriesServices(categoryData);

            return Created(nameof(GetCategoriesById), ApiResponse<CategoryReadDto>.SuccessResponse(categoryReadDto, 201, "Categories Created successfully"));

        }


        //delete: /api/categories/{Id} => Delete a category
        [HttpDelete("{Id:Guid}")]
        public IActionResult DeleteCategories(Guid id){
            var foundCategory = _categoryService.DeleteCategoryByIdServices(id); 
            if(foundCategory == 404){
                return NotFound(ApiResponse<object>.ErrorResponse(new List<string>{"Category with this id does not exist"}, 404, "Validation failed" ));
            };

            return Ok(Ok(ApiResponse<object>.SuccessResponse(null, 204, "Category deleted successfully")));
        }

        //PUT: /api/categories/{Id} => update a category
        [HttpPut("{Id:Guid}")]
        public IActionResult UpdateCategoriesById(Guid Id, [FromBody] CategoryUpdateDto category){

                var foundCategory =  _categoryService.UpdateCategoryByIdServices(Id, category);
                if(foundCategory == null){
                   return NotFound(ApiResponse<object>.ErrorResponse(new List<string>{"Category with this id does not exist"}, 404, "Validation failed" ));
                };

                return Ok(ApiResponse<CategoryUpdateDto>.SuccessResponse(foundCategory, 204, "Categories updated successfully"));
        }
    }
}