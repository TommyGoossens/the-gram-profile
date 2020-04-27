using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NLog;
using TheGramProfile.Properties.Models;
using TheGramProfile.Properties.Models.DTO;
using TheGramProfile.Services;

namespace TheGramProfile.Controllers
{
    public class ProfileController : AbstractProfileController
    {

        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private readonly IProfileService _profileService;
        public ProfileController(IProfileService profileService)
        {
            _profileService = profileService;
        }
        [HttpGet("{userId}")]
        public async Task<UserProfile> GetProfile(string userId)
        {
            return await _profileService.GetUser(userId);

        }

        [HttpPost("{userId}")]
        public async Task<ActionResult> CreateProfile(string userId, [FromBody] CreateProfileDTO createProfileDto)
        {
            UserProfile createdProfile = new UserProfile
            {
                UserName = createProfileDto.Username,
                Email = createProfileDto.Email
            };
            return new OkResult();
        }

        [HttpPut("{userId}")]
        public Task<ActionResult> UpdateProfile(string userId)
        {
            throw new NotImplementedException();
        }
    }
}