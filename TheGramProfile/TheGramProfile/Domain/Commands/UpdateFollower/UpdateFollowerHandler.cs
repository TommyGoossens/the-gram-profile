using System.Threading;
using System.Threading.Tasks;
using MediatR;
using TheGramProfile.Services;

namespace TheGramProfile.Domain.Commands.UpdateFollower
{
    public class UpdateFollowerHandler : IRequestHandler<UpdateFollowerCommand, bool>
    {
        private readonly IProfileService _profileService;

        public UpdateFollowerHandler(IProfileService profileService)
        {
            _profileService = profileService;
        }

        public async Task<bool> Handle(UpdateFollowerCommand request, CancellationToken cancellationToken)
        {
         return await _profileService.UpdateFollowerForUser(request.UserId, request.UserIdToFollow);
        }
    }
}