using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TheGramProfile.Domain.DTO.Response;
using TheGramProfile.Repository;

namespace TheGramProfile.Domain.Query.GetFollowers
{
    public class GetFollowedUsersProfileHandler : IRequestHandler<GetFollowedUsersProfilesQuery,List<FollowerProfileResponse>>
    {
        private readonly ProfileContext _profileContext;

        public GetFollowedUsersProfileHandler(ProfileContext profileContext)
        {
            _profileContext = profileContext;
        }


        public async Task<List<FollowerProfileResponse>> Handle(GetFollowedUsersProfilesQuery request, CancellationToken cancellationToken)
        {
            return await _profileContext.Profiles.Where(p => p.Following.Any(pr => pr.UserId.Equals(request.UserId))).Select(
                    profile => new FollowerProfileResponse(profile.UserId,profile.Username,profile.ProfilePictureURL)).AsNoTracking()
                .ToListAsync(cancellationToken: cancellationToken);
        }
    }
}