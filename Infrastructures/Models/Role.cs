namespace Infrastructures.Models
{
    public class Role
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }
        public required ICollection<User> Users { get; set; }
    }
}