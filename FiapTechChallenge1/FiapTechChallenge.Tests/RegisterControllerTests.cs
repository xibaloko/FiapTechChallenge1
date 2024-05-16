using FiapTechChallenge.API.Controllers;
using FiapTechChallenge.AppService.Interfaces;
using FiapTechChallenge.Domain.DTOs.RequestsDto;
using FiapTechChallenge.Domain.Entities;
using FiapTechChallenge.Infra.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace FiapTechChallenge.Tests
{
    public class RegisterControllerTests
    {
        private RegisterController _controller;
        private Mock<IUnitOfWork> _unitOfWorkMock;
        private Mock<IPersonService> _personService;
        
        
        public RegisterControllerTests()
        {

            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _personService = new Mock<IPersonService>();
            _controller = new RegisterController(_unitOfWorkMock.Object,_personService.Object);
        }

        [Fact]
        public async Task CreateContactV2_ValidData_ShouldReturnCreatedAtActionResult()
        {
            var personRequestDto = new PersonRequestByIdDto
            {
                Name = "John Doe",
                Birthday = new DateTime(1990, 5, 15),
                CPF = "123.456.789-00",
                Email = "john.doe@example.com",
                Phones = new List<PhoneRequestByIdDto>
            {
                 new PhoneRequestByIdDto { PhoneNumber = "123456789", PhoneTypeId = 1, DDDId = 1 },
                 new PhoneRequestByIdDto { PhoneNumber = "987654321", PhoneTypeId = 2, DDDId = 2 }
                }
            };

            // Mocking async method AddAsync to return a completed Task
            _unitOfWorkMock.Setup(uow => uow.Person.AddAsync(It.IsAny<Person>()))
                          .Returns(Task.CompletedTask);

            // Act
            var result = await _controller.CreateContactV2(personRequestDto);

            // Assert
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result);
            Assert.Null(createdAtActionResult.Value); // Assuming the action returns null for the created resource
        }

        // Add more test methods for other scenarios like invalid data, exception handling, etc.
    }
}
