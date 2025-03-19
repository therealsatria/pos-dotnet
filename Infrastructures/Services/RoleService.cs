using Infrastructures.DTOs;
using Infrastructures.Models;
using Infrastructures.Repositories;
using Infrastructures.Exceptions;

namespace Infrastructures.Services
{
    public class RoleService : GenericService<Role>
    {
        private readonly IGenericRepository<Role> _roleRepository;

        public RoleService(IGenericRepository<Role> roleRepository) : base(roleRepository)
        {
            _roleRepository = roleRepository;
        }

        public async Task UpdateRoleAsync(Role role, RoleUpdateRequestDto updateDto)
        {
            role.Name = updateDto.Name;
            await _roleRepository.UpdateAsync(role);
        }

        public async Task AddAsync(RoleCreateRequestDto createDto)
        {
            var role = new Role
            {
                Id = Guid.NewGuid(),
                Name = createDto.Name
            };
            await _roleRepository.AddAsync(role);
        }

        public new async Task<IEnumerable<Role>> GetAllAsync()
        {
            var roles = await _roleRepository.GetAllAsync();
            return roles;
        }

        public new async Task<Role> GetByIdAsync(Guid id)
        {
            var role = await _roleRepository.GetByIdAsync(id);
            if (role == null)
            {
                throw new NotFoundException($"Role with ID {id} not found.");
            }
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