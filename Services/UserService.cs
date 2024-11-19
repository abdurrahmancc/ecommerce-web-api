using AutoMapper;
using Ecommerce_web_api.data;
using Ecommerce_web_api.DTOs.User;
using Ecommerce_web_api.Interfaces;
using Ecommerce_web_api.Models;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce_web_api.Services
{
    public class UserService : IUserService
    {
        private readonly IMapper _mapper;
        private readonly AppDbContext _appDbContext;

        public UserService(IMapper mapper, AppDbContext appDbContext)
        {
            _mapper = mapper;
            _appDbContext = appDbContext;
        }

        public async Task<List<UserReadDto>> GetAllUsersService()
        {
            IQueryable<UserModel> queryUsers = _appDbContext.Users;
            var usersList = await queryUsers.ToListAsync();
            return _mapper.Map<List<UserReadDto>>(usersList);
        }

        public async Task<UserReadDto> CreateUsersService(UserCreateDto userCreateData)
        {

            var newUserData = _mapper.Map<UserModel>(userCreateData);
            newUserData.Id = Guid.NewGuid(); 
            newUserData.CreatedAt = DateTime.UtcNow;
            await _appDbContext.Users.AddAsync(newUserData);
            await _appDbContext.SaveChangesAsync();

            return _mapper.Map<UserReadDto>(newUserData);
        }
    }
}
