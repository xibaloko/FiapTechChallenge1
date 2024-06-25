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
using FiapTechChallenge.Domain.DTOs.RequestsDto;
using FiapTechChallenge.Tests.Helpers;
using FiapTechChallenge.Domain.Entities;
using Microsoft.AspNetCore.Mvc.Infrastructure;

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
            // Inicializar a base de dados
            InitializeDatabase();
        }

        [Fact]
        public async Task CreateContact_ReturnsCreatedAtActionResponse()
        {
            using (var scope = ServiceProvider.CreateScope())
            {
                var unitOfWorkService = scope.ServiceProvider.GetService<IUnitOfWork>();
                var personService = scope.ServiceProvider.GetService<IPersonService>();

                RegisterController registerController = new RegisterController(unitOfWorkService, personService);

                var personDto = PersonRequestDtoFaker.GeneratePersonRequest();
                foreach (var phone in personDto.Phones)
                {
                    phone.DDDNumber = 11;
                }
                var result = await registerController.CreateContact(personDto);

                var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result);
                Assert.Equal(nameof(RegisterController.GetContactById), createdAtActionResult.ActionName);
                Assert.NotNull(createdAtActionResult.RouteValues["id"]);
            }
        }

        [Fact]
        public async Task GetAllContacts_ReturnsOkResponse_WithListOfContacts()
        {
            using (var scope = ServiceProvider.CreateScope())
            {
                Person person = await CreatePersonTest(scope);
                var unitOfWorkService = scope.ServiceProvider.GetService<IUnitOfWork>();
                var personService = scope.ServiceProvider.GetService<IPersonService>();
                RegisterController registerController = new RegisterController(unitOfWorkService, personService);
                var result = await registerController.GetAllContacts();
                var okResult = Assert.IsType<OkObjectResult>(result);
                var returnValue = Assert.IsType<List<PersonResponseDto>>(okResult.Value);
                //Assert.Single(returnValue);
                Assert.True(returnValue.Count >= 1);
            }
        }

        [Fact]
        public async Task GetContactById_ReturnsOkResponse_WithContact()
        {
            using (var scope = ServiceProvider.CreateScope())
            {
                var unitOfWorkService = scope.ServiceProvider.GetService<IUnitOfWork>();
                var personService = scope.ServiceProvider.GetService<IPersonService>();
                RegisterController registerController = new RegisterController(unitOfWorkService, personService);
                Person person = await CreatePersonTest(scope);
                var result = await registerController.GetContactById(person.Id);
                var okResult = Assert.IsType<OkObjectResult>(result);
                var returnValue = Assert.IsType<PersonResponseDto>(okResult.Value);
                Assert.NotNull(returnValue);
            }
        }

        [Fact]
        public async Task GetContactsByRegion_ReturnsOkResponse_WithListOfContacts()
        {
            using (var scope = ServiceProvider.CreateScope())
            {
                var unitOfWorkService = scope.ServiceProvider.GetService<IUnitOfWork>();
                var personService = scope.ServiceProvider.GetService<IPersonService>();

                RegisterController registerController = new RegisterController(unitOfWorkService, personService);

                var result = await registerController.GetContactsByRegion(1);

                var okResult = Assert.IsType<OkObjectResult>(result);
                var returnValue = Assert.IsType<List<PersonResponseDto>>(okResult.Value);
                Assert.NotNull(returnValue);
            }
        }

        [Fact]
        public async Task GetContactsByDDD_ReturnsOkResponse_WithListOfContacts()
        {
            using (var scope = ServiceProvider.CreateScope())
            {
                var unitOfWorkService = scope.ServiceProvider.GetService<IUnitOfWork>();
                var personService = scope.ServiceProvider.GetService<IPersonService>();
                RegisterController registerController = new RegisterController(unitOfWorkService, personService);
                var result = await registerController.GetContactsByDDD(11);
                var okResult = Assert.IsType<OkObjectResult>(result);
                var returnValue = Assert.IsType<List<PersonResponseDto>>(okResult.Value);
                Assert.NotNull(returnValue);
            }
        }

        [Fact]
        public async Task UpdateContact_ReturnsOkResponse_WithModifiedContact()
        {
            using (var scope = ServiceProvider.CreateScope())
            {
                var unitOfWorkService = scope.ServiceProvider.GetService<IUnitOfWork>();
                var personService = scope.ServiceProvider.GetService<IPersonService>();
                Person person = await CreatePersonTest(scope);
                RegisterController registerController = new RegisterController(unitOfWorkService, personService);
                var personDto = PersonRequestDtoFaker.GeneratePersonRequest();
                foreach (var phone in personDto.Phones)
                {
                    phone.DDDNumber = 11;
                }
                var result = await registerController.UpdateContact(1, personDto);
                var okResult = Assert.IsType<OkObjectResult>(result);
                var returnValue = Assert.IsType<PersonResponseDto>(okResult.Value);
                Assert.NotNull(returnValue);
            }
        }

        [Fact]
        public async Task DeleteContact_ReturnsOkResponse_WithSuccessfulMessage()
        {
            using (var scope = ServiceProvider.CreateScope())
            {
                Person person = await CreatePersonTest(scope);
                var unitOfWorkService = scope.ServiceProvider.GetService<IUnitOfWork>();
                var personService = scope.ServiceProvider.GetService<IPersonService>();
                RegisterController registerController = new RegisterController(unitOfWorkService, personService);
                var result = await registerController.DeleteContact(person.Id);
                var okResult = Assert.IsType<OkObjectResult>(result);
                var returnValue = okResult.StatusCode;
                Assert.Equal(200, returnValue);
            }
        }

        private static async Task<Person> CreatePersonTest(IServiceScope scope)
        {
            var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            var person = new Person()
            {
                Birthday = DateTime.Now,
                CPF = "12345678901",
                Email = "Teste@teste.com.be",
                Name = "João da Silva",
                Created = DateTime.Now,
                Modified = DateTime.Now,
                Phones = new List<Phone>()
                    {
                        new Phone()
                        {
                            DDDId = 11,
                            PhoneNumber = "999999999",
                            PhoneTypeId = 1,
                        }
                    }
            };
            await context.AddAsync(person);
            context.SaveChanges();
            return person;
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
    }
}
