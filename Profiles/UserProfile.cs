using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Ecommerce_web_api.DTOs.User;
using Ecommerce_web_api.Models;

namespace Ecommerce_web_api.Profiles
{
    public class UserProfile : Profile
    {
        public UserProfile() {
            CreateMap<UserModel, UserReadDto>();
            CreateMap<UserCreateDto, UserModel>();
        }
    }
}
