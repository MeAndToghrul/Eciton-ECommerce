using Eciton.Application.Abstractions;
using Eciton.Application.Abstractions.Repositories;
using Eciton.Application.Events;
using Eciton.Domain.Settings;
using Eciton.Infrastructure.BackGroundServices;
using Eciton.Infrastructure.Context;
using Eciton.Infrastructure.EventHandlers;
using Eciton.Infrastructure.Repositories.Read;
using Eciton.Infrastructure.Services;
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
        services.AddScoped<IEventHandler<UserEmailConfirmedEvent>, UserEmailConfirmedEventHandler>();
        services.AddScoped<IEventHandler<CategoryCreatedEvent>, CategoryCreatedEventHandler>();
        services.AddScoped<IEventHandler<CategoryFieldCreatedEvent>, CategoryFieldCreatedEventHandler>();


        services.AddHostedService<LockoutCleanupTimer>();   

        return services;
    }

    public static void AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<IStorageService, CloudinaryStorageService>();
        services.AddSingleton<ICategoryReadRepository,CategoryReadRepository>();
        services.AddSingleton<ICategoryFieldReadRepository,CategoryFieldReadRepository>();
    }
}
