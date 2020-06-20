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
        public string Username { get; set; } = "";
        public string ProfilePictureURL { get; set; } = "";

        public List<FollowerProfile> Following { get; set; } = new List<FollowerProfile>();

        public bool IsFollowingUserId(string userId)
        {
            return FindIndexOfFollowedUser(userId) != -1;
        }

        public bool RemoveFollower(string userId)
        {
            var index = FindIndexOfFollowedUser(userId);
            if (index == -1) return false;
            Following.RemoveAt(index);
            return true;
        }


        private int FindIndexOfFollowedUser(string userId)
        {
            return Following.FindIndex(f => f.UserId.Equals(userId));
        }

        public void AddFollower(string userId)
        {
            if (FindIndexOfFollowedUser(userId) == -1)
            {
                Following.Add(new FollowerProfile {UserId = userId});
            }
        }
    }
}