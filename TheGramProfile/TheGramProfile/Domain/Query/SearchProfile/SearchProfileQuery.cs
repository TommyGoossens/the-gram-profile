using System.Collections.Generic;
using MediatR;
using TheGramProfile.Domain.DTO.Response;

namespace TheGramProfile.Domain.Query.SearchProfile
{
    public class SearchProfileQuery : IRequest<List<ProfileSearchResult>>
    {
        public string SearchTerm { get; set; }

        public SearchProfileQuery(string searchTerm)
        {
            SearchTerm = searchTerm;
        }
    }
}