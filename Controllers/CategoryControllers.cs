using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ecommerce_web_api.Models;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce_web_api.Controllers
{
    [ApiController]
    [Route("api/categories/")]
    public class CategoryControllers : ControllerBase
    {

        private static List<Category> categories = new List<Category>();



        //GET: /api/categories/ => Read categories
        [HttpGet]
        public IActionResult GetCategories([FromQuery] string searchValue = ""){
             Console.WriteLine($"{searchValue}");

            if(!string.IsNullOrEmpty(searchValue)){
            var searchCategories =   categories.Where(c=> c.Name.Contains(searchValue, StringComparison.OrdinalIgnoreCase)).ToList();
                return Ok(searchCategories);
            }
            return Ok(categories);
        }
        



        
        //POST: /api/categories/ => Create categories
        [HttpPost]
        public IActionResult CreateCategories([FromBody] Category categoryData){
                    
            if(string.IsNullOrEmpty(categoryData.Name)){
                return BadRequest("Category Name is required and can not be empty");
            };

            var newCategory = new Category{
                CategoryId = Guid.NewGuid(),
                Name = categoryData.Name,
                Description = categoryData.Description,
                CreateAt = DateTime.UtcNow
            };

            categories.Add(newCategory);
            return Created($"/api/categories/{newCategory.CategoryId}",newCategory);

        }


        //delete: /api/categories/{Id} => Delete a category
        [HttpDelete("{Id:Guid}")]
        public IActionResult DeleteCategories(Guid id){
            var foundCategory = categories.FirstOrDefault(category=>category.CategoryId == id ); 
            if(foundCategory == null){
                return NotFound("Category wiht this id does not exist");
            };
            categories.Remove(foundCategory);
            return NoContent();
        }

        //PUT: /api/categories/{Id} => update a category
        [HttpPut("{Id:Guid}")]
        public IActionResult UpdateCategoriesById(Guid Id, [FromBody] Category category){
                Console.WriteLine($"{category}");
                var foundCategory = categories.FirstOrDefault(category=>category.CategoryId == Id); 
                if(foundCategory == null){
                    return NotFound("Category wiht this id does not exist");
                };

                if(category == null){
                    return NotFound("Category data is missing");
                };
                if(!string.IsNullOrEmpty(category.Name)){
                    foundCategory.Name = category.Name ?? foundCategory.Name;
                };
                if(!string.IsNullOrEmpty(category.Description)){
                    foundCategory.Description = category.Description ?? foundCategory.Description;
                };
            
                return Ok(categories);
        }
    }
}