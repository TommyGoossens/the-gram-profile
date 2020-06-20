using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TheGramProfile.Domain.DTO.Response;
using TheGramProfile.Domain.Models;
using TheGramProfile.Repository;

namespace TheGramProfile.Domain.Query.SearchPaginatedProfiles
{
    public class SearchPaginatedProfilesHandler : IRequestHandler<SearchPaginatedProfilesQuery,PaginatedList<ProfileSearchResult>>
    {
        private readonly ProfileContext _profileContext;

        public SearchPaginatedProfilesHandler(ProfileContext profileContext)
        {
            _profileContext = profileContext;
        }
        
        public async Task<PaginatedList<ProfileSearchResult>> Handle(SearchPaginatedProfilesQuery request, CancellationToken cancellationToken)
        {
            return await PaginatedList<ProfileSearchResult>
                .CreateAsync(_profileContext.Profiles
                        .Where(p => p.Username.Contains(request.SearchTerm))
                        .Select(profile => new ProfileSearchResult
                        {
                            UserId = profile.UserId,
                            UserName = profile.Username,
                            ProfilePictureURL = profile.ProfilePictureURL
                        })
                        .AsNoTracking(),
                    request.PageNumber,
                    10,cancellationToken);
        }
    }
}