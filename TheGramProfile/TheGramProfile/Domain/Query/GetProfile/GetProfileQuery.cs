using MediatR;
using TheGramProfile.Domain.Models.DTO;

namespace TheGramProfile.Domain.Query.GetProfile
{
    public class GetProfileQuery : IRequest<ProfileResponse>
    {
        public GetProfileQuery(string userId)
        {
            UserId = userId;
        }

        public string UserId { get; set; }
    }
}