using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using NLog;
using TheGramProfile.Domain.DTO.Request;
using TheGramProfile.Domain.DTO.Response;
using TheGramProfile.Domain.Models;
using TheGramProfile.Domain.Query.GetPostPreviews;
using TheGramProfile.Helpers;
using TheGramProfile.Repository;

namespace TheGramProfile.Services
{
    public class ProfileService : IProfileService
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private static bool _dataIsCreated = false;
        private readonly IMediator _mediator;
        private readonly ProfileContext _repo;
        private readonly IUserContextHelper _userContext;
        public ProfileService(IMediator mediator,ProfileContext repo, IUserContextHelper userContextHelper)
        {
            _mediator = mediator;
            _repo = repo;
            _userContext = userContextHelper;
            if (!_dataIsCreated)
            {
                AddTempUser();    
            }
            
        }

        public async Task<ProfileResponse> GetUser(string id)
        {
            var token = await _userContext.GetAuthToken();
            
            if (!id.Equals(_userContext.GetUserId()))
            {
                Logger.Info("UserId param {0} is not the same as the auth userId {1}",id,_userContext.GetUserId());
                return null;
            }
            Logger.Info("Getting posts");
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

        public async Task<List<ProfileSearchResult>> QueryProfiles(string searchTerm)
        {
            var result = await _repo.Profiles.Where(p => p.UserName.Contains(searchTerm)).Select(profile =>
                new ProfileSearchResult
                {
                    UserId = profile.UserId,
                    UserName = profile.UserName,
                    ProfilePictureURL = profile.ProfilePictureURL
                }).Take(10).ToListAsync();
            return result;
        }

        private async Task AddTempUser()
        {
            Logger.Info("Creating temp users");
            var list = new List<UserProfile>()
            {
                new UserProfile
                {
                    UserName = "tommy.goossens",
                    Email = "tommygoossens@ziggo.nl",
                    UserId = "UBh7cektzYhSu6s4s6IdEEsNfz61"
                },
                new UserProfile
                {
                    UserName = "tommy.goossens1",
                    Email = "tommygoossens@ziggo.nl",
                    UserId = "UBh7cektzYhSu6s4s6IdEEsNfz62"
                },
                new UserProfile
                {
                    UserName = "tommy.goossens2",
                    Email = "tommygoossens@ziggo.nl",
                    UserId = "UBh7cektzYhSu6s4s6IdEEsNfz62"
                },
                new UserProfile
                {
                    UserName = "tommy.goossens3",
                    Email = "tommygoossens@ziggo.nl",
                    UserId = "UBh7cektzYhSu6s4s6IdEEsNfz63"
                },
                new UserProfile
                {
                    UserName = "tommy.goossens4",
                    Email = "tommygoossens@ziggo.nl",
                    UserId = "UBh7cektzYhSu6s4s6IdEEsNfz64"
                },
                new UserProfile
                {
                    UserName = "tommy.goossens5",
                    Email = "tommygoossens@ziggo.nl",
                    UserId = "UBh7cektzYhSu6s4s6IdEEsNfz65"
                },
                new UserProfile
                {
                    UserName = "jan.goossens",
                    Email = "tommygoossens@ziggo.nl",
                    UserId = "UBh7cektzYhSu6s4s6IdEEsNfz66"
                },
                new UserProfile
                {
                    UserName = "tommy.goossens",
                    Email = "tommygoossens@ziggo.nl",
                    UserId = "UBh7cektzYhSu6s4s6IdEEsNfz61"
                },
                new UserProfile
                {
                    UserName = "tommy.goossens1",
                    Email = "tommygoossens@ziggo.nl",
                    UserId = "UBh7cektzYhSu6s4s6IdEEsNfz62"
                },
                new UserProfile
                {
                    UserName = "tommy.goossens2",
                    Email = "tommygoossens@ziggo.nl",
                    UserId = "UBh7cektzYhSu6s4s6IdEEsNfz62"
                },
                new UserProfile
                {
                    UserName = "tommy.goossens3",
                    Email = "tommygoossens@ziggo.nl",
                    UserId = "UBh7cektzYhSu6s4s6IdEEsNfz63"
                },
                new UserProfile
                {
                    UserName = "tommy.goossens4",
                    Email = "tommygoossens@ziggo.nl",
                    UserId = "UBh7cektzYhSu6s4s6IdEEsNfz64"
                },
                new UserProfile
                {
                    UserName = "tommy.goossens5",
                    Email = "tommygoossens@ziggo.nl",
                    UserId = "UBh7cektzYhSu6s4s6IdEEsNfz65"
                },
                new UserProfile
                {
                    UserName = "jan.goossens",
                    Email = "tommygoossens@ziggo.nl",
                    UserId = "UBh7cektzYhSu6s4s6IdEEsNfz66"
                },
                new UserProfile
                {
                    UserName = "tommy.goossens",
                    Email = "tommygoossens@ziggo.nl",
                    UserId = "UBh7cektzYhSu6s4s6IdEEsNfz61"
                },
                new UserProfile
                {
                    UserName = "tommy.goossens1",
                    Email = "tommygoossens@ziggo.nl",
                    UserId = "UBh7cektzYhSu6s4s6IdEEsNfz62"
                },
                new UserProfile
                {
                    UserName = "tommy.goossens2",
                    Email = "tommygoossens@ziggo.nl",
                    UserId = "UBh7cektzYhSu6s4s6IdEEsNfz62"
                },
                new UserProfile
                {
                    UserName = "tommy.goossens3",
                    Email = "tommygoossens@ziggo.nl",
                    UserId = "UBh7cektzYhSu6s4s6IdEEsNfz63"
                },
                new UserProfile
                {
                    UserName = "tommy.goossens4",
                    Email = "tommygoossens@ziggo.nl",
                    UserId = "UBh7cektzYhSu6s4s6IdEEsNfz64"
                },
                new UserProfile
                {
                    UserName = "tommy.goossens5",
                    Email = "tommygoossens@ziggo.nl",
                    UserId = "UBh7cektzYhSu6s4s6IdEEsNfz65"
                },
                new UserProfile
                {
                    UserName = "jan.goossens",
                    Email = "tommygoossens@ziggo.nl",
                    UserId = "UBh7cektzYhSu6s4s6IdEEsNfz66"
                },new UserProfile
                {
                    UserName = "tommy.goossens",
                    Email = "tommygoossens@ziggo.nl",
                    UserId = "UBh7cektzYhSu6s4s6IdEEsNfz61"
                },
                new UserProfile
                {
                    UserName = "tommy.goossens1",
                    Email = "tommygoossens@ziggo.nl",
                    UserId = "UBh7cektzYhSu6s4s6IdEEsNfz62"
                },
                new UserProfile
                {
                    UserName = "tommy.goossens2",
                    Email = "tommygoossens@ziggo.nl",
                    UserId = "UBh7cektzYhSu6s4s6IdEEsNfz62"
                },
                new UserProfile
                {
                    UserName = "tommy.goossens3",
                    Email = "tommygoossens@ziggo.nl",
                    UserId = "UBh7cektzYhSu6s4s6IdEEsNfz63"
                },
                new UserProfile
                {
                    UserName = "tommy.goossens4",
                    Email = "tommygoossens@ziggo.nl",
                    UserId = "UBh7cektzYhSu6s4s6IdEEsNfz64"
                },
                new UserProfile
                {
                    UserName = "tommy.goossens5",
                    Email = "tommygoossens@ziggo.nl",
                    UserId = "UBh7cektzYhSu6s4s6IdEEsNfz65"
                },
                new UserProfile
                {
                    UserName = "jan.goossens",
                    Email = "tommygoossens@ziggo.nl",
                    UserId = "UBh7cektzYhSu6s4s6IdEEsNfz66"
                }
            };
            await _repo.Profiles.AddRangeAsync(list);
            await _repo.SaveChangesAsync();
            _dataIsCreated = true;
        }
    }
}