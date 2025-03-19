namespace Infrastructures.DTOs
{
    public class UserUpdateRequestDto
    {
        public required string Username { get; set; }
        public required string Email { get; set; }
        public Guid? RoleId { get; set; }
    }
}