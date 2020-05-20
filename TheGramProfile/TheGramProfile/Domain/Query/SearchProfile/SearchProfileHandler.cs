using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using TheGramProfile.Domain.DTO.Response;
using TheGramProfile.Services;

namespace TheGramProfile.Domain.Query.SearchProfile
{
    public class SearchProfileHandler : IRequestHandler<SearchProfileQuery,List<ProfileSearchResult>>
    {
        private readonly IProfileService _profileService;

        public SearchProfileHandler(IProfileService profileService)
        {
            _profileService = profileService;
        }
        
        public async Task<List<ProfileSearchResult>> Handle(SearchProfileQuery request, CancellationToken cancellationToken)
        {
            return await _profileService.QueryProfiles(request.SearchTerm);
        }
    }
}