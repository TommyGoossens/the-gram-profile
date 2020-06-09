namespace TheGramProfile.Domain.DTO.Request
{
    public class UpdateFollowerRequest
    {
        public string UserId { get; set; }
        public string UserIdToFollow { get; set; }

        public UpdateFollowerRequest(string userId, string userIdToFollow)
        {
            UserId = userId;
            UserIdToFollow = userIdToFollow;
        }
    }
}