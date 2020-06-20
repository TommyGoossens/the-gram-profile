using System.Collections.Generic;
using MediatR;
using TheGramProfile.Domain.DTO.Response;

namespace TheGramProfile.Domain.Query.GetFollowers
{
    public class GetFollowedUsersProfilesQuery : IRequest<List<FollowerProfileResponse>>
    {
        public GetFollowedUsersProfilesQuery(string userId)
        {
            UserId = userId;
        }

        public string UserId { get; }
    }
}