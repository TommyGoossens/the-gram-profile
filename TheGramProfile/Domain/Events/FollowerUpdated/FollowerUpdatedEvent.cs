using MediatR;

namespace TheGramProfile.Domain.Events.FollowerUpdated
{
    public class FollowerUpdatedEvent : INotification
    {
        public string UserIdInitiatedRequest { get; }
        public string FollowerUserId { get; }
        public bool IsFollowing { get; }

        public FollowerUpdatedEvent(string userIdInitiatedRequest, string followerUserId, bool isFollowing)
        {
            UserIdInitiatedRequest = userIdInitiatedRequest;
            FollowerUserId = followerUserId;
            IsFollowing = isFollowing;
        }
    }
}