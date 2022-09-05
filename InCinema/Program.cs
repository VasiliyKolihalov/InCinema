using InCinema.Extensions;
using InCinema.Middlewares;
using InCinema.Models;
using InCinema.Repositories;
using InCinema.Services;

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

builder.Services.AddJwtAuthentication(jwtAuthConfiguration.Get<JwtAuthOptions>());

builder.Services.AddAutoMapper(typeof(Program));

builder.Services
    .AddTransient<MoviesService>()
    .AddTransient<GenresService>()
    .AddTransient<MoviePersonsService>()
    .AddTransient<CareersService>()
    .AddTransient<AccountService>()
    .AddTransient<IJwtService, JwtService>()
    .AddTransient<UsersService>()
    .AddTransient<RolesService>();

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