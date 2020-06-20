using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TheGramProfile.Domain.DTO.Response;
using TheGramProfile.Domain.Models;
using TheGramProfile.Repository;

namespace TheGramProfile.Domain.Commands.CreateUserProfileDb
{
    public class CreateUserProfileFromFirebaseIdHandler :IRequestHandler<CreateUserProfileFromFirebaseIdCommand,ProfileCreatedResponse>
    {
        private readonly ProfileContext _profileContext;

        public CreateUserProfileFromFirebaseIdHandler(ProfileContext profileContext)
        {
            _profileContext = profileContext;
        }

        public async Task<ProfileCreatedResponse> Handle(CreateUserProfileFromFirebaseIdCommand request, CancellationToken cancellationToken)
        {
            var user = await _profileContext.Profiles.Where(p => p.UserId.Equals(request.FirebaseId)).FirstOrDefaultAsync(cancellationToken: cancellationToken);
            if (user != null) return null;

            user = new UserProfile
            {
                Username = request.UserName,
                UserId = request.FirebaseId
            };
            await _profileContext.AddAsync(user, cancellationToken);
            await _profileContext.SaveChangesAsync(cancellationToken);
            return new ProfileCreatedResponse(user.UserId);
        }
    }
}