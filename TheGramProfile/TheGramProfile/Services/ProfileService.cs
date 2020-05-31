using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using NLog;
using TheGramProfile.Domain.DTO.Request;
using TheGramProfile.Domain.DTO.Response;
using TheGramProfile.Domain.Models;
using TheGramProfile.Repository;

namespace TheGramProfile.Services
{
    public class ProfileService : IProfileService
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private static bool _dataIsCreated = false;
        private readonly IMediator _mediator;
        private readonly ProfileContext _repo;

        public ProfileService(IMediator mediator, ProfileContext repo)
        {
            _mediator = mediator;
            _repo = repo; 
            if (!_dataIsCreated)
            {
                AddTempUser();
            }
        }

        public async Task<ProfileResponse> GetUser(string username)
        {
            //var rpcPostsTask =  _mediator.Send(new GetPostPreviewsQuery(id));
            var followersTask = GetFollowers(username);
            var result = await _repo.Profiles.Where(p => p.UserName.Equals(username)).Select(profile => new ProfileResponse
            {
                UserId = profile.UserId,
                Email = profile.Email,
                FirstName = profile.FirstName,
                LastName = profile.LastName,
                UserName = profile.UserName,
                ProfilePictureURL = profile.ProfilePictureURL,
                Following = profile.Following,
            }).AsNoTracking().FirstOrDefaultAsync();
            // result.Posts = await rpcPostsTask;
            result.Followers = await followersTask;
            return result;
        }

        private async Task<List<FollowerProfile>> GetFollowers(string username)
        {
            var result = await _repo.Profiles.Where(p => p.Following.Any(p => p.UserName.Equals(username))).Select(profile => new  FollowerProfile
                {
                    UserName = profile.UserName,
                    ProfilePictureURL = profile.ProfilePictureURL
                }).AsNoTracking()
                .ToListAsync();
            return result;
        }

        public async Task<ProfileCreatedResponse> CreateUser(string id, CreateProfileRequest profileRequest)
        {
            var user = await _repo.Profiles.Where(p => p.UserId.Equals(id)).FirstOrDefaultAsync();
            if (user != null) return null;

            user = new UserProfile
            {
                Email = profileRequest.Email,
                UserName = profileRequest.Username,
                UserId = id
            };
            await _repo.AddAsync(user);
            await _repo.SaveChangesAsync();
            return new ProfileCreatedResponse(user.UserId);
        }

        public async Task<PaginatedList<ProfileSearchResult>> QueryProfiles(string searchTerm, int pageNumber)
        {
            var result = await PaginatedList<ProfileSearchResult>
                .CreateAsync(_repo.Profiles
                        .Where(p => p.UserName.Contains(searchTerm))
                        .Select(profile => new ProfileSearchResult
                        {
                            UserId = profile.UserId,
                            UserName = profile.UserName,
                            ProfilePictureURL = profile.ProfilePictureURL
                        })
                        .AsNoTracking(),
                    pageNumber,
                    10);
            return result;
        }

        public async Task<bool> UpdateFollowerForUser(string userName, string follower)
        {
            var profile = await _repo.Profiles.Where(p => p.UserName.Equals(userName)).Include(p => p.Following)
                .FirstAsync();
            
            var index = profile.Following.FindIndex(f => f.UserName.Equals(follower));
            if (index == -1)
            {
                var userToFollow = await _repo.Profiles.Where(p => p.UserName.Equals(follower)).Select(p => new FollowerProfile
                    {
                        UserName = p.UserName,
                        ProfilePictureURL = p.ProfilePictureURL
                    })
                    .FirstAsync();
                profile.Following.Add(userToFollow);
            }
            else {
                profile.Following.RemoveAt(index);
            }

            _repo.Profiles.Update(profile);
            await _repo.SaveChangesAsync();
            
            return index == -1;
        }

        private async Task AddTempUser()
        {
            var list = new List<UserProfile>();
            for (var i = 0; i < 20; i++)
            {
                list.Add(new UserProfile
                {
                    UserName = $"tommy.goossens{i}",
                    Email = $"tommygoossens{i}@ziggo.nl",
                    UserId = $"UBh7cektzYhSu6s4s6IdEEsNfz6{i}",
                    ProfilePictureURL = "profilepic"
                });
            }

            await _repo.Profiles.AddRangeAsync(list);
            await _repo.SaveChangesAsync();
            _dataIsCreated = true;
        }
    }
}