using System.Threading.Tasks;
using TheGramProfile.Domain.DTO.Request;
using TheGramProfile.Domain.DTO.Response;
using TheGramProfile.Domain.Models;

namespace TheGramProfile.Services
{
    public interface IProfileService
    {
        Task<ProfileResponse> GetUser(string id);
        Task<ProfileCreatedResponse> CreateUser(string id, CreateProfileRequest profileRequest);
        Task<PaginatedList<ProfileSearchResult>> QueryProfiles(string searchTerm,int pageNumber);
        Task<bool> UpdateFollowerForUser(string userName, string follower);
    }
}