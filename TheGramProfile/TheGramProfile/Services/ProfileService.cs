using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using NLog;
using TheGramPost.Helpers;
using TheGramProfile.Domain.Models;
using TheGramProfile.Domain.Models.DTO;
using TheGramProfile.Domain.Query.GetPostPreviews;
using TheGramProfile.Repository;

namespace TheGramProfile.Services
{
    public class ProfileService : IProfileService
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private readonly IMediator _mediator;
        private readonly ProfileContext _repo;
        private readonly IUserContextHelper _userContext;
        public ProfileService(IMediator mediator,ProfileContext repo, IUserContextHelper userContextHelper)
        {
            _mediator = mediator;
            _repo = repo;
            _userContext = userContextHelper;
            AddTempUser();
        }

        public async Task<ProfileResponse> GetUser(string id)
        {
            var token = await _userContext.GetAuthToken();
            
            if (!id.Equals(_userContext.GetUserId()))
            {
                Logger.Info("UserId param {0} is not the same as the auth userId {1}",id,_userContext.GetUserId());
                return null;
            }

            var rpcPosts =  await _mediator.Send(new GetPostPreviewsQuery(id));
            
            var result = await _repo.Profiles.Where(p => p.UserId.Equals(id)).Select(profile => new ProfileResponse
            {
                UserId = profile.UserId,
                Email = profile.Email,
                FirstName = profile.FirstName,
                LastName = profile.LastName,
                UserName = profile.UserName,
                ProfilePictureURL = profile.ProfilePictureURL,
                Followers = profile.Followers,
                Following = profile.Following,
                Posts = rpcPosts
            }).FirstOrDefaultAsync();
            
            return result;
        }

        public async Task<UserProfile> CreateUser(string id, CreateProfileRequest profileRequest)
        {
            var userId = _userContext.GetUserId();
            var user = await _repo.Profiles.Where(p => p.UserId.Equals(userId)).FirstOrDefaultAsync();
            if (user == null) return null;
            
            user = new UserProfile
            {
                Email = profileRequest.Email,
                UserName = profileRequest.Username,
                UserId = userId
            };
            await _repo.AddAsync(user);
            await _repo.SaveChangesAsync();
            return user;
        }

        private async Task AddTempUser()
        {
            await _repo.Profiles.AddAsync(new UserProfile
            {
                UserName = "Tommy.Goossens",
                Email = "tommygoossens@ziggo.nl",
                UserId = "UBh7cektzYhSu6s4s6IdEEsNfz63"
            });
            await _repo.SaveChangesAsync();
        }
    }
}