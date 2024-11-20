using FiapTechChallenge.AppService.Interfaces;
using FiapTechChallenge.AppService.Services;
using FiapTechChallenge.Infra.Data;
using FiapTechChallenge.Infra.DbInitializer;
using FiapTechChallenge.Infra.Interfaces;
using FiapTechChallenge.Infra.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.OpenApi.Models;
using System.Reflection;
using Prometheus;

var builder = WebApplication.CreateBuilder(args);

//var configuration = new ConfigurationBuilder()
//    .AddJsonFile("appsettings.json")
//    .Build();

var configuration = builder.Configuration;

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
},  ServiceLifetime.Scoped);

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IDbInitializer, DbInitializer>();

builder.Services.AddScoped<IPersonService, PersonService>();
builder.Services.AddScoped<ISuporteDataService, SuporteDataService>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, $"{Assembly.GetExecutingAssembly().GetName().Name}.xml"));
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Fiap TechChallenge", Version = "v1" });
});

var app = builder.Build(); 

app.UseHttpMetrics();
app.MapMetrics();

 
app.UseSwagger();
app.UseSwaggerUI();


app.UseHttpsRedirection();

SeedDatabase();

app.UseAuthorization();

app.MapControllers();

app.Run();


void SeedDatabase()
{
    using (var scope = app.Services.CreateScope())
    {
        var dbInitializer = scope.ServiceProvider.GetRequiredService<IDbInitializer>();
        dbInitializer.Initialize();
    }
}
