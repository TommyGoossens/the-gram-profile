namespace TheGramProfile.Domain.DTO.Request
{
    public class QueryProfilesRequest
    {
        public string SearchTerm { get; set; }
        public int PageNumber { get; set; }
    }
}