using MediatR;
using TheGramProfile.Domain.DTO.Request;

namespace TheGramProfile.Domain.Commands.UpdateFollower
{
    public class UpdateFollowerCommand : IRequest<bool>
    {
        public string UserName { get; set; }
        public string UserToFollow { get; set; }

        public UpdateFollowerCommand(UpdateFollowerRequest request)
        {
            UserName = request.UserName;
            UserToFollow = request.UserToFollow;
        }
    }
}