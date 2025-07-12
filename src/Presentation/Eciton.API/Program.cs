using Eciton.Persistence;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
        options.JsonSerializerOptions.WriteIndented = true;
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
});

builder.Services.AddPostgreSql(builder.Configuration.GetConnectionString("PostgreSQL")!);

var app = builder.Build();

app.UseStaticFiles();

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
app.UseAuthorization();
app.MapControllers();
app.Run();