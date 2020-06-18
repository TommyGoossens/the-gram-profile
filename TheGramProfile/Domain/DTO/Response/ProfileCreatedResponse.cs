namespace TheGramProfile.Domain.DTO.Response
{
    public class ProfileCreatedResponse
    {
        public ProfileCreatedResponse(string userId)
        {
            Id = userId;
        }

        public string Id { get; set; }
    }
}