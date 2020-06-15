using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using TheGramProfile.Domain.DTO.Request;
using TheGramProfile.Domain.DTO.Response;
using TheGramProfile.Properties;
using TheGramProfile.Services;

namespace TheGramProfile.Domain.Commands.CreateProfile
{
    public class CreateProfileHandler : IRequestHandler<CreateProfileCommand, ProfileCreatedResponse>
    {
        private readonly IProfileService _profileService;
        private readonly HttpClient _httpClient = new HttpClient();
        private readonly string _endpoint = $"{Constants.FirebaseSignUpUrl}{Constants.FirebaseApiKey}";
        
        public CreateProfileHandler(IProfileService profileService)
        {
            _profileService = profileService;
        }
        public async Task<ProfileCreatedResponse> Handle(CreateProfileCommand request, CancellationToken cancellationToken)
        {
            var body = new {email = request.Email, password = request.Password, returnSecureToken = false};
            var response = await _httpClient.PostAsync(
                _endpoint,
                new StringContent(JsonConvert.SerializeObject(body)),
                cancellationToken);
            
            if (!response.IsSuccessStatusCode) return null;
            
            var json = JObject.Parse(await response.Content.ReadAsStringAsync());
            var id = (string)json["localId"];
            
            return await _profileService.CreateUser(id, new CreateProfileRequest
            {
                Email = request.Email,
                Username = request.Username
            });
        }
    }
}