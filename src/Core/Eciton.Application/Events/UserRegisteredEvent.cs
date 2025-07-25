namespace Eciton.Application.Events;
public class UserRegisteredEvent
{
    public string Id { get; set; } = null!;
    public string FullName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string RoleName { get; set; } = null!;
    public bool IsEmailConfirmed { get; set; }

    public UserRegisteredEvent(string id, string fullName, string email,string roleName, bool isEmailConfirm)
    {
        Id = id;
        FullName = fullName;
        Email = email;
        RoleName = roleName;
        IsEmailConfirmed = isEmailConfirm;
    }
}
