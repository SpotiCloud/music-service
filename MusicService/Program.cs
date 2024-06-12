using MusicService.Data;
using MusicService.Services.Song;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi.Models;
using MusicService.Services.Event;
using Microsoft.EntityFrameworkCore;
using Prometheus;

var builder = WebApplication.CreateBuilder(args);

// Ensure environment variables are added early
builder.Configuration.AddEnvironmentVariables();

if (builder.Environment.IsDevelopment())
{
    builder.Configuration.AddUserSecrets<Program>();
}

var services = builder.Services;

// Add authentication services
services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(o =>
{

    //authority runs behind reverse proxy
    o.RequireHttpsMetadata = false;

    o.Authority = builder.Configuration["Jwt:Authority"];
    o.Audience = builder.Configuration["Jwt:Audience"];


    o.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateAudience = false
    };

});

// Configure DbContext with environment variables
services.AddDbContext<DataContext>(options =>
    options.UseNpgsql(
        $"Host={builder.Configuration["POSTGRES_HOST_NAME"]};Port=5432;Database=music;Username={builder.Configuration["POSTGRES_USERNAME"]};Password={builder.Configuration["POSTGRES_PASSWORD"]}"
    )
);

services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(setup =>
{
    setup.SwaggerDoc("v1", new OpenApiInfo { Title = "Music API v1.0", Version = "v1" });
    setup.AddSecurityDefinition("OAuth2", new OpenApiSecurityScheme
    {
        Type = SecuritySchemeType.OAuth2,
        Flows = new OpenApiOAuthFlows
        {
            Implicit = new OpenApiOAuthFlow
            {
                AuthorizationUrl = new Uri("http://localhost:8180/realms/SpotiCloud/protocol/openid-connect/auth"),
            }
        }
    });
    setup.AddSecurityRequirement(new OpenApiSecurityRequirement{
    {
        new OpenApiSecurityScheme{
            Reference = new OpenApiReference{
                Type = ReferenceType.SecurityScheme,
                Id = "OAuth2" //The name of the previously defined security scheme.
            }
        },
        new string[] {}
    }
    });
});

services.AddAutoMapper(typeof(Program).Assembly);
services.AddScoped<ISongService, SongService>();
services.AddSingleton<IEventService, EventService>();

var app = builder.Build();

app.UseCors(builder => builder
    .AllowAnyOrigin()
    .AllowAnyMethod()
    .AllowAnyHeader());

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.UseMetricServer();

app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<DataContext>();
    dbContext.Database.Migrate();
}

app.Run();