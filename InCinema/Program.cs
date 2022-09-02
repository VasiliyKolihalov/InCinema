using InCinema.Middlewares;
using InCinema.Models;
using InCinema.Repositories;
using InCinema.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

#region Configuration

builder.Configuration.AddJsonFile("jwtauthsettings.json");

#endregion

#region Services

builder.Services.AddControllers()
    .AddNewtonsoftJson();

string connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddTransient<IApplicationContext>(_ => new ApplicationContext(connectionString));

IConfigurationSection jwtAuthConfiguration = builder.Configuration.GetSection("JwtAuthData");
builder.Services.Configure<JwtAuthOptions>(jwtAuthConfiguration);

JwtAuthOptions jwtAuthOptions = new JwtAuthOptions
{
    Issuer = jwtAuthConfiguration["Issuer"],
    Audience = jwtAuthConfiguration["Audience"],
    Secret = jwtAuthConfiguration["Secret"],
    TokenMinuteLifetime = Convert.ToInt32(jwtAuthConfiguration["TokenMinuteLifetime"])
};

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = false;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = jwtAuthOptions.Issuer,

            ValidateAudience = true,
            ValidAudience = jwtAuthOptions.Audience,
            ValidateLifetime = true,

            IssuerSigningKey = jwtAuthOptions.GetSymmetricSecurityKey(),
            ValidateIssuerSigningKey = true
        };
    });

builder.Services.AddAutoMapper(typeof(Program));

builder.Services
    .AddTransient<MoviesService>()
    .AddTransient<GenresService>()
    .AddTransient<MoviePersonsService>()
    .AddTransient<CareersService>()
    .AddTransient<AccountService>()
    .AddTransient<UsersService>();

#endregion

var app = builder.Build();

#region Middlewares

app.UseHttpLogging()
    .UseAuthentication()
    .UseAuthorization()
    .UseMiddleware<ErrorHandlerMiddleware>();

#endregion

app.MapControllers();
app.Run();