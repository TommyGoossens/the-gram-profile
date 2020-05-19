using MediatR;
using TheGramProfile.Domain.DTO.Request;
using TheGramProfile.Domain.DTO.Response;
using TheGramProfile.Domain.Models.DTO;

namespace TheGramProfile.Domain.Commands.CreateProfile
{
    public class CreateProfileCommand : IRequest<ProfileCreatedResponse>
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        public CreateProfileCommand(CreateProfileRequest request)
        {
            Username = request.Username;
            Email = request.Email;
            Password = request.Password;
        }
    }
}