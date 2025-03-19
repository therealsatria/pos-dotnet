using Infrastructures.DTOs;
using Infrastructures.Models;
using Infrastructures.Repositories;
using Infrastructures.Exceptions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Infrastructures.Services
{
    public class UserService : GenericService<User>
    {
        private readonly IGenericRepository<User> _userRepository;
        private readonly IGenericRepository<Role> _roleRepository;

        public UserService(
            IGenericRepository<User> userRepository, 
            IGenericRepository<Role> roleRepository) : base(userRepository)
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _roleRepository = roleRepository ?? throw new ArgumentNullException(nameof(roleRepository));
        }

        public async Task<User> UpdateUserAsync(User user, UserUpdateRequestDto updateDto)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));
            
            if (updateDto == null)
                throw new ValidationException("Update data cannot be null");

            if (updateDto.RoleId.HasValue)
            {
                // Verify that role exists
                try
                {
                    await _roleRepository.GetByIdAsync(updateDto.RoleId.Value);
                }
                catch (NotFoundException)
                {
                    throw new ValidationException($"Role with ID {updateDto.RoleId} not found");
                }
            }

            user.Username = updateDto.Username;
            user.Email = updateDto.Email;
            
            if (updateDto.RoleId.HasValue)
            {
                user.RoleId = updateDto.RoleId.Value;
            }
            
            await _userRepository.UpdateAsync(user);
            return user;
        }

        public async Task<User> AddAsync(UserCreateRequestDto createDto)
        {
            if (createDto == null)
                throw new ValidationException("Create data cannot be null");

            if (!createDto.RoleId.HasValue)
                throw new ValidationException("RoleId is required");

            // Verify that role exists
            try
            {
                await _roleRepository.GetByIdAsync(createDto.RoleId.Value);
            }
            catch (NotFoundException)
            {
                throw new ValidationException($"Role with ID {createDto.RoleId} not found");
            }

            // Check if username already exists
            var existingUser = await _userRepository.GetQueryable()
                .FirstOrDefaultAsync(u => u.Username == createDto.Username);
                
            if (existingUser != null)
                throw new ValidationException($"Username '{createDto.Username}' is already taken");

            // Check if email already exists
            existingUser = await _userRepository.GetQueryable()
                .FirstOrDefaultAsync(u => u.Email == createDto.Email);
                
            if (existingUser != null)
                throw new ValidationException($"Email '{createDto.Email}' is already registered");

            var user = new User
            {
                Id = Guid.NewGuid(),
                Username = createDto.Username,
                Email = createDto.Email,
                RoleId = createDto.RoleId.Value
            };
            
            await _userRepository.AddAsync(user);
            return user;
        }

        public override async Task<IEnumerable<User>> GetAllAsync()
        {
            var users = await _userRepository.GetQueryable()
                .Include(u => u.Role)
                .ToListAsync();
                
            return users;
        }

        public override async Task<User> GetByIdAsync(Guid id)
        {
            if (id == Guid.Empty)
                throw new ValidationException("User ID cannot be empty");
                
            var user = await _userRepository.GetQueryable()
                .Include(u => u.Role)
                .FirstOrDefaultAsync(u => u.Id == id);
                
            if (user == null)
                throw new NotFoundException($"User with ID {id} not found");
                
            return user;
        }

        public new async Task DeleteAsync(Guid id)
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