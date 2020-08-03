using System.Collections.Generic;

namespace user.management.api.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public List<string> Roles { get; set; }
        public User(int id, string name, string email, List<string> roles)
        {
            Id = id;
            Name = name;
            Email = email;
            Roles = roles;
        }
    }
}
