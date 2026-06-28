using Newtonsoft.Json;
using System.Text;
using ClinicManagementSystem.Models;

namespace ClinicManagementSystem.Services
{
    public class AuthService
    {
        private readonly HttpClient _httpClient;

        public AuthService(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("CMSApi");
        }

        public async Task<string?> AuthenticateUserAsync(string email, string password)
        {
            var requestData = new
            {
                email = email,
                password = password
            };

            var content = new StringContent(JsonConvert.SerializeObject(requestData), Encoding.UTF8, "application/json");

            HttpResponseMessage response = await _httpClient.PostAsync("auth/login", content);

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsStringAsync();
            }

            return null;
        }
        public async Task<string?> RegisterUserAsync(RegisterRequest request)
        {
            var content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");
            HttpResponseMessage response = await _httpClient.PostAsync("auth/register", content);

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsStringAsync();
            }

            return null;
        }
    }
}
