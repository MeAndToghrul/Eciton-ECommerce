using Eciton.Application.Abstractions;
using Eciton.Application.Events;
using Eciton.Domain.Settings;
using Eciton.Infrastructure.Context;
using Eciton.Infrastructure.EventHandlers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Eciton.Infrastructure;
public static class ServiceRegistration
{
    public static IServiceCollection AddMongoDb(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<MongoDbContext>(sp =>
        {
            var settings = sp.GetRequiredService<IOptions<MongoSettings>>().Value;
            return new MongoDbContext(settings);
        });
        services.Configure<MongoSettings>(configuration.GetSection("MongoSettings"));

        services.AddScoped<IEventHandler<UserRegisteredEvent>, UserRegisteredEventHandler>();
        services.AddScoped<IEventHandler<RoleCreatedEvent>, RoleEventHandler>();

        return services;
    }
}
