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
    public class RoleService : GenericService<Role>
    {
        private readonly IGenericRepository<Role> _roleRepository;

        public RoleService(IGenericRepository<Role> roleRepository) : base(roleRepository)
        {
            _roleRepository = roleRepository ?? throw new ArgumentNullException(nameof(roleRepository));
        }

        public async Task<Role> UpdateRoleAsync(Role role, RoleUpdateRequestDto updateDto)
        {
            if (role == null)
                throw new ArgumentNullException(nameof(role));
            
            if (updateDto == null)
                throw new ValidationException("Update data cannot be null");

            // Check if name already exists
            var existingRole = await _roleRepository.GetQueryable()
                .FirstOrDefaultAsync(r => r.Name == updateDto.Name && r.Id != role.Id);
                
            if (existingRole != null)
                throw new ValidationException($"Role name '{updateDto.Name}' is already taken");

            role.Name = updateDto.Name;
            await _roleRepository.UpdateAsync(role);
            return role;
        }

        public async Task<Role> AddAsync(RoleCreateRequestDto createDto)
        {
            if (createDto == null)
                throw new ValidationException("Create data cannot be null");

            // Check if name already exists
            var existingRole = await _roleRepository.GetQueryable()
                .FirstOrDefaultAsync(r => r.Name == createDto.Name);
                
            if (existingRole != null)
                throw new ValidationException($"Role name '{createDto.Name}' is already taken");

            var role = new Role
            {
                Id = Guid.NewGuid(),
                Name = createDto.Name
            };
            
            await _roleRepository.AddAsync(role);
            return role;
        }

        public override async Task<IEnumerable<Role>> GetAllAsync()
        {
            var roles = await _roleRepository.GetQueryable()
                .ToListAsync();
                
            return roles;
        }

        public override async Task<Role> GetByIdAsync(Guid id)
        {
            if (id == Guid.Empty)
                throw new ValidationException("Role ID cannot be empty");
                
            var role = await _roleRepository.GetQueryable()
                .FirstOrDefaultAsync(r => r.Id == id);
                
            if (role == null)
                throw new NotFoundException($"Role with ID {id} not found");
                
            return role;
        }

        public new async Task DeleteAsync(Guid id)
        {
            var role = await _roleRepository.GetByIdAsync(id);
            if (role == null)
            {
                throw new NotFoundException($"Role with ID {id} not found.");
            }
            await _roleRepository.DeleteAsync(id);
        }
    }
}