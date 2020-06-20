using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TheGramProfile.Domain.Commands.CreateFirebaseUser;
using TheGramProfile.Domain.Commands.CreateUserProfileDb;
using TheGramProfile.Domain.DTO.Response;
using TheGramProfile.Repository;

namespace TheGramProfile.Domain.Commands.CreateFirebaseUserAndProfile
{
    public class CreateFirebaseUserAndProfileHandler : IRequestHandler<CreateFirebaseUserAndProfileCommand, ProfileCreatedResponse>
    {
        private readonly IMediator _mediator;
        private readonly ProfileContext _profileContext;
        
        public CreateFirebaseUserAndProfileHandler(ProfileContext profileContext, IMediator mediator)
        {
            _profileContext = profileContext;
            _mediator = mediator;
        }
        public async Task<ProfileCreatedResponse> Handle(CreateFirebaseUserAndProfileCommand request, CancellationToken cancellationToken)
        {
            var user = _profileContext.Profiles.Where(u => u.Username.Equals(request.Username)).AsNoTracking()
                .FirstOrDefaultAsync(cancellationToken: cancellationToken);
            if (user != null)
            {
                throw new DuplicateNameException(request.Username);
                
            }
            var firebaseId = await _mediator.Send(new CreateFirebaseUserCommand(request.Email, request.Password), cancellationToken);
            return await _mediator.Send(new CreateUserProfileFromFirebaseIdCommand(firebaseId, request.Username), cancellationToken);
        }
    }
}