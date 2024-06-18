using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;
using YourNamespace.Controllers;
using YourNamespace.DTOs;
using YourNamespace.Models;
using YourNamespace.Services;
using YourNamespace.UnitOfWork;

public class RegisterControllerTests
{
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly Mock<IPersonService> _mockPersonService;
    private readonly RegisterController _controller;

    public RegisterControllerTests()
    {
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        _mockPersonService = new Mock<IPersonService>();
        _controller = new RegisterController(_mockUnitOfWork.Object, _mockPersonService.Object);
    }

    [Fact]
    public async Task GetAllContacts_ReturnsOkResponse_WithListOfContacts()
    {
        // Arrange
        var mockContacts = new List<PersonResponseDto>
        {
            new PersonResponseDto { Id = 1, Name = "John Doe" },
            new PersonResponseDto { Id = 2, Name = "Jane Doe" }
        };

        _mockPersonService.Setup(service => service.GetAllContactsAsync())
            .ReturnsAsync(mockContacts);

        // Act
        var result = await _controller.GetAllContacts();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnValue = Assert.IsType<List<PersonResponseDto>>(okResult.Value);
        Assert.Equal(2, returnValue.Count);
    }

    [Fact]
    public async Task GetContactById_ReturnsOkResponse_WithContact()
    {
        // Arrange
        int contactId = 1;
        var mockContact = new Person
        {
            Id = contactId,
            Name = "John Doe",
            Phones = new List<Phone>
            {
                new Phone { PhoneNumber = "123456789", DDD = new DDD { DDDNumber = 11 }, PhoneType = new PhoneType { Description = "Mobile" } }
            }
        };

        _mockUnitOfWork.Setup(uow => uow.Person.FirstOrDefaultAsync(
            It.IsAny<System.Linq.Expressions.Expression<System.Func<Person, bool>>>(),
            It.IsAny<string>()))
            .ReturnsAsync(mockContact);

        // Act
        var result = await _controller.GetContactById(contactId);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnValue = Assert.IsType<PersonResponseDto>(okResult.Value);
        Assert.Equal(contactId, returnValue.Id);
        Assert.Equal("John Doe", returnValue.Name);
    }

    [Fact]
    public async Task GetContactsByRegion_ReturnsOkResponse_WithListOfContacts()
    {
        // Arrange
        int regionId = 1;
        var mockContacts = new List<Person>
        {
            new Person
            {
                Id = 1,
                Name = "John Doe",
                Phones = new List<Phone>
                {
                    new Phone { PhoneNumber = "123456789", DDD = new DDD { DDDNumber = 11, State = new State { RegionId = regionId } }, PhoneType = new PhoneType { Description = "Mobile" } }
                }
            }
        };

        _mockUnitOfWork.Setup(uow => uow.Person.GetAllAsync(
            It.IsAny<System.Linq.Expressions.Expression<System.Func<Person, bool>>>(),
            It.IsAny<string>()))
            .ReturnsAsync(mockContacts);

        // Act
        var result = await _controller.GetContactsByRegion(regionId);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnValue = Assert.IsType<List<PersonResponseDto>>(okResult.Value);
        Assert.Single(returnValue);
        Assert.Equal("John Doe", returnValue.First().Name);
    }

    [Fact]
    public async Task GetContactsByDDD_ReturnsOkResponse_WithListOfContacts()
    {
        // Arrange
        int ddd = 11;
        var mockContacts = new List<Person>
        {
            new Person
            {
                Id = 1,
                Name = "John Doe",
                Phones = new List<Phone>
                {
                    new Phone { PhoneNumber = "123456789", DDD = new DDD { DDDNumber = ddd }, PhoneType = new PhoneType { Description = "Mobile" } }
                }
            }
        };

        _mockUnitOfWork.Setup(uow => uow.Person.GetAllAsync(
            It.IsAny<System.Linq.Expressions.Expression<System.Func<Person, bool>>>(),
            It.IsAny<string>()))
            .ReturnsAsync(mockContacts);

        // Act
        var result = await _controller.GetContactsByDDD(ddd);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnValue = Assert.IsType<List<PersonResponseDto>>(okResult.Value);
        Assert.Single(returnValue);
        Assert.Equal("John Doe", returnValue.First().Name);
    }

    [Fact]
    public async Task CreateContactV1_ReturnsCreatedResponse()
    {
        // Arrange
        var newContact = new PersonRequestByDDDDto
        {
            Name = "John Doe",
            Birthday = DateTime.Now.AddYears(-30),
            CPF = "123.456.789-00",
            Email = "john.doe@example.com",
            Phones = new List<PhoneRequestByDDDDto>
            {
                new PhoneRequestByDDDDto { PhoneNumber = "123456789", PhoneType = "Mobile", DDDNumber = 11 }
            }
        };

        var ddds = new List<DDD> { new DDD { Id = 1, DDDNumber = 11 } };
        var phoneTypes = new List<PhoneType> { new PhoneType { Id = 1, Description = "Mobile" } };

        _mockUnitOfWork.Setup(uow => uow.DDD.GetAllAsync()).ReturnsAsync(ddds);
        _mockUnitOfWork.Setup(uow => uow.PhoneType.GetAllAsync()).ReturnsAsync(phoneTypes);

        // Act
        var result = await _controller.CreateContactV1(newContact);

        // Assert
        var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result);
        Assert.Equal(nameof(RegisterController.GetContactById), createdAtActionResult.ActionName);
    }

    [Fact]
    public async Task CreateContactV2_ReturnsCreatedResponse()
    {
        // Arrange
        var newContact = new PersonRequestByIdDto
        {
            Name = "Jane Doe",
            Birthday = DateTime.Now.AddYears(-25),
            CPF = "987.654.321-00",
            Email = "jane.doe@example.com",
            Phones = new List<PhoneRequestByIdDto>
            {
                new PhoneRequestByIdDto { PhoneNumber = "987654321", PhoneTypeId = 1, DDDId = 1 }
            }
        };

        // Act
        var result = await _controller.CreateContactV2(newContact);

        // Assert
        var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result);
        Assert.Equal(nameof(RegisterController.GetContactById), createdAtActionResult.ActionName);
    }

    [Fact]
    public async Task UpdateContactV1_ReturnsOkResponse_WithUpdatedContact()
    {
        // Arrange
        int contactId = 1;
        var updatedContact = new PersonRequestByDDDDto
        {
            Name = "John Smith",
            Birthday = DateTime.Now.AddYears(-30),
            CPF = "123.456.789-00",
            Email = "john.smith@example.com",
            Phones = new List<PhoneRequestByDDDDto>
            {
                new PhoneRequestByDDDDto { PhoneNumber = "123456789", PhoneType = "Mobile", DDDNumber = 11 }
            }
        };

        var mockContact = new Person
        {
            Id = contactId,
            Name = "John Doe",
            Phones = new List<Phone>
            {
                new Phone { PhoneNumber = "123456789", DDD = new DDD { DDDNumber = 11 }, PhoneType = new PhoneType { Description = "Mobile" } }
            }
        };

        var ddds = new List<DDD> { new DDD { Id = 1, DDDNumber = 11 } };
        var phoneTypes = new List<PhoneType> { new PhoneType { Id =
