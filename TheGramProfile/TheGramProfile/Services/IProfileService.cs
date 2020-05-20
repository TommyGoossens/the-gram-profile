using System.Collections.Generic;
using System.Threading.Tasks;
using TheGramProfile.Domain.DTO.Request;
using TheGramProfile.Domain.DTO.Response;
using TheGramProfile.Domain.Models.DTO;

namespace TheGramProfile.Services
{
    public interface IProfileService
    {
        Task<ProfileResponse> GetUser(string id);
        Task<ProfileCreatedResponse> CreateUser(string id, CreateProfileRequest profileRequest);
        Task<List<ProfileSearchResult>> QueryProfiles(string requestSearchTerm);
    }
}