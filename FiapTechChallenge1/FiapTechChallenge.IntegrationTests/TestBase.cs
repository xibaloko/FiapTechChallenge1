using FiapTechChallenge.Infra.Data;
using FiapTechChallenge.Infra.DbInitializer;
using FiapTechChallenge.Infra.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace FiapTechChallenge.IntegrationTests
{
    public class TestBase : IDisposable
    {
        protected readonly ServiceProvider ServiceProvider;

        public TestBase()
        {
            var serviceCollection = new ServiceCollection();

            // Configuração do DbContext para usar o SQL Server no Docker
            serviceCollection.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer("Data Source=localhost,1433;Database=TechChallenge;User Id=sa;Password=SqlServer2019!;TrustServerCertificate=True;"));

            // Registrar o DbInitializer
            serviceCollection.AddScoped<IDbInitializer, DbInitializer>();

            // Registrar outros serviços necessários
            // serviceCollection.AddScoped<IYourService, YourServiceImplementation>();

            ServiceProvider = serviceCollection.BuildServiceProvider();

            // Inicializar a base de dados
            InitializeDatabase();
        }

        private void InitializeDatabase()
        {
            using (var scope = ServiceProvider.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                context.Database.EnsureDeleted(); // Opcional: Limpa o banco de dados antes de cada teste
                context.Database.EnsureCreated();

                var dbInitializer = scope.ServiceProvider.GetRequiredService<IDbInitializer>();
                dbInitializer.Initialize();
            }
        }

        public void Dispose()
        {
            // Limpar recursos quando os testes terminarem
            ServiceProvider?.Dispose();
        }
    }
}
