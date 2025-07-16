using Eciton.Application.Abstractions;
using Eciton.Application.Helpers;
using Eciton.Application.MapperProfiles;
using Eciton.Application.Options;
using Eciton.Persistence.Contexts;
using Eciton.Persistence.Implements;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
namespace Eciton.Persistence;
public static class ServiceRegistration
{
    public static IServiceCollection AddBlServices(this IServiceCollection services, IConfiguration configuration)
    {
        return services.AddServices(configuration)
                       .AddAutoMapper(typeof(AuthMappingProfile).Assembly);
        
    }

    public static IServiceCollection AddFluentValidation(this IServiceCollection services)
    {        
        return services;
    }

    public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<ITokenService, TokenService>();
        services.AddScoped<PasswordService>();
        services.AddScoped<IEmailService, EmailService>();




        services.Configure<SmtpOptions>(configuration.GetSection(SmtpOptions.Position));
        return services;
    }

    public static IServiceCollection AddPostgreSql(this IServiceCollection services, string connStr)
    {
        services.AddNpgsql<AppDbContext>(connStr);
        return services;
    }
}

