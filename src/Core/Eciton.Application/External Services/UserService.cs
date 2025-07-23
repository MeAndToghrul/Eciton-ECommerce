using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.JsonWebTokens;
using System.Security.Claims;
public class UserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    public UserService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public string? GetCurrentUserId()
    {
        return _httpContextAccessor.HttpContext?.User?.Claims
            .FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
    }
    public string? GetCurrentTokenId()
    {
        return _httpContextAccessor.HttpContext?.User?.Claims
            .FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Jti)?.Value;
    }
}
