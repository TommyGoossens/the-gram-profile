using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace TheGramProfile.Properties.Models
{
    public class UserProfile
    {
        public long Id { get; set; }
        public string UserId { get; set; } = "";
        public string Email { get; set; }= "";
        public string FirstName { get; set; }= "";
        public string LastName { get; set; }= "";
        public string UserName { get; set; }= "";
        public string ProfilePictureURL { get; set; }= "";

        [NotMapped]
        public List<string> Followers { get; set; } = new List<string>();
        [NotMapped]
        public List<string> Following { get; set; } = new List<string>();
    }
}