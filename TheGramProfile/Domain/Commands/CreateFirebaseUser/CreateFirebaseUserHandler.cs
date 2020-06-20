using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using TheGramProfile.Properties;

namespace TheGramProfile.Domain.Commands.CreateFirebaseUser
{
    public class CreateFirebaseUserHandler : IRequestHandler<CreateFirebaseUserCommand,string>
    {
        private readonly HttpClient _httpClient = new HttpClient();
        private readonly string _endpoint = $"{Constants.FirebaseSignUpUrl}{Constants.FirebaseApiKey}";
        
        public async Task<string> Handle(CreateFirebaseUserCommand request, CancellationToken cancellationToken)
        {
            var body = new {email = request.Email, password = request.Password, returnSecureToken = false};
            var response = await _httpClient.PostAsync(
                _endpoint,
                new StringContent(JsonConvert.SerializeObject(body)),
                cancellationToken);
            if (!response.IsSuccessStatusCode) return null;
            var json = JObject.Parse(await response.Content.ReadAsStringAsync());
            return (string)json["localId"];
        }
    }
}