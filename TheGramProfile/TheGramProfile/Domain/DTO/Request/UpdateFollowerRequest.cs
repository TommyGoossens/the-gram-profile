namespace TheGramProfile.Domain.DTO.Request
{
    public class UpdateFollowerRequest
    {
        public string UserName { get; set; }
        public string UserToFollow { get; set; }

        public UpdateFollowerRequest(string userName, string userToFollow)
        {
            UserName = userName;
            UserToFollow = userToFollow;
        }
    }
}