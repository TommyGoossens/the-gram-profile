namespace TheGramProfile.Domain.DTO.Response
{
    public class FollowerProfileResponse
    {
        public string UserId { get; set; }
        public string UserName { get; set; }

        public string ProfilePictureURL { get; set; }

        public FollowerProfileResponse(string userId, string userName, string profilePictureURL)
        {
            UserId = userId;
            UserName = userName;
            ProfilePictureURL = profilePictureURL;
        }
    }
}