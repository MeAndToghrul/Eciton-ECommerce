using System.Text.Json.Serialization;

namespace Eciton.Application.DTOs.Auth;
public class LoginDTO
{
    public string Email { get; set; }
    public string Password { get; set; }
    public bool RememberMe { get; set; }

    [JsonIgnore]
    public string? UserIp { get; set; }
}
