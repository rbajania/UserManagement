using MongoDB.Entities;
using UserService;

namespace UserService.Services
{
    public class UserServiceHttpClient
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _config;

        public UserServiceHttpClient(HttpClient httpClient, IConfiguration config)
        {
            _httpClient = httpClient;
            _config = config;
        }

        public async Task<List<User>> GetUsersForSearchDb()
        {
            return await _httpClient.GetFromJsonAsync<List<User>>(_config["UserServiceUrl"]
                + "/api/users");
        }
    }
}