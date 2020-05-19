using System.Threading;
using System.Threading.Tasks;
using MediatR;
using TheGramProfile.Domain.DTO.Response;
using TheGramProfile.Domain.Models.DTO;
using TheGramProfile.Services;

namespace TheGramProfile.Domain.Query.GetProfile
{
    public class GetProfileHandler : IRequestHandler<GetProfileQuery,ProfileResponse>
    {
        private readonly IProfileService _profileService;

        public GetProfileHandler(IProfileService profileService)
        {
            _profileService = profileService;
        }

        public async Task<ProfileResponse> Handle(GetProfileQuery request, CancellationToken cancellationToken)
        {
            return await _profileService.GetUser(request.UserId);
        }
    }
}