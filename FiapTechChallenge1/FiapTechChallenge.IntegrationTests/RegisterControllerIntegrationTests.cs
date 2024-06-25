using FiapTechChallenge.API.Controllers;
using FiapTechChallenge.AppService.Interfaces;
using FiapTechChallenge.Domain.DTOs.ResponsesDto;
using FiapTechChallenge.Infra.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using FiapTechChallenge.Infra.Data;
using Microsoft.EntityFrameworkCore;
using FiapTechChallenge.AppService.Services;
using FiapTechChallenge.Infra.DbInitializer;
using FiapTechChallenge.Infra.Repositories;

namespace FiapTechChallenge.IntegrationTests
{
    public class RegisterControllerIntegrationTests
    {
        protected readonly ServiceProvider ServiceProvider;

        public RegisterControllerIntegrationTests()
        {
            var serviceCollection = new ServiceCollection();

            // Configuração do DbContext para usar o SQL Server no Docker
            serviceCollection.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer("Data Source=localhost,1444;Database=TechChallenge;User Id=sa;Password=SqlServer2019!;TrustServerCertificate=True;"));

            // Registrar o DbInitializer
            serviceCollection.AddScoped<IUnitOfWork, UnitOfWork>();
            serviceCollection.AddScoped<IDbInitializer, DbInitializer>();
            serviceCollection.AddScoped<IPersonService, PersonService>();
            serviceCollection.AddScoped<ISuporteDataService, SuporteDataService>();

            ServiceProvider = serviceCollection.BuildServiceProvider();
        }

        [Fact]
        public async Task GetAllContacts_ReturnsOkResponse_WithListOfContacts()
        {
            using (var scope = ServiceProvider.CreateScope())
            {
                var unitOfWorkService = scope.ServiceProvider.GetService<IUnitOfWork>();
                var personService = scope.ServiceProvider.GetService<IPersonService>();

                RegisterController registerController = new RegisterController(unitOfWorkService, personService);

                var result = await registerController.GetAllContacts();

                var okResult = Assert.IsType<OkObjectResult>(result);
                var returnValue = Assert.IsType<List<PersonResponseDto>>(okResult.Value);
                Assert.Equal(1, returnValue.Count);
            }
        }
    }
}
