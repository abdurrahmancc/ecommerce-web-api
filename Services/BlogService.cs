using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ecommerce_web_api.data;
using Ecommerce_web_api.Interfaces;
using Ecommerce_web_api.Models;

namespace Ecommerce_web_api.Services
{
    public class BlogService: IBlogService
    {
        private readonly AppDbContext _appDbContext;
                public BlogService( AppDbContext appDbContext)
        {

            _appDbContext = appDbContext;
        }
        public async Task<Blog> CreateBlogService(Blog blogData){
            await _appDbContext.Blogs.AddAsync(blogData);
            await _appDbContext.SaveChangesAsync();
            return blogData;
        }
    }
}