namespace Infrastructures.Models
{
    public class User
    {
        public Guid Id { get; set; }
        public required string Username { get; set; }
        public required string Email { get; set; }
        public Guid RoleId { get; set; }
        public required Role Role { get; set; }
    }
}