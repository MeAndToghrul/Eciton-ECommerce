namespace Eciton.Application.DTOs.Auth;
public class UserGetDTO
{
    public string Id { get; set; }
    public DateTime? LockoutEnd { get; set; }
    public int AccessFailedCount { get; set; } = 0;

}
