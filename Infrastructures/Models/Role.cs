namespace Infrastructures.Models
{
    public class Role
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }
        public ICollection<User>? Users { get; set; }
    }
}