using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TheGramProfile.Domain.Events.FollowerUpdated;
using TheGramProfile.Repository;

namespace TheGramProfile.Domain.Commands.UpdateFollowerForUser
{
    public class UpdateFollowerForUserHandler : IRequestHandler<UpdateFollowerForUserCommand,bool>
    {
        private readonly ProfileContext _profileContext;
        private readonly IMediator _mediator;

        public UpdateFollowerForUserHandler(ProfileContext profileContext, IMediator mediator)
        {
            _profileContext = profileContext;
            _mediator = mediator;
        }

        public async Task<bool> Handle(UpdateFollowerForUserCommand request, CancellationToken cancellationToken)
        {
            var profile = await _profileContext.Profiles.Where(p => p.UserId.Equals(request.UserId)).Include(p => p.Following)
                .FirstAsync(cancellationToken: cancellationToken);

            if (!profile.RemoveFollower(request.UserIdToFollow))
            {
                profile.AddFollower(request.UserIdToFollow);
            }
            var userIsNowFollowing = profile.IsFollowingUserId(request.UserIdToFollow);
            var notifyFollowerUpdateTask = _mediator.Publish(new FollowerUpdatedEvent(request.UserId, request.UserIdToFollow, userIsNowFollowing), cancellationToken);
            
            _profileContext.Profiles.Update(profile);
            await _profileContext.SaveChangesAsync(cancellationToken);
            
            await notifyFollowerUpdateTask;
            return userIsNowFollowing;
        }
    }
}