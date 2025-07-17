using Eciton.Application.Abstractions;
using Eciton.Application.Handlers.Auth;
using Eciton.Application.Helpers;
using Eciton.Application.MapperProfiles;
using Eciton.Application.Options;
using Eciton.Application.Validators.Auth;
using Eciton.Domain.Settings;
using Eciton.Persistence.Contexts;
using Eciton.Persistence.Implements;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Text;
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
        services.AddFluentValidationAutoValidation();
        services.AddValidatorsFromAssemblyContaining(typeof(RegisterDtoValidator));
        return services;
    }

    public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<ITokenService, TokenService>();
        services.AddScoped<PasswordService>();
        services.AddScoped<IEmailService, EmailService>();

        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(typeof(LoginUserCommandHandler).Assembly);
        });

        services.Configure<JwtSettings>(configuration.GetSection("JwtSettings"));

        var jwtSettings = configuration.GetSection("JwtSettings").Get<JwtSettings>();

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.SecretKey)),
                ValidateIssuer = true,
                ValidIssuer = jwtSettings.Issuer,
                ValidateAudience = true,
                ValidAudience = jwtSettings.Audience,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            };
        });
        services.Configure<SmtpOptions>(configuration.GetSection(SmtpOptions.Position));


        


        return services;
    }

    public static IServiceCollection AddPostgreSql(this IServiceCollection services, string connStr)
    {
        services.AddNpgsql<AppDbContext>(connStr);
        return services;
    }
}

