using Ecommerce_web_api.DTOs;
using Ecommerce_web_api.Interfaces;
using Ecommerce_web_api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Writers;
using System.Threading.Tasks;

namespace Ecommerce_web_api.Controllers
{
    [ApiController]
    [Route("api/v1/blogs")]
    public class BlogController : ControllerBase // Inherit from ControllerBase
    {
        private readonly IBlogService _blogService;

        public BlogController(IBlogService blogService)
        {
            _blogService = blogService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllBlogs()
        {
            var result = await _blogService.GetAllBlogsService();

            if (result.Count < 1)
            {
                return NotFound(ApiResponse<object>.ErrorResponse(new List<string> { "Blogs are not founded" }, 404, "Validation failed"));
            }

            return Ok(ApiResponse<List<Blog>>.SuccessResponse(result, 200, "blogs returned success"));
        }

        [HttpGet("{Id:guid}")]
        public async Task<IActionResult> GetBlogById(Guid Id)
        {
            var result = await _blogService.GetBlogByIdService(Id);

            if (result == null)
            {
                return NotFound(ApiResponse<object>.ErrorResponse(new List<string> { $"Not found blog with this id: {Id} " }, 404, "Validation failed"));
            }

            return Ok(ApiResponse<Blog>.SuccessResponse(result, 200, "blog returned successfully"));
        }


        [HttpPost]
        public async Task<IActionResult> CreateBlog([FromBody] Blog blogInfo)
        {
            var result = await _blogService.CreateBlogService(blogInfo);

            if (result == null)
            {
                return BadRequest(ApiResponse<object>.ErrorResponse(new List<string> { "Failed to create blog." }, 400, "Validation failed"));
            }

            return CreatedAtAction(nameof(GetBlogById), new { Id = result.Id }, ApiResponse<Blog>.SuccessResponse(result, 201, "Blog created successfully"));
        }


        [HttpDelete]
        public async Task<IActionResult> DeleteBlog(Guid id)
        {
            var result = await _blogService.DeleteBlogService(id);

            if(result == 404)
            {
                return NotFound(ApiResponse<object>.ErrorResponse(new List<string> {$"Blog is not found in this id : {id}"}, 404, "Validation failed"));
            }

            return Ok(ApiResponse<object>.SuccessResponse(null, 204,"Blog is deleted successfully"));
        }


    }
}
