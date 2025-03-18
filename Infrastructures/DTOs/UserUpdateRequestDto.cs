namespace Infrastructures.DTOs
{
    public class UserUpdateRequestDto
    {
        public Guid Id { get; set; }
        public required string Username { get; set; }
        public required string Email { get; set; }
        public Guid RoleId { get; set; }
    }
}