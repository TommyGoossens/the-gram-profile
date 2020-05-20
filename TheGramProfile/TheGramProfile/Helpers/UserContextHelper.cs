using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using NLog;
using TheGramProfile.Helpers;

namespace TheGramPost.Helpers
{
    public class UserContextHelper : IUserContextHelper
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private readonly HttpContextAccessor _context;

        public UserContextHelper(IHttpContextAccessor context)
        {
            this._context = context as HttpContextAccessor;
        }


        public async Task<string> GetAuthToken()
        {
            try
            {
                return await _context.HttpContext.GetTokenAsync("access_token");;
            }
            catch (Exception e)
            {
                Logger.Error(e);
                throw;
            }
        }

        public string GetUserId()
        {
            try
            {
                return _context.HttpContext.User.Claims.First(i => i.Type.Equals("user_id")).Value;
            }
            catch (Exception e)
            {
                Logger.Error(e);
                throw;
            }
        }
    }
}