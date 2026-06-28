using Newtonsoft.Json;

namespace ClinicManagementSystem.Models
{
    public class LoginRequest
    {
        [JsonProperty("email")]
        public string Email { get; set; } = string.Empty;
        [JsonProperty("password")]
        public string Password { get; set; } = string.Empty;
    }

    public class LoginResponse
    {
        [JsonProperty("token")]
        public string Token { get; set; } = string.Empty;
        [JsonProperty("user")]
        public UserInfo User { get; set; } = new();
    }

    public class RegisterRequest
    {
        [JsonProperty("name")]
        public string Name { get; set; } = string.Empty;
        [JsonProperty("email")]
        public string Email { get; set; } = string.Empty;
        [JsonProperty("password")]
        public string Password { get; set; } = string.Empty;
        [JsonProperty("clinicCode")]
        public string ClinicCode { get; set; } = string.Empty;
    }

    public class UserInfo
    {
        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; } = string.Empty;
        [JsonProperty("email")]
        public string Email { get; set; } = string.Empty;
        [JsonProperty("role")]
        public string Role { get; set; } = string.Empty;
        [JsonProperty("phone")]
        public string? Phone { get; set; }
        [JsonProperty("clinicId")]
        public int ClinicId { get; set; }
        [JsonProperty("clinicName")]
        public string ClinicName { get; set; } = string.Empty;
        [JsonProperty("clinicCode")]
        public string ClinicCode { get; set; } = string.Empty;
    }
}
