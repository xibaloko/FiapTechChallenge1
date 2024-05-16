using System.Linq.Expressions;
using FiapTechChallenge.AppService.Interfaces;
using FiapTechChallenge.AppService.Services;
using FiapTechChallenge.Domain.DTOs.RequestsDto;
using FiapTechChallenge.Domain.DTOs.ResponsesDto;
using FiapTechChallenge.Domain.Entities;
using FiapTechChallenge.Infra.Interfaces;
using FiapTechChallenge.Tests.Helpers;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace FiapTechChallenge.Tests
{
    public class RegisterControllerTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IPersonRepository> _personRepositoryMock;
        private readonly IPersonService _personService;

        public RegisterControllerTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _personRepositoryMock = new Mock<IPersonRepository>();

            // Configurando o UnitOfWork para retornar o mock do PersonRepository
            _unitOfWorkMock.Setup(uow => uow.Person).Returns(_personRepositoryMock.Object);

            _personService = new PersonService(_unitOfWorkMock.Object);
        }

        [Fact]
        public async Task GetAllContactsAsync_ShouldReturnAllContacts()
        {
            // Arrange
            var contacts = TestDataFactory.GeneratePersons(1);

            _personRepositoryMock
                .Setup(repo => repo.GetAllAsync(
                    It.IsAny<Expression<Func<Person, bool>>>(),
                    It.IsAny<Func<IQueryable<Person>, IOrderedQueryable<Person>>>(),
                    It.IsAny<string>(),
                    It.IsAny<bool>()
                ))
                .ReturnsAsync(contacts);

            // Act
            var result = await _personService.GetAllContactsAsync()!;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(contacts.Count, result.Count);

            var firstContact = result.First();
            var firstGeneratedContact = contacts.First();

            Assert.Equal(firstGeneratedContact.Id, firstContact.Id);
            Assert.Equal(firstGeneratedContact.Name, firstContact.Name);
            Assert.Equal(firstGeneratedContact.Birthday, firstContact.Birthday);
            Assert.Equal(firstGeneratedContact.CPF, firstContact.CPF);
            Assert.Equal(firstGeneratedContact.Email, firstContact.Email);

            var firstPhone = firstContact.Phones.First();
            var firstGeneratedPhone = firstGeneratedContact.Phones.First();

            Assert.Equal(firstGeneratedPhone.PhoneNumber, firstPhone.PhoneNumber);
            Assert.Equal(firstGeneratedPhone.PhoneType.Description, firstPhone.PhoneType);
            Assert.Equal(firstGeneratedPhone.DDD.DDDNumber, firstPhone.DDD);
        }

        [Fact]
        public async Task CreateContactV1_ValidData_ShouldReturnCreatedAtActionResult()
        {
            // Arrange
            // var unitOfWorkMock = new Mock<IUnitOfWork>();
            // var controller = new ContactsController(unitOfWorkMock.Object);

            var personDto = new PersonRequestByDDDDto
            {
                Name = "John Doe",
                Birthday = new DateTime(1990, 5, 15),
                CPF = "123.456.789-00",
                Email = "john.doe@example.com",
                Phones = new List<PhoneRequestByDDDDto>
                {
                    new PhoneRequestByDDDDto { PhoneNumber = "123456789", DDDNumber = 11, PhoneType = "Residencial" },
                    new PhoneRequestByDDDDto { PhoneNumber = "987654321", DDDNumber = 12, PhoneType = "Celular" }
                }
            };

            // Act
            var result = await _controller.CreateContactV1(personDto);

            // Assert
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result);
            Assert.Null(createdAtActionResult.Value); // Assuming the action returns null for the created resource
        }

        [Fact]
        public async Task GetAllContacts_ContactsExist_ShouldReturnOkWithContacts()
        {
            // Arrange
            //var unitOfWorkMock = new Mock<IUnitOfWork>();
            //var controller = new ContactsController(unitOfWorkMock.Object);

            var contacts = new List<Person>
            {
                new Person
                {
                    Id = 1,
                    Name = "John Doe",
                    Birthday = new DateTime(1990, 5, 15),
                    CPF = "123.456.789-00",
                    Email = "john.doe@example.com",
                    Phones = new List<Phone>
                    {
                        new Phone { PhoneNumber = "123456789", DDD = new DDD { DDDNumber = 11 }, PhoneType = new PhoneType { Description = "Residencial" } }
                    }
                }
            };

            // Act
            var result = await _controller.GetAllContacts();

            // Assert
            var okObjectResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsAssignableFrom<IEnumerable<PersonResponseDto>>(okObjectResult.Value);
            Assert.Single(response); // Assuming only one contact is returned
            var personResponseDto = response.First();
            Assert.Equal(1, personResponseDto.Id);
            Assert.Equal("John Doe", personResponseDto.Name);
            Assert.Equal(new DateTime(1990, 5, 15), personResponseDto.Birthday);
            Assert.Equal("123.456.789-00", personResponseDto.CPF);
            Assert.Equal("john.doe@example.com", personResponseDto.Email);
            Assert.Single(personResponseDto.Phones);
            var phoneResponseDto = personResponseDto.Phones.First();
            Assert.Equal("Residencial", phoneResponseDto.PhoneType);
            Assert.Equal(11, phoneResponseDto.DDD);
            Assert.Equal("123456789", phoneResponseDto.PhoneNumber);
        }

        [Fact]
        public async Task GetAllContacts_NoContactsExist_ShouldReturnNotFound()
        {
            // Act
            var result = await _controller.GetAllContacts();

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task GetContactById_ContactExists_ShouldReturnOkWithContact()
        {
            // Arrange
            //var unitOfWorkMock = new Mock<IUnitOfWork>();
            //var controller = new ContactsController(unitOfWorkMock.Object);

            var contact = new Person
            {
                Id = 1,
                Name = "John Doe",
                Birthday = new DateTime(1990, 5, 15),
                CPF = "123.456.789-00",
                Email = "john.doe@example.com",
                Phones = new[]
                {
                    new Phone { PhoneNumber = "123456789", DDD = new DDD { DDDNumber = 11 }, PhoneType = new PhoneType { Description = "Residencial" } }
                }
            };

            // Act
            var result = await _controller.GetContactById(1);

            // Assert
            var okObjectResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsAssignableFrom<PersonResponseDto>(okObjectResult.Value);
            Assert.Equal(1, response.Id);
            Assert.Equal("John Doe", response.Name);
            Assert.Equal(new DateTime(1990, 5, 15), response.Birthday);
            Assert.Equal("123.456.789-00", response.CPF);
            Assert.Equal("john.doe@example.com", response.Email);
            Assert.Single(response.Phones);
            var phoneResponseDto = response.Phones.First();
            Assert.Equal("Residencial", phoneResponseDto.PhoneType);
            Assert.Equal(11, phoneResponseDto.DDD);
            Assert.Equal("123456789", phoneResponseDto.PhoneNumber);
        }

        [Fact]
        public async Task GetContactById_ContactDoesNotExist_ShouldReturnNotFound()
        {
            // Arrange
            //var unitOfWorkMock = new Mock<IUnitOfWork>();
            //var controller = new ContactsController(unitOfWorkMock.Object);

            // Act
            var result = await _controller.GetContactById(1);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task GetContactsByRegion_ContactsExistInRegion_ShouldReturnOkWithContacts()
        {
            // Arrange
            //var unitOfWorkMock = new Mock<IUnitOfWork>();
            //var controller = new ContactsController(unitOfWorkMock.Object);

            var contacts = new List<Person>
            {
                new Person
                {
                    Id = 1,
                    Name = "John Doe",
                    Birthday = new DateTime(1990, 5, 15),
                    CPF = "123.456.789-00",
                    Email = "john.doe@example.com",
                    Phones = new[]
                    {
                        new Phone { PhoneNumber = "123456789", DDD = new DDD { State = new State { RegionId = 1 } }, PhoneType = new PhoneType { Description = "Residencial" } }
                    }
                }
            };

            // Act
            var result = await _controller.GetContactsByRegion(1);

            // Assert
            var okObjectResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsAssignableFrom<IEnumerable<PersonResponseDto>>(okObjectResult.Value);
            Assert.Single(response); // Assuming only one contact is returned
            var personResponseDto = response.First();
            Assert.Equal(1, personResponseDto.Id);
            Assert.Equal("John Doe", personResponseDto.Name);
            Assert.Equal(new DateTime(1990, 5, 15), personResponseDto.Birthday);
            Assert.Equal("123.456.789-00", personResponseDto.CPF);
            Assert.Equal("john.doe@example.com", personResponseDto.Email);
            Assert.Single(personResponseDto.Phones);
            var phoneResponseDto = personResponseDto.Phones.First();
            Assert.Equal("Residencial", phoneResponseDto.PhoneType);
            Assert.Equal("123456789", phoneResponseDto.PhoneNumber);
        }

        [Fact]
        public async Task GetContactsByRegion_NoContactsInRegion_ShouldReturnNotFound()
        {
            // Arrange
            //var unitOfWorkMock = new Mock<IUnitOfWork>();
            //var controller = new ContactsController(unitOfWorkMock.Object);

            // Act
            var result = await _controller.GetContactsByRegion(1);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task GetContactsByDDD_ContactsExistWithDDD_ShouldReturnOkWithContacts()
        {
            // Arrange
            //var unitOfWorkMock = new Mock<IUnitOfWork>();
            //var controller = new ContactsController(unitOfWorkMock.Object);

            var contacts = new List<Person>
            {
                new Person
                {
                    Id = 1,
                    Name = "John Doe",
                    Birthday = new DateTime(1990, 5, 15),
                    CPF = "123.456.789-00",
                    Email = "john.doe@example.com",
                    Phones = new[]
                    {
                        new Phone { PhoneNumber = "123456789", DDD = new DDD { DDDNumber = 11 }, PhoneType = new PhoneType { Description = "Residencial" } }
                    }
                }
            };

            // Act
            var result = await _controller.GetContactsByDDD(11);

            // Assert
            var okObjectResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsAssignableFrom<IEnumerable<PersonResponseDto>>(okObjectResult.Value);
            Assert.Single(response); // Assuming only one contact is returned
            var personResponseDto = response.First();
            Assert.Equal(1, personResponseDto.Id);
            Assert.Equal("John Doe", personResponseDto.Name);
            Assert.Equal(new DateTime(1990, 5, 15), personResponseDto.Birthday);
            Assert.Equal("123.456.789-00", personResponseDto.CPF);
            Assert.Equal("john.doe@example.com", personResponseDto.Email);
            Assert.Single(personResponseDto.Phones);
            var phoneResponseDto = personResponseDto.Phones.First();
            Assert.Equal("Residencial", phoneResponseDto.PhoneType);
            Assert.Equal(11, phoneResponseDto.DDD);
            Assert.Equal("123456789", phoneResponseDto.PhoneNumber);
        }

        [Fact]
        public async Task GetContactsByDDD_NoContactsWithDDD_ShouldReturnNotFound()
        {
            // Arrange
            //var unitOfWorkMock = new Mock<IUnitOfWork>();
            //var controller = new ContactsController(unitOfWorkMock.Object);

            // Act
            var result = await _controller.GetContactsByDDD(11);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        //ContactsQueryTests FIM

        //Update INICIO

        [Fact]
        public async Task UpdateContactV1_ValidData_ShouldReturnOkWithModifiedContact()
        {
            // Arrange
            //var unitOfWorkMock = new Mock<IUnitOfWork>();
            //var controller = new ContactsController(unitOfWorkMock.Object);

            var id = 1;

            var personDto = new PersonRequestByDDDDto
            {
                Name = "Updated Name",
                Birthday = new DateTime(1990, 5, 15),
                CPF = "123.456.789-00",
                Email = "updated.email@example.com",
                Phones = new List<PhoneRequestByDDDDto>
                {
                    new PhoneRequestByDDDDto { PhoneNumber = "987654321", DDDNumber = 21, PhoneType = "Celular" }
                }
            };

            var ddds = new List<DDD> { new DDD { Id = 1, DDDNumber = 11 }, new DDD { Id = 2, DDDNumber = 21 } };
            var phoneTypes = new List<PhoneType> { new PhoneType { Id = 1, Description = "Residencial" }, new PhoneType { Id = 2, Description = "Celular" } };
            var originalPerson = new Person
            {
                Id = id,
                Name = "Original Name",
                Birthday = new DateTime(1980, 1, 1),
                CPF = "987.654.321-00",
                Email = "original.email@example.com",
                Phones = new List<Phone>
                {
                    new Phone { Id = 1, PhoneNumber = "123456789", DDD = ddds[0], PhoneType = phoneTypes[0] }
                }
            };
         
            // Act
            var result = await _controller.UpdateContactV1(id, personDto);

            // Assert
            var okObjectResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<PersonResponseDto>(okObjectResult.Value);
            Assert.Equal(id, response.Id);
            Assert.Equal("Updated Name", response.Name);
            Assert.Equal(new DateTime(1990, 5, 15), response.Birthday);
            Assert.Equal("123.456.789-00", response.CPF);
            Assert.Equal("updated.email@example.com", response.Email);
            Assert.Single(response.Phones);
            var phoneResponseDto = response.Phones.First();
            Assert.Equal("Celular", phoneResponseDto.PhoneType);
            Assert.Equal(21, phoneResponseDto.DDD);
            Assert.Equal("987654321", phoneResponseDto.PhoneNumber);
        }

        [Fact]
        public async Task UpdateContactV1_InvalidData_ShouldReturnBadRequest()
        {
            // Arrange

            var id = 1;
            var personDto = new PersonRequestByDDDDto
            {
                Name = "John Doe",
                Birthday = new DateTime(1990, 5, 15),
                CPF = "123.456.789-00",
                Email = "john.doe@example.com",
                Phones = new List<PhoneRequestByDDDDto>
                {
                    new PhoneRequestByDDDDto { PhoneNumber = "123456789", DDDNumber = 11, PhoneType = "Residencial" },
                    new PhoneRequestByDDDDto { PhoneNumber = "987654321", DDDNumber = 12, PhoneType = "Celular" }
                }
            };

            // Act
            var result = await _controller.UpdateContactV1(id, personDto);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task UpdateContactV2_ValidData_ShouldReturnOkWithModifiedContact()
        {
            // Arrange

            var id = 1;

            var ddds = new List<DDD> { new DDD { Id = 1, DDDNumber = 11 }, new DDD { Id = 2, DDDNumber = 21 } };
            var phoneTypes = new List<PhoneType> { new PhoneType { Id = 1, Description = "Residencial" }, new PhoneType { Id = 2, Description = "Celular" } };
            var personDto = new PersonRequestByIdDto
            {
                Name = "Updated Name",
                Birthday = new DateTime(1990, 5, 15),
                CPF = "123.456.789-00",
                Email = "updated.email@example.com",
                Phones = new List<PhoneRequestByIdDto>
                {
                     new PhoneRequestByIdDto { PhoneNumber = "123456789", PhoneTypeId = 1, DDDId = 1 }
                }
            };

            var originalPerson = new Person
            {
                Id = id,
                Name = "Original Name",
                Birthday = new DateTime(1980, 1, 1),
                CPF = "987.654.321-00",
                Email = "original.email@example.com",
                Phones = new List<Phone>
                {
                    new Phone { Id = 1, PhoneNumber = "123456789", DDDId = 2, PhoneTypeId = 2 }
                }
            };

            // Act
            var result = await _controller.UpdateContactV2(id, personDto);

            // Assert
            var okObjectResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<PersonResponseDto>(okObjectResult.Value);
            Assert.Equal(id, response.Id);
            Assert.Equal("Updated Name", response.Name);
            Assert.Equal(new DateTime(1990, 5, 15), response.Birthday);
            Assert.Equal("123.456.789-00", response.CPF);
            Assert.Equal("updated.email@example.com", response.Email);
            Assert.Single(response.Phones);
            var phoneResponseDto = response.Phones.First();
            Assert.Equal("123456789", phoneResponseDto.PhoneNumber);
            Assert.Equal(12, phoneResponseDto.DDD);
            Assert.Equal("Residencial", phoneResponseDto.PhoneType);
        }

        [Fact]
        public async Task UpdateContactV2_InvalidData_ShouldReturnBadRequest()
        {
            // Arrange
            //var unitOfWorkMock = new Mock<IUnitOfWork>();
            //var controller = new ContactsController(unitOfWorkMock.Object);

            var id = 1;
            var personDto = new PersonRequestByIdDto
            {
                Name = "Updated Name",
                Birthday = new DateTime(1990, 5, 15),
                CPF = "123.456.789-00",
                Email = "updated.email@example.com",
                Phones = new List<PhoneRequestByIdDto>
                {
                     new PhoneRequestByIdDto { PhoneNumber = "123456789", PhoneTypeId = 1, DDDId = 1 }
                }
            };

            // Act
            var result = await _controller.UpdateContactV2(id, personDto);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }
        //Update FIM

        //Delete Ini
        [Fact]
        public async Task DeleteContact_ContactExists_ShouldReturnOkWithSuccessfulMessage()
        {
            // Arrange
            //var unitOfWorkMock = new Mock<IUnitOfWork>();
            //var controller = new ContactsController(unitOfWorkMock.Object);

            var id = 1;

            var person = new Person
            {
                Id = id,
                Name = "John Doe",
                Birthday = new DateTime(1990, 5, 15),
                CPF = "123.456.789-00",
                Email = "john.doe@example.com"
            };

            // Act
            var result = await _controller.DeleteContact(id);

            // Assert
            var okObjectResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<OkObjectResult>(okObjectResult);
            Assert.Equal("This person was successfully deleted.", response.Value);
        }

        [Fact]
        public async Task DeleteContact_ContactDoesNotExist_ShouldReturnNotFound()
        {
            // Arrange
            //var unitOfWorkMock = new Mock<IUnitOfWork>();
            //var controller = new ContactsController(unitOfWorkMock.Object);

            var id = 1;
            
            // Act
            var result = await _controller.DeleteContact(id);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            var response = Assert.IsType<NotFoundObjectResult>(notFoundResult);
            Assert.Equal("This person was not found.", response.Value);
        }
    }
}
