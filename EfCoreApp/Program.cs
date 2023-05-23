using EfCoreApp.ConfigurationExtensions;
using EfCoreApp.Servicies;
using EfCoreApp.Servicies.Interfaces;

var builder = WebApplication.CreateBuilder(args);

IConfiguration _configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", true, false)
#if DEBUG
            .AddJsonFile("appsettings.Development.json", true, false)
#endif
            .AddEnvironmentVariables()
            .Build();

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IDataService, DataService>();
builder.Services.RegisterDbServices(_configuration["DatabaseConnectionString"]); ;

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.EnsureDbIsReady();

app.Run();
