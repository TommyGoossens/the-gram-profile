using MediatR;
using TheGramProfile.Domain.DTO.Request;
using TheGramProfile.Domain.DTO.Response;

namespace TheGramProfile.Domain.Commands.CreateFirebaseUserAndProfile
{
    public class CreateFirebaseUserAndProfileCommand : IRequest<ProfileCreatedResponse>
    {
        public string Username { get; }
        public string Email { get; }
        public string Password { get; }

        public CreateFirebaseUserAndProfileCommand(CreateProfileRequest request)
        {
            Username = request.Username;
            Email = request.Email;
            Password = request.Password;
        }
    }
}