namespace Eciton.Application.Events;
public class UserEmailConfirmedEvent 
{
    public string UserId { get; set; }

    public UserEmailConfirmedEvent(string userId)
    {
        UserId = userId;
    }
}
