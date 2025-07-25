using Microsoft.AspNetCore.SignalR;
namespace Eciton.Application.Hubs;
public class AuthHub : Hub
{
    public async Task UpdateUserStatus(string userId, bool status)
    {
        await Clients.All.SendAsync("UserStatusUpdated", userId, status);
    }
}
