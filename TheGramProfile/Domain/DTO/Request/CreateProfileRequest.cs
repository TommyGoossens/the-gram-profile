namespace TheGramProfile.Domain.DTO.Request
{
    public class CreateProfileRequest
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
}