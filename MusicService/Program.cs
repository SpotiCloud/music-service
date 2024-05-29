using MusicService.Data;
using MusicService.Services.Song;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi.Models;
using MusicService.Services.Event;

var builder = WebApplication.CreateBuilder(args);

var services = builder.Services;

var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
                      policy =>
                      {
                          policy.WithOrigins("*",
                                              "*");
                      });
});

// Add authentication services
services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(o =>
{

    //authority runs behind reverse proxy
    o.RequireHttpsMetadata = false;

    o.Authority = builder.Configuration["Jwt:Authority"];
    o.Audience = builder.Configuration["Jwt:Audience"];


    o.TokenValidationParameters = new TokenValidationParameters
    {
        //asp.net core seems to expect an audience with the name of the client, this disables the check
        //This is primarily for architectures where certain services aren't trusted.
        //The person sending the token can specify what service the token is meant for.
        //This prevents "bad" services from using the token to send requests to other services.
        //Since this architecture doesn't have untrusted services, this is not necessary.
        ValidateAudience = false
    };

});

/*.AddOpenIdConnect(options =>
{
    // Configure the authority to your Keycloak realm URL
    options.Authority = "http://localhost:8080/realms/SpotiCloud";

    options.RequireHttpsMetadata = false;
    // Configure the client ID and client secret
    options.ClientId = "test";
    options.ClientSecret = "DRW0BGdceP1mMdO7tYo0NIycGYyPVifx";

    // Set the callback path
    options.CallbackPath = "/swagger/index.html";

    // Configure the response type
    options.ResponseType = OpenIdConnectResponseType.Code;

    // Configure the scope
    options.Scope.Clear();
    options.Scope.Add("openid");
    //options.Scope.Add("profile");
    //options.Scope.Add("email");

    // Configure the token validation parameters
    options.TokenValidationParameters = new TokenValidationParameters
    {
        NameClaimType = "name",
        RoleClaimType = "role"
    };
});*/

services.AddDbContext<DataContext>();
// Add services to the container.
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
                AuthorizationUrl = new Uri("http://localhost:8080/realms/SpotiCloud/protocol/openid-connect/auth"),
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

    /*var jwtSecurityScheme = new OpenApiSecurityScheme
    {
        Scheme = "bearer",
        BearerFormat = "JWT",
        Name = "Authentication",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,

        Reference = new OpenApiReference
        {
            Id = JwtBearerDefaults.AuthenticationScheme,
            Type = ReferenceType.SecurityScheme
        }
    };

    setup.AddSecurityDefinition(jwtSecurityScheme.Reference.Id, jwtSecurityScheme);

    setup.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        { jwtSecurityScheme, Array.Empty<string>() }
    });*/
});

services.AddAutoMapper(typeof(Program).Assembly);

services.AddScoped<ISongService, SongService>();
services.AddSingleton<IEventService, EventService>();

var app = builder.Build();

app.UseCors(builder => builder
    .AllowAnyOrigin()
    .AllowAnyMethod()
    .AllowAnyHeader());

//app.UseCors(MyAllowSpecificOrigins);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

/*app.UseCookiePolicy(new CookiePolicyOptions{
    MinimumSameSitePolicy = SameSiteMode.Lax
});*/

app.MapControllers();

app.Run();
