using AutoMapper;
using Ecommerce_web_api.DTOs;
using Ecommerce_web_api.Models;

namespace Ecommerce_web_api.Profiles
{
    public class CategoryProfile: Profile
    {
        public CategoryProfile() {
            CreateMap<Category, CategoryReadDto>();
            CreateMap<CategoryCreateDto, Category>();
            CreateMap<CategoryUpdateDto, Category>();
        }
    }
}
