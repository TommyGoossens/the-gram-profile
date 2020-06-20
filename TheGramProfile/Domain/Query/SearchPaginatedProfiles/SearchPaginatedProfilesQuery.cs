using MediatR;
using TheGramProfile.Domain.DTO.Request;
using TheGramProfile.Domain.DTO.Response;
using TheGramProfile.Domain.Models;

namespace TheGramProfile.Domain.Query.SearchPaginatedProfiles
{
    public class SearchPaginatedProfilesQuery : IRequest<PaginatedList<ProfileSearchResult>>
    {
        public int PageNumber { get; set; }
        public string SearchTerm { get; set; }

        public SearchPaginatedProfilesQuery(QueryProfilesRequest request)
        {
            SearchTerm = request.SearchTerm;
            PageNumber = request.PageNumber;
        }
    }
}