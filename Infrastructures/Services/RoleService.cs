using Infrastructures.DTOs;
using Infrastructures.Models;
using Infrastructures.Repositories;


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

    }
}