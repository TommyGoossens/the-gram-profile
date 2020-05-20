using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using TheGramProfile.Domain.Models.DTO;

namespace TheGramProfile.Domain.DTO.Response
{
    public class ProfileResponse
    {
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
        
        [NotMapped]
        public List<PostPreviewResponse> Posts = new List<PostPreviewResponse>();
    }
}