using MediatR;

namespace TheGramProfile.Domain.Commands.CreateFirebaseUser
{
    public class CreateFirebaseUserCommand : IRequest<string>
    {
        public string Email { get; }
        public string Password { get; }

        public CreateFirebaseUserCommand(string email, string password)
        {
            Email = email;
            Password = password;
        }
    }
}