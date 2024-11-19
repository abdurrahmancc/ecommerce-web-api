using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ecommerce_web_api.DTOs.User;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce_web_api.Interfaces
{
    public interface IUserService
    {
        Task<List<UserReadDto>> GetAllUsersService();
        Task<UserReadDto> CreateUsersService([FromBody] UserCreateDto userCreateData);
    }
}