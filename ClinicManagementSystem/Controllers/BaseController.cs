using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;

namespace ClinicManagementSystem.Controllers
{
    public class BaseController : Controller
    {
        protected readonly HttpClient _client;
        protected readonly IHttpContextAccessor _httpContextAccessor;

        public BaseController(IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor)
        {
            _client = httpClientFactory.CreateClient("CMSApi");
            _httpContextAccessor = httpContextAccessor;
        }

        protected void AddJwtToken()
        {
            var token = _httpContextAccessor.HttpContext?.Session.GetString("JWTToken");
            if (!string.IsNullOrEmpty(token))
            {
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
        }

        protected string? GetUserRole()
        {
            return _httpContextAccessor.HttpContext?.Session.GetString("UserRole");
        }
    }
}
