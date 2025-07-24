using Eciton.Application.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
namespace Eciton.Infrastructure.BackGroundServices;
public class LockoutCleanupTimer : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;
    public LockoutCleanupTimer(IServiceScopeFactory scopeFactory)
    {
        _scopeFactory = scopeFactory;
    }
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
       while (!stoppingToken.IsCancellationRequested)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var userManager = scope.ServiceProvider.GetRequiredService<IAuthService>();
                await userManager.RefreshLockoutEndAsync();
                await Task.Delay(TimeSpan.FromSeconds(10), stoppingToken);
            }
        }
    }
}
