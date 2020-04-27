using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NLog;
using TheGramPost.Helpers;
using TheGramProfile.Properties.Models;
using TheGramProfile.Properties.Models.DTO;
using TheGramProfile.Repository;

namespace TheGramProfile.Services
{
    public class ProfileService : IProfileService
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private readonly ProfileContext _repo;
        private readonly IUserContextHelper _userContext;

        public ProfileService(ProfileContext repo, IUserContextHelper userContextHelper)
        {
            _repo = repo;
            _userContext = userContextHelper;
            AddTempUser("UBh7cektzYhSu6s4s6IdEEsNfz63");
        }

        public async Task<UserProfile> GetUser(string id)
        {
            var token = await _userContext.GetAuthToken();
            Console.WriteLine(token);
            //if (!id.Equals(_userContext.GetUserId())) return null;
            var result =await _repo.Profiles.Where(p => p.UserId.Equals(id)).FirstOrDefaultAsync();
            return result;
        }

        public async Task<UserProfile> CreateUser(string id, CreateProfileDTO profileDto)
        {
            var userId = _userContext.GetUserId();
            var user = await _repo.Profiles.Where(p => p.UserId.Equals(userId)).FirstOrDefaultAsync();
            if (user == null) return null;
            
            user = new UserProfile
            {
                Email = profileDto.Email,
                UserName = profileDto.Username,
                UserId = userId
            };
            await _repo.AddAsync(user);
            await _repo.SaveChangesAsync();
            return user;
        }

        public async Task AddTempUser(string id)
        {
            await _repo.Profiles.AddAsync(new UserProfile
            {
                UserName = "Tommy.Goossens",
                Email = "tommygoossens@ziggo.nl",
                UserId = id
            });
            await _repo.SaveChangesAsync();
        }
    }
}