using System.Threading.Tasks;

namespace TheGramPost.Helpers
{
    public interface IUserContextHelper
    {
        public Task<string> GetAuthToken();
        public string GetUserId();
    }
}