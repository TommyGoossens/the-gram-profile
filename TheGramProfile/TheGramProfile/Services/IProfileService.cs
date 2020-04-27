using System.Threading.Tasks;
using TheGramProfile.Properties.Models;
using TheGramProfile.Properties.Models.DTO;

namespace TheGramProfile.Services
{
    public interface IProfileService
    {
        Task<UserProfile> GetUser(string id);
        Task<UserProfile> CreateUser(string id, CreateProfileDTO profileDto);
    }
}