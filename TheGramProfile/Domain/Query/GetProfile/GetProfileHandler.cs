using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TheGramProfile.Domain.DTO.Response;
using TheGramProfile.Domain.Query.GetFollowers;
using TheGramProfile.Domain.Query.GetPostPreviews;
using TheGramProfile.Repository;

namespace TheGramProfile.Domain.Query.GetProfile
{
    public class GetProfileHandler : IRequestHandler<GetProfileQuery,ProfileResponse>
    {
        private readonly ProfileContext _profileContext;
        private readonly IMediator _mediator;

        public GetProfileHandler(ProfileContext profileContext, IMediator mediator)
        {
            _profileContext = profileContext;
            _mediator = mediator;
        }

        public async Task<ProfileResponse> Handle(GetProfileQuery request, CancellationToken cancellationToken)
        {
            var rpcPostsTask = _mediator.Send(new GetPostPreviewsQuery(request.UserId), cancellationToken);
            var followersTask =  _mediator.Send(new  GetFollowedUsersProfilesQuery(request.UserId), cancellationToken);
            var result = await _profileContext.Profiles.Where(p => p.UserId.Equals(request.UserId)).Select(profile => new ProfileResponse
            {
                UserId = profile.UserId,
                Email = profile.Email,
                FirstName = profile.FirstName,
                LastName = profile.LastName,
                UserName = profile.Username,
                ProfilePictureURL = profile.ProfilePictureURL,
                NumberOfFollowing = profile.Following.Count
            }).AsNoTracking().FirstOrDefaultAsync(cancellationToken: cancellationToken);
            
            result.Posts = await rpcPostsTask;
            var resultOfFollowersTask = await followersTask;
            result.NumberOfFollowers = resultOfFollowersTask.Count;
            return result;
        }
    }
}