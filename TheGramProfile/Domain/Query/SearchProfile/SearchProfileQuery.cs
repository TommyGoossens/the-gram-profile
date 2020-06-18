using System.Collections.Generic;
using MediatR;
using TheGramProfile.Domain.DTO.Request;
using TheGramProfile.Domain.DTO.Response;
using TheGramProfile.Domain.Models;

namespace TheGramProfile.Domain.Query.SearchProfile
{
    public class SearchProfileQuery : IRequest<PaginatedList<ProfileSearchResult>>
    {
        public int PageNumber { get; set; }
        public string SearchTerm { get; set; }

        public SearchProfileQuery(QueryProfilesRequest request)
        {
            SearchTerm = request.SearchTerm;
            PageNumber = request.PageNumber;
        }
    }
}