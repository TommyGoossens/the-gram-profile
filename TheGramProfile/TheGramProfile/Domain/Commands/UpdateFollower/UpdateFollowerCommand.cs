using MediatR;
using TheGramProfile.Domain.DTO.Request;

namespace TheGramProfile.Domain.Commands.UpdateFollower
{
    public class UpdateFollowerCommand : IRequest<bool>
    {
        public string UserId { get; set; }
        public string UserIdToFollow { get; set; }

        public UpdateFollowerCommand(UpdateFollowerRequest request)
        {
            UserId = request.UserId;
            UserIdToFollow = request.UserIdToFollow;
        }
    }
}