namespace Eciton.Application.Abstractions;
public interface IRateLimitService
{
    Task<bool> IsBlocked(string ip);
    Task RegisterFailedAttempt(string ip);
}
