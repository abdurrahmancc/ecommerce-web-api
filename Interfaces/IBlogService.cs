using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Threading.Tasks;
using Ecommerce_web_api.Models;
using Ecommerce_web_api.DTOs;

namespace Ecommerce_web_api.Interfaces
{
    public interface IBlogService
    {
        Task<GenericResponse<Blog>> CreateBlogService(Blog blogData);
        Task<List<Blog>> GetAllBlogsService();
        Task<Blog?> GetBlogByIdService(Guid Id);
        Task<int> DeleteBlogService(Guid Id);
    }
}