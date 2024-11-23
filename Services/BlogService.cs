using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ecommerce_web_api.data;
using Ecommerce_web_api.Interfaces;
using Ecommerce_web_api.Models;
using Microsoft.EntityFrameworkCore;

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


        public async Task<Blog> CreateBlogService(Blog blogData)
        {
            try
            {
                Console.WriteLine($"Blog Title: {blogData.Title}, Description: {blogData.Description}");
                Console.WriteLine("Saving Blog...");
                await _appDbContext.Blogs.AddAsync(blogData);
                await _appDbContext.SaveChangesAsync();
                Console.WriteLine($"Blog saved successfully with Id: {blogData.Id}");
                return blogData;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                throw;
            }
        }



    }
}