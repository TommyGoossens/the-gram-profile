using System.Threading.Tasks;

namespace TheGramProfile.Helpers
{
    public interface IUserContextHelper
    {
        public Task<string> GetAuthToken();
        public string GetUserId();
    }
}