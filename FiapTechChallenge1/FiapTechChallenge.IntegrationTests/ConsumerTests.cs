using FiapTechChallenge.API.Controllers;
using FiapTechChallenge.AppService.Interfaces;
using FiapTechChallenge.AppService.Services;
using FiapTechChallenge.Domain.DTOs.RequestsDto;
using FiapTechChallenge.Domain.DTOs.ResponsesDto;
using FiapTechChallenge.Domain.Entities;
using FiapTechChallenge.Infra.Data;
using FiapTechChallenge.Infra.DbInitializer;
using FiapTechChallenge.Infra.Interfaces;
using FiapTechChallenge.Infra.Repositories;
using FiapTechChallenge.Tests.Helpers;
using MassTransit;
using MassTransit.Testing;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FiapTechChallenge.IntegrationTests
{
    public class ConsumerTests
    {
        protected readonly ServiceProvider ServiceProvider;

        public ConsumerTests()
        {
            var serviceCollection = new ServiceCollection();

            // Configuração do DbContext para usar o SQL Server no Docker
            serviceCollection.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(
                    "Data Source=localhost,1444;Database=TechChallenge;User Id=sa;Password=SqlServer2019!;TrustServerCertificate=True;"));

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
                
                var personDto = PersonRequestDtoFaker.GeneratePersonRequest();
                foreach (var phone in personDto.Phones)
                {
                    phone.DDDNumber = 11;
                }
                (bool valid, string mess, int rows) = await personService.CreateContact(personDto);

                Assert.True(valid); 
            }
        }

        [Fact]
        public async Task Should_Consume_CreateContactMessage()
        {
            // Configuração do serviço com MassTransit e Consumer
            var services = new ServiceCollection();

            services.AddMassTransitInMemoryTestHarness(cfg =>
            {
                cfg.AddConsumer<Consumer.CreateContactConsumer>(); // Substitua pelo nome real do Consumer
            });

            var provider = services.BuildServiceProvider(true);
            var harness = provider.GetRequiredService<InMemoryTestHarness>();
            harness.TestInactivityTimeout = TimeSpan.FromSeconds(5);
            await harness.Start();

            try
            {
                var message = new PersonRequestByDDDDto
                {
                    // Preencha com dados de teste apropriados
                    Name = "João da Silva",
                    CPF = "12345678901",
                    Email = "teste@teste.com",
                    Birthday = DateTime.Now,
                    Phones = new List<PhoneRequestByDDDDto>
                    {
                        new PhoneRequestByDDDDto
                        {
                            DDDNumber = 11,
                            PhoneNumber = "999999999",
                            PhoneType = "Celular"
                        }
                    }

                };

                // Publicar a mensagem na fila
                await harness.Bus.Publish(message);

                // Verificar se a mensagem foi consumida
                Assert.True(await harness.Consumed.Any<PersonRequestByDDDDto>());

                // Você pode adicionar verificações adicionais para ver se o consumidor fez o que deveria
            }
            finally
            {
                await harness.Stop();
                await provider.DisposeAsync();
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
