using System.Collections.Generic;

namespace TheGramProfile.Domain.Models
{
    public class UserProfile
    {
        public long Id { get; set; }
        public string UserId { get; set; } = "";
        public string Email { get; set; } = "";
        public string FirstName { get; set; } = "";
        public string LastName { get; set; } = "";
        public string UserName { get; set; } = "";
        public string ProfilePictureURL { get; set; } = "";
        
        public List<FollowerProfile> Following { get; set; } = new List<FollowerProfile>();
    }
}