using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Threading.Tasks;
using Ecommerce_web_api.Models;

namespace Ecommerce_web_api.Interfaces
{
    public interface IBlogService
    {
        Task<Blog> CreateBlogService(Blog blogData);
        Task<List<Blog>?> GetAllBlogsService();
    }
}