using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ecommerce_web_api.Interfaces;
using Ecommerce_web_api.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Ecommerce_web_api.Controllers
{
    [ApiController]
    [Route("v1/api/blogs/")]
    public class BlogControllers : ControllerBase
    {

        private IBlogService _BlogService;

        public BlogControllers(IBlogService blogService)
        {
            _BlogService = blogService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllBlogs()
        {
            var result = await _BlogService.GetAllBlogsService();


            if (result == null || !result.Any())
            {
                return NotFound(ApiResponse<object>.ErrorResponse(new List<string> { "Blogs are not found" }, 404, "Validation failed"));
            }
            return Ok(ApiResponse<List<Blog>?>.SuccessResponse(result, 200, "blogs returned successfully"));
        }



        [HttpPost]
        public async Task<IActionResult> CreateBlogs([FromBody] Blog blogData)
        {
            var result = await _BlogService.CreateBlogService(blogData);
            return Ok(ApiResponse<Blog>.SuccessResponse(result, 200, "blog created successfully"));
        }





    }
}