using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Ecommerce_web_api.DTOs;
using Ecommerce_web_api.Interfaces;
using Ecommerce_web_api.Models;
using Ecommerce_web_api.Services;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce_web_api.Controllers
{
    [ApiController]
    [Route("v1/api/categories/")]
    public class CategoryControllers : ControllerBase
    {


        private ICategoryServices _categoryService;

        public CategoryControllers(ICategoryServices categoryService)
        {
            _categoryService = categoryService;
        }

        //GET: /api/categories/ => Read categories
        [HttpGet]
        public async Task<IActionResult> GetCategories()
        {
            var categoryList = await _categoryService.GetAllCategoriesService();

            return Ok(ApiResponse<List<CategoryReadDto>>.SuccessResponse(categoryList, 200, "Categories returned successfully"));
        }



        //GET: /api/categories/ => Read categories by id
        [HttpGet("{Id:Guid}")]
        public async Task<IActionResult> GetCategoriesById(Guid id)
        {
            var categoryReadDto =await  _categoryService.GetCategoryByIdServices(id);

            if (categoryReadDto == null)
            {
                return NotFound(ApiResponse<object>.ErrorResponse(new List<string> { "Category with this id does not exist" }, 404, "Validation failed"));
            }

            return Ok(ApiResponse<CategoryReadDto>.SuccessResponse(categoryReadDto, 200, "Category is returned successfully"));
        }


        //POST: /api/categories/ => Create categories
        [HttpPost]
        public async Task<IActionResult> CreateCategories([FromBody] CategoryCreateDto categoryData)
        {
            var categoryReadDto = await _categoryService.CreateCategoriesServices(categoryData);

            return Created(nameof(GetCategoriesById), ApiResponse<CategoryReadDto>.SuccessResponse(categoryReadDto, 201, "Categories Created successfully"));

        }


        //delete: /api/categories/{Id} => Delete a category
        [HttpDelete("{Id:Guid}")]
        public async Task<IActionResult> DeleteCategories(Guid id){
            var foundCategory = await _categoryService.DeleteCategoryByIdServices(id); 
            if(foundCategory == 404){
                return NotFound(ApiResponse<object>.ErrorResponse(new List<string>{"Category with this id does not exist"}, 404, "Validation failed" ));
            };

            return Ok(Ok(ApiResponse<object>.SuccessResponse(null, 204, "Category deleted successfully")));
        }

        //PUT: /api/categories/{Id} => update a category
        [HttpPut("{Id:Guid}")]
        public async Task<IActionResult> UpdateCategoriesById(Guid Id,  CategoryUpdateDto category){

                var foundCategory = await  _categoryService.UpdateCategoryByIdServices(Id, category);
                if(foundCategory == null){
                   return NotFound(ApiResponse<object>.ErrorResponse(new List<string>{"Category with this id does not exist"}, 404, "Validation failed" ));
                };

                return Ok(ApiResponse<CategoryReadDto>.SuccessResponse(foundCategory, 204, "Categories updated successfully"));
        }
    }
}