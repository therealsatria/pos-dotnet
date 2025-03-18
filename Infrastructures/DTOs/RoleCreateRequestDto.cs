namespace Infrastructures.DTOs
{
    public class RoleCreateRequestDto
    {
        public required string Name { get; set; }
        public required ICollection<UserCreateRequestDto> Users { get; set; }

        public RoleCreateRequestDto()
        {
            Users = new List<UserCreateRequestDto>();
        }
    }
}