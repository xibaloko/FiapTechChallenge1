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
                Assert.Single(returnValue);
            }
        }

        [Fact]
        public async Task GetContactById_ReturnsOkResponse_WithContact()
        {
            // Arrange
            using (var scope = ServiceProvider.CreateScope())
            {
                var contactsServices = scope.ServiceProvider.GetService<IContactsServices>();
                var contactController = new ContactController(contactsServices);

                // Adicione um contato de teste ao banco de dados ou mock
                var testContact = new Contact { Id = 1, Name = "John Doe", Email = "john.doe@example.com" };
                await contactsServices.AddContact(testContact);

                // Act
                var result = await contactController.GetContactById(1);

                // Assert
                var okResult = Assert.IsType<OkObjectResult>(result);
                var returnValue = Assert.IsType<Contact>(okResult.Value);
                Assert.Equal(testContact.Id, returnValue.Id);
                Assert.Equal(testContact.Name, returnValue.Name);
                Assert.Equal(testContact.Email, returnValue.Email);
            }
        }

        [Fact]
        public async Task GetContactById_ReturnsNotFoundResponse_WhenContactDoesNotExist()
        {
            // Arrange
            using (var scope = ServiceProvider.CreateScope())
            {
                var contactsServices = scope.ServiceProvider.GetService<IContactsServices>();
                var contactController = new ContactController(contactsServices);

                // Act
                var result = await contactController.GetContactById(999); // Id que não existe

                // Assert
                Assert.IsType<NotFoundResult>(result);
            }
        }

        [Fact]
        public async Task GetContactsByRegion_ReturnsOkResponse_WithListOfContacts()
        {
            // Arrange
            using (var scope = ServiceProvider.CreateScope())
            {
                var contactsServices = scope.ServiceProvider.GetService<IContactsServices>();
                var contactController = new ContactController(contactsServices);

                // Adicione contatos de teste para uma região específica
                var testContacts = new List<Contact>
        {
            new Contact { Id = 1, Name = "John Doe", Email = "john.doe@example.com", RegionId = 1 },
            new Contact { Id = 2, Name = "Jane Smith", Email = "jane.smith@example.com", RegionId = 1 }
        };

                foreach (var contact in testContacts)
                {
                    await contactsServices.AddContact(contact);
                }

                // Act
                var result = await contactController.GetContactsByRegion(1);

                // Assert
                var okResult = Assert.IsType<OkObjectResult>(result);
                var returnValue = Assert.IsType<List<Contact>>(okResult.Value);
                Assert.Equal(testContacts.Count, returnValue.Count);
                Assert.All(returnValue, contact => Assert.Equal(1, contact.RegionId));
            }
        }

        [Fact]
        public async Task GetContactsByRegion_ReturnsNotFoundResponse_WhenNoContactsInRegion()
        {
            // Arrange
            using (var scope = ServiceProvider.CreateScope())
            {
                var contactsServices = scope.ServiceProvider.GetService<IContactsServices>();
                var contactController = new ContactController(contactsServices);

                // Act
                var result = await contactController.GetContactsByRegion(999); // Região que não existe ou sem contatos

                // Assert
                Assert.IsType<NotFoundResult>(result);
            }
        }

        [Fact]
        public async Task GetContactsByDDD_ReturnsOkResponse_WithListOfContacts()
        {
            // Arrange
            using (var scope = ServiceProvider.CreateScope())
            {
                var contactsServices = scope.ServiceProvider.GetService<IContactsServices>();
                var contactController = new ContactController(contactsServices);

                // Adicione contatos de teste para um DDD específico
                var testContacts = new List<Contact>
        {
            new Contact { Id = 1, Name = "John Doe", Email = "john.doe@example.com", DDD = 11 },
            new Contact { Id = 2, Name = "Jane Smith", Email = "jane.smith@example.com", DDD = 11 }
        };

                foreach (var contact in testContacts)
                {
                    await contactsServices.AddContact(contact);
                }

                // Act
                var result = await contactController.GetContactsByDDD(11);

                // Assert
                var okResult = Assert.IsType<OkObjectResult>(result);
                var returnValue = Assert.IsType<List<Contact>>(okResult.Value);
                Assert.Equal(testContacts.Count, returnValue.Count);
                Assert.All(returnValue, contact => Assert.Equal(11, contact.DDD));
            }
        }

        [Fact]
        public async Task GetContactsByDDD_ReturnsNotFoundResponse_WhenNoContactsWithDDD()
        {
            // Arrange
            using (var scope = ServiceProvider.CreateScope())
            {
                var contactsServices = scope.ServiceProvider.GetService<IContactsServices>();
                var contactController = new ContactController(contactsServices);

                // Act
                var result = await contactController.GetContactsByDDD(999); // DDD que não existe ou sem contatos

                // Assert
                Assert.IsType<NotFoundResult>(result);
            }
        }

        [Fact]
        public async Task CreateContact_ReturnsCreatedAtActionResponse_WithValidContact()
        {
            // Arrange
            using (var scope = ServiceProvider.CreateScope())
            {
                var contactsServices = scope.ServiceProvider.GetService<IContactsServices>();
                var contactController = new ContactController(contactsServices);

                var newContact = new PersonRequestByDDDDto
                {
                    Name = "John Doe",
                    Email = "john.doe@example.com",
                    DDD = 11
                };

                // Act
                var result = await contactController.CreateContact(newContact);

                // Assert
                var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result);
                Assert.Equal(nameof(contactController.GetContactById), createdAtActionResult.ActionName);
                Assert.NotNull(createdAtActionResult.RouteValues["id"]);
            }
        }

        [Fact]
        public async Task CreateContact_ReturnsBadRequestResponse_WithInvalidModel()
        {
            // Arrange
            using (var scope = ServiceProvider.CreateScope())
            {
                var contactsServices = scope.ServiceProvider.GetService<IContactsServices>();
                var contactController = new ContactController(contactsServices);

                var invalidContact = new PersonRequestByDDDDto
                {
                    // Campos obrigatórios não preenchidos ou preenchidos incorretamente
                    Email = "invalid-email"
                };

                contactController.ModelState.AddModelError("Name", "The Name field is required.");
                contactController.ModelState.AddModelError("DDD", "The DDD field is required.");

                // Act
                var result = await contactController.CreateContact(invalidContact);

                // Assert
                var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
                var modelState = Assert.IsType<SerializableError>(badRequestResult.Value);
                Assert.True(modelState.ContainsKey("Name"));
                Assert.True(modelState.ContainsKey("DDD"));
            }
        }

        [Fact]
        public async Task CreateContact_ReturnsBadRequestResponse_WhenCreationFails()
        {
            // Arrange
            using (var scope = ServiceProvider.CreateScope())
            {
                var contactsServices = scope.ServiceProvider.GetService<IContactsServices>();
                var contactController = new ContactController(contactsServices);

                var newContact = new PersonRequestByDDDDto
                {
                    Name = "John Doe",
                    Email = "john.doe@example.com",
                    DDD = 11
                };

                // Mocking the CreateContact method to return a failure status
                var mockContactsServices = new Mock<IContactsServices>();
                mockContactsServices.Setup(s => s.CreateContact(It.IsAny<PersonRequestByDDDDto>()))
                    .ReturnsAsync((false, "Creation failed", 0));

                contactController = new ContactController(mockContactsServices.Object);

                // Act
                var result = await contactController.CreateContact(newContact);

                // Assert
                var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
                Assert.Equal("Creation failed", badRequestResult.Value);
            }
        }

        [Fact]
        public async Task UpdateContact_ReturnsOkResponse_WithUpdatedContact()
        {
            // Arrange
            using (var scope = ServiceProvider.CreateScope())
            {
                var contactsServices = scope.ServiceProvider.GetService<IContactsServices>();
                var contactController = new ContactController(contactsServices);

                var existingContactId = 1;
                var existingContact = new Contact { Id = existingContactId, Name = "John Doe", Email = "john.doe@example.com", DDD = 11 };
                await contactsServices.AddContact(existingContact);

                var updatedContactDto = new PersonRequestByDDDDto
                {
                    Name = "John Doe Updated",
                    Email = "john.doe.updated@example.com",
                    DDD = 11
                };

                // Act
                var result = await contactController.UpdateContact(existingContactId, updatedContactDto);

                // Assert
                var okResult = Assert.IsType<OkObjectResult>(result);
                var returnValue = Assert.IsType<PersonResponseDto>(okResult.Value);
                Assert.Equal(updatedContactDto.Name, returnValue.Name);
                Assert.Equal(updatedContactDto.Email, returnValue.Email);
            }
        }

        [Fact]
        public async Task UpdateContact_ReturnsBadRequestResponse_WithInvalidModel()
        {
            // Arrange
            using (var scope = ServiceProvider.CreateScope())
            {
                var contactsServices = scope.ServiceProvider.GetService<IContactsServices>();
                var contactController = new ContactController(contactsServices);

                var invalidContactDto = new PersonRequestByDDDDto
                {
                    // Campos obrigatórios não preenchidos ou preenchidos incorretamente
                    Email = "invalid-email"
                };

                contactController.ModelState.AddModelError("Name", "The Name field is required.");
                contactController.ModelState.AddModelError("DDD", "The DDD field is required.");

                // Act
                var result = await contactController.UpdateContact(1, invalidContactDto);

                // Assert
                var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
                var modelState = Assert.IsType<SerializableError>(badRequestResult.Value);
                Assert.True(modelState.ContainsKey("Name"));
                Assert.True(modelState.ContainsKey("DDD"));
            }
        }

        [Fact]
        public async Task UpdateContact_ReturnsBadRequestResponse_WhenUpdateFails()
        {
            // Arrange
            using (var scope = ServiceProvider.CreateScope())
            {
                var contactsServices = scope.ServiceProvider.GetService<IContactsServices>();
                var contactController = new ContactController(contactsServices);

                var updateContactDto = new PersonRequestByDDDDto
                {
                    Name = "John Doe Updated",
                    Email = "john.doe.updated@example.com",
                    DDD = 11
                };

                // Mocking the UpdateContact method to return a failure status
                var mockContactsServices = new Mock<IContactsServices>();
                mockContactsServices.Setup(s => s.UpdateContact(It.IsAny<int>(), It.IsAny<PersonRequestByDDDDto>()))
                    .ReturnsAsync((false, "Update failed", null));

                contactController = new ContactController(mockContactsServices.Object);

                // Act
                var result = await contactController.UpdateContact(1, updateContactDto);

                // Assert
                var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
                Assert.Equal("Update failed", badRequestResult.Value);
            }
        }

        [Fact]
        public async Task UpdateContact_ReturnsBadRequestResponse_WhenContactNotFound()
        {
            // Arrange
            using (var scope = ServiceProvider.CreateScope())
            {
                var contactsServices = scope.ServiceProvider.GetService<IContactsServices>();
                var contactController = new ContactController(contactsServices);

                var updateContactDto = new PersonRequestByDDDDto
                {
                    Name = "Nonexistent Contact",
                    Email = "nonexistent@example.com",
                    DDD = 11
                };

                // Act
                var result = await contactController.UpdateContact(999, updateContactDto); // ID inexistente

                // Assert
                var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
                Assert.Equal("Update failed", badRequestResult.Value);
            }
        }

        [Fact]
        public async Task DeleteContact_ReturnsOkResponse_WhenContactDeletedSuccessfully()
        {
            // Arrange
            using (var scope = ServiceProvider.CreateScope())
            {
                var contactsServices = scope.ServiceProvider.GetService<IContactsServices>();
                var contactController = new ContactController(contactsServices);

                var contactId = 1;
                var contact = new Contact { Id = contactId, Name = "John Doe", Email = "john.doe@example.com", DDD = 11 };
                await contactsServices.AddContact(contact);

                // Act
                var result = await contactController.DeleteContact(contactId);

                // Assert
                var okResult = Assert.IsType<OkObjectResult>(result);
                var returnValue = Assert.IsType<Dictionary<string, string>>(okResult.Value);
                Assert.Equal("This person was successfully deleted.", returnValue["Message"]);
            }
        }

        [Fact]
        public async Task DeleteContact_ReturnsBadRequestResponse_WhenDeletionFails()
        {
            // Arrange
            using (var scope = ServiceProvider.CreateScope())
            {
                var contactsServices = scope.ServiceProvider.GetService<IContactsServices>();
                var contactController = new ContactController(contactsServices);

                var contactId = 999; // ID inexistente

                // Mocking the DeleteContact method to return a failure status
                var mockContactsServices = new Mock<IContactsServices>();
                mockContactsServices.Setup(s => s.DeleteContact(It.IsAny<int>()))
                    .ReturnsAsync((false, "Deletion failed"));

                contactController = new ContactController(mockContactsServices.Object);

                // Act
                var result = await contactController.DeleteContact(contactId);

                // Assert
                var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
                Assert.Equal("Deletion failed", badRequestResult.Value);
            }
        }

        [Fact]
        public async Task DeleteContact_ReturnsBadRequestResponse_WhenContactNotFound()
        {
            // Arrange
            using (var scope = ServiceProvider.CreateScope())
            {
                var contactsServices = scope.ServiceProvider.GetService<IContactsServices>();
                var contactController = new ContactController(contactsServices);

                var contactId = 999; // ID inexistente

                // Act
                var result = await contactController.DeleteContact(contactId);

                // Assert
                var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
                Assert.Equal("Deletion failed", badRequestResult.Value);
            }
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
