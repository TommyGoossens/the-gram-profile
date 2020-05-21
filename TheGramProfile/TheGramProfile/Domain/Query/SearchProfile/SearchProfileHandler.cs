using System.Threading;
using System.Threading.Tasks;
using MediatR;
using TheGramProfile.Domain.DTO.Response;
using TheGramProfile.Domain.Models;
using TheGramProfile.Services;

namespace TheGramProfile.Domain.Query.SearchProfile
{
    public class SearchProfileHandler : IRequestHandler<SearchProfileQuery,PaginatedList<ProfileSearchResult>>
    {
        private readonly IProfileService _profileService;

        public SearchProfileHandler(IProfileService profileService)
        {
            _profileService = profileService;
        }
        
        public async Task<PaginatedList<ProfileSearchResult>> Handle(SearchProfileQuery request, CancellationToken cancellationToken)
        {
            return await _profileService.QueryProfiles(request.SearchTerm,request.PageNumber);
        }
    }
}