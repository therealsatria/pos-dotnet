using Infrastructures.DTOs;
using Infrastructures.Models;
using Infrastructures.Repositories;

namespace Infrastructures.Services
{
    public class UserService : GenericService<User>
    {
        private readonly IGenericRepository<User> _userRepository;

        public UserService(IGenericRepository<User> userRepository) : base(userRepository)
        {
            _userRepository = userRepository;
        }
        public async Task UpdateUserAsync(User user, UserUpdateRequestDto updateDto)
        {
            user.Username = updateDto.Username;
            user.Email = updateDto.Email;
            user.RoleId = updateDto.RoleId;
            await _userRepository.UpdateAsync(user);
        }
    }
}