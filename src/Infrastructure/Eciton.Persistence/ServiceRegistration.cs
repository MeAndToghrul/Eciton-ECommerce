using Eciton.Application.Abstractions;
using Eciton.Domain.Settings;
using Eciton.Persistence.Contexts;
using Eciton.Persistence.Implements;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Eciton.Persistence;
public static class ServiceRegistration
{
    public static IServiceCollection AddBlServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddServices(configuration);
        return services;
    }
    public static void AddFluentValidation(this IServiceCollection services)
    {

    }
    private static void AddServices(this IServiceCollection services, IConfiguration configuration)
    {
        
        
    }
    public static IServiceCollection AddPostgreSql(this IServiceCollection services, string connStr)
    {
        services.AddNpgsql<AppDbContext>(connStr);
        return services;
    }
}
