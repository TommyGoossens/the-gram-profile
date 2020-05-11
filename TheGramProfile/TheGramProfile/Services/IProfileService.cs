using System.Threading.Tasks;
using TheGramProfile.Domain.Models;
using TheGramProfile.Domain.Models.DTO;

namespace TheGramProfile.Services
{
    public interface IProfileService
    {
        Task<ProfileResponse> GetUser(string id);
        Task<UserProfile> CreateUser(string id, CreateProfileRequest profileRequest);
    }
}