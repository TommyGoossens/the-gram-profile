using MediatR;

namespace TheGramProfile.Domain.Commands.UpdateFollowerForUser
{
    public class UpdateFollowerForUserCommand : IRequest<bool>
    {
        public string UserId { get; }
        public string UserIdToFollow { get; }

        public UpdateFollowerForUserCommand(string userId, string userIdToFollow)
        {
            UserId = userId;
            UserIdToFollow = userIdToFollow;
        }
    }
}