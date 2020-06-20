using MediatR;
using TheGramProfile.Domain.DTO.Response;

namespace TheGramProfile.Domain.Commands.CreateUserProfileDb
{
    public class CreateUserProfileFromFirebaseIdCommand : IRequest<ProfileCreatedResponse>
    {
        public string FirebaseId { get; }
        public string UserName { get; }

        public CreateUserProfileFromFirebaseIdCommand(string firebaseId, string userName)
        {
            FirebaseId = firebaseId;
            UserName = userName;
        }
    }
}