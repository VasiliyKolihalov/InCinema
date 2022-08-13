using InCinema.Middlewares;
using InCinema.Repositories;
using InCinema.Services;

var builder = WebApplication.CreateBuilder(args);

#region Services

builder.Services.AddControllers()
    .AddNewtonsoftJson();

string connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddTransient<IApplicationContext>(_ => new ApplicationContext(connectionString));

builder.Services.AddAutoMapper(typeof(Program));

builder.Services.AddTransient<MoviesService>();

#endregion

var app = builder.Build();

#region Middlewares

app.UseMiddleware<ErrorHandlerMiddleware>();
app.UseHttpLogging();

#endregion

app.MapControllers();
app.Run();