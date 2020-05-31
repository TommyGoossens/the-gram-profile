namespace TheGramProfile.Domain.Models
{
    public class FollowerProfile
    {
        public long Id { get; set; }
        public string UserName { get; set; }
        public string ProfilePictureURL { get; set; }

        public FollowerProfile()
        {
        }
    }
}