using AspNetCoreRateLimit;
using Eciton.Application.Abstractions;
using Eciton.Infrastructure;
using Eciton.Infrastructure.Context;
using Eciton.Infrastructure.EventBus;
using Eciton.Persistence;
using Eciton.Persistence.Contexts;
using Eciton.Persistence.SeedDatas;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddScoped<ValidationFilter>();
builder.Services.AddControllers(options =>
{
    options.Filters.Add<ValidationFilter>();
});
builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.SuppressModelStateInvalidFilter = true;
});
builder.Services.AddBlServices(builder.Configuration);
builder.Services.AddMongoDb(builder.Configuration);
builder.Services.AddPostgreSql(builder.Configuration.GetConnectionString("PostgreSQL")!);
builder.Services.AddFluentValidation();
builder.Services.AddScoped<IEventBus, InMemoryEventBus>();

//SignalR
builder.Services.AddSignalR();

//Rate Limiting
builder.Services.Configure<IpRateLimitOptions>(builder.Configuration.GetSection("IpRateLimiting"));
builder.Services.AddInMemoryRateLimiting();
builder.Services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();





builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
        options.JsonSerializerOptions.WriteIndented = true;
        options.JsonSerializerOptions.Converters.Add(new System.Text.Json.Serialization.JsonStringEnumConverter());
    });

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "E-Commerce API",
        Version = "v1",
        Description = "API for managing product categories in the shopping platform: create, update, delete, and list categories."
    });
    c.EnableAnnotations();
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "bearer"
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement{
        {
            new OpenApiSecurityScheme{
                Reference = new OpenApiReference{
                    Id = "Bearer",
                    Type = ReferenceType.SecurityScheme
                }
            }, new List<string>()
        }
    });
});



var app = builder.Build();

using var scope = app.Services.CreateScope();
var services = scope.ServiceProvider;

// SQL Seed
var sqlContext = services.GetRequiredService<AppDbContext>();
var eventBus = services.GetRequiredService<IEventBus>();
await SeedData.SeedRolesAndAdminAsync(sqlContext,eventBus);


app.UseStaticFiles();
app.UseIpRateLimiting();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "E-Commerce API v1");
        c.InjectStylesheet("/swagger/SwaggerDark.css");
    });
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
