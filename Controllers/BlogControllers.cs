using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ecommerce_web_api.Interfaces;
using Ecommerce_web_api.Models;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce_web_api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BlogControllers : ControllerBase
    {

        private IBlogService _BlogService;

        public BlogControllers(IBlogService blogService)
        {
            _BlogService = blogService;
        }


        [HttpPost] 
        public async Task<IActionResult> CreateBlogs([FromBody] Blog blogData){
            var result = await _BlogService.CreateBlogService(blogData);
            return Ok(ApiResponse<Blog>.SuccessResponse(result, 200, "Categories returned successfully"));
        }
    }
}