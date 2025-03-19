using Infrastructures.DTOs;
using Infrastructures.Models;
using Infrastructures.Repositories;
using Infrastructures.Exceptions;

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

        public async Task AddAsync(UserCreateRequestDto createDto)
        {
            var user = new User
            {
                Id = Guid.NewGuid(),
                Username = createDto.Username,
                Email = createDto.Email,
                RoleId = createDto.RoleId
            };
            await _userRepository.AddAsync(user);
        }

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            var users = await _userRepository.GetAllAsync();
            return users;
        }

        public async Task<User> GetByIdAsync(Guid id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null)
            {
                throw new NotFoundException($"User with ID {id} not found.");
            }
            return user;
        }

        public async Task DeleteAsync(Guid id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null)
            {
                throw new NotFoundException($"User with ID {id} not found.");
            }
            await _userRepository.DeleteAsync(id);
        }
    }
}