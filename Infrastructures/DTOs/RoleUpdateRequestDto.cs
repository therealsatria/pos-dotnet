namespace Infrastructures.DTOs
{
    public class RoleUpdateRequestDto
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }
        public required ICollection<UserCreateRequestDto> Users { get; set; }

        public RoleUpdateRequestDto()
        {
            Users = new List<UserCreateRequestDto>();
        }
    }
}