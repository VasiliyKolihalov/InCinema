using InCinema.Extensions;
using InCinema.Middlewares;
using InCinema.Models;
using InCinema.Models.Configurations;
using InCinema.Repositories;
using InCinema.Services;

var builder = WebApplication.CreateBuilder(args);

#region Configuration

builder.Configuration.AddJsonFile("jwtauthoptions.json");
builder.Configuration.AddJsonFile("companydata.json");
builder.Configuration.AddJsonFile("emailoptions.json");

#endregion

#region Services

builder.Services.AddControllers()
    .AddNewtonsoftJson();

string connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddTransient<IApplicationContext>(_ => new ApplicationContext(connectionString));

IConfigurationSection jwtAuthOptions = builder.Configuration.GetSection("JwtAuthOptions");
builder.Services.Configure<JwtAuthOptions>(jwtAuthOptions);

builder.Services.AddJwtAuthentication(jwtAuthOptions.Get<JwtAuthOptions>());

IConfigurationSection companyData = builder.Configuration.GetSection("CompanyData");
builder.Services.Configure<CompanyData>(companyData);

IConfigurationSection emailOptions = builder.Configuration.GetSection("EmailOptions");
builder.Services.Configure<EmailOptions>(emailOptions);

builder.Services.AddAutoMapper(typeof(Program));

builder.Services
    .AddTransient<MoviesService>()
    .AddTransient<GenresService>()
    .AddTransient<MoviePersonsService>()
    .AddTransient<CareersService>()
    .AddTransient<AccountService>()
    .AddTransient<IJwtService, JwtService>()
    .AddTransient<UsersService>()
    .AddTransient<RolesService>()
    .AddTransient<IConfirmCodeGenerator, ConfirmCodeGenerator>()
    .AddTransient<IEmailService, EmailService>();

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