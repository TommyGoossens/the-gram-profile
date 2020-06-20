using System.Collections.Generic;
using TheGramProfile.Domain.Models.DTO;

namespace TheGramProfile.Domain.DTO.Response
{
    public class ProfileResponse
    {
        public string UserId { get; set; } = "";
        public string Email { get; set; } = "";
        public string FirstName { get; set; } = "";
        public string LastName { get; set; } = "";
        public string UserName { get; set; } = "";
        public string ProfilePictureURL { get; set; } = "";

        public int NumberOfFollowers { get; set; }
        public int NumberOfFollowing { get; set; }

        public List<PostPreviewResponse> Posts = new List<PostPreviewResponse>();
    }
}