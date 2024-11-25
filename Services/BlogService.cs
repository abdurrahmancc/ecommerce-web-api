using System;
using Ecommerce_web_api.data;
using Ecommerce_web_api.DTOs;
using Ecommerce_web_api.Interfaces;
using Ecommerce_web_api.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace Ecommerce_web_api.Services
{
    public class BlogService : IBlogService
    {
        private readonly AppDbContext _appDbContext;
        public BlogService(AppDbContext appDbContext)
        {

            _appDbContext = appDbContext;
        }
        public async Task<List<Blog>?> GetAllBlogsService()
        {
            return await _appDbContext.Blogs.Include(b => b.Posts).ToListAsync();

        }


        public async Task<Blog?> GetBlogByIdService(Guid Id)
        {
            return await _appDbContext.Blogs
                               .Include(b => b.Posts)
                               .FirstOrDefaultAsync(b => b.Id == Id);
        }

        public async Task<GenericResponse<Blog>> CreateBlogService(Blog blogData)
        {
            try
            {
                if (blogData.Id == Guid.Empty )
                {
                    blogData.Id = Guid.NewGuid();
                }
                foreach (var post in blogData.Posts)
                {
                    if (post.Id == Guid.Empty)
                    {
                        post.Id = Guid.NewGuid();
                        post.BlogId = blogData.Id;
                    }
                }

                _appDbContext.Blogs.Add(blogData);
                await _appDbContext.SaveChangesAsync();
                return GenericResponse<Blog>.SuccessResponse(blogData); ;
            }
            catch (Exception ex)
            {
                if (ex.InnerException?.Message.Contains("duplicate key") == true)
                {
                    return GenericResponse<Blog>.ErrorResponse( "A record with the same ID already exists. Please use a unique ID.");
                }

                return GenericResponse<Blog>.ErrorResponse( ex.InnerException?.Message ?? "An unknown database error occurred.");
            }
        }


        public async Task<int> DeleteBlogService(Guid Id)
        {
            try
            {
              var result = await _appDbContext.Blogs.FindAsync(Id);

                if (result == null)
                {
                    return 404;
                }
                _appDbContext.Remove(result);
                await _appDbContext.SaveChangesAsync();
                return 202;
            }
            catch (Exception ex)
            {
               Console.WriteLine($"an error occurred: {ex.Message}");
                throw new InvalidOperationException("Failed to create the blog", ex);
            }
        }



    }
}