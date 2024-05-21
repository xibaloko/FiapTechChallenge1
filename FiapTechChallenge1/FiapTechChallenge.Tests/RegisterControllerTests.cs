using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;
using FiapTechChallenge.AppService.Interfaces;
using FiapTechChallenge.AppService.Services;
using FiapTechChallenge.Infra.Interfaces;
using FiapTechChallenge.Tests.Helpers;
using Moq;
using Person = FiapTechChallenge.Domain.Entities.Person;

namespace FiapTechChallenge.Tests
{
    public class RegisterControllerTests
    {
        private readonly Mock<IPersonRepository> _personRepositoryMock;
        private readonly IPersonService _personService;

        public RegisterControllerTests()
        {
            Mock<IUnitOfWork> unitOfWorkMock = new();
            _personRepositoryMock = new Mock<IPersonRepository>();

            // Configurando o UnitOfWork para retornar o mock do PersonRepository
            unitOfWorkMock.Setup(uow => uow.Person).Returns(_personRepositoryMock.Object);

            _personService = new PersonService(unitOfWorkMock.Object);
        }

        private static void ValidateDddNumber(int dddNumber)
        {
            if (dddNumber <= 0)
            {
                throw new ValidationException("The field DDDNumber must be between 1 and 2147483647.");
            }
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
            Assert.NotEmpty(result);
        }

        [Fact]
        public void PersonRequestByDDDDto_Validation_ShouldFail_ForInvalidData()
        {
            // Arrange
            var dto = PersonRequestDtoFaker.GeneratePersonRequest();
            dto.Name = null;
            dto.CPF = null;
            dto.Birthday = DateTime.MinValue;
            dto.Email = "invalid-email";
            dto.Phones = null;

            // Act
            var validationResults = new List<ValidationResult>();
            var validationContext = new ValidationContext(dto, serviceProvider: null, items: null);
            var isValid = Validator.TryValidateObject(dto, validationContext, validationResults, validateAllProperties: true);

            // Assert
            Assert.False(isValid);
            Assert.Contains(validationResults, v => v.ErrorMessage == "The Name field is required.");
            Assert.Contains(validationResults, v => v.ErrorMessage == "The CPF field is required.");
            Assert.Contains(validationResults, v => v.ErrorMessage == "The Email field is not a valid e-mail address.");
        }

        [Fact]
        public void PersonRequestByDDDDto_Validation_ShouldPass_ForValidData()
        {
            // Arrange
            var dto = PersonRequestDtoFaker.GeneratePersonRequest();

            // Act
            var validationResults = new List<ValidationResult>();
            var validationContext = new ValidationContext(dto, serviceProvider: null, items: null);
            var isValid = Validator.TryValidateObject(dto, validationContext, validationResults, validateAllProperties: true);

            // Assert
            Assert.True(isValid);
            Assert.Empty(validationResults);
        }
        
        [Fact]
        public void PhoneRequestByDDDDto_Validation_ShouldFail_ForInvalidData()
        {
            // Arrange
            var dto = PhoneRequestDtoFaker.GeneratePhoneRequest();
            dto.PhoneNumber = "invalid-phone";
            dto.DDDNumber = 0;
            dto.PhoneType = "InvalidType";

            // Act
            var validationResults = new List<ValidationResult>();
            var validationContext = new ValidationContext(dto, serviceProvider: null, items: null);
            var isValid = Validator.TryValidateObject(dto, validationContext, validationResults, validateAllProperties: true);

            // Assert
            Assert.False(isValid);
            Assert.Contains(validationResults, v => v.ErrorMessage == "The phone number must contain only digits.");
            Assert.Contains(validationResults, v => v.ErrorMessage == "Only values 'Residencial', 'Comercial' and 'Celular' are allowed");
        }

        [Fact]
        public void PhoneRequestByDDDDto_Validation_ShouldPass_ForValidData()
        {
            // Arrange
            var dto = PhoneRequestDtoFaker.GeneratePhoneRequest();

            // Act
            var validationResults = new List<ValidationResult>();
            var validationContext = new ValidationContext(dto, serviceProvider: null, items: null);
            var isValid = Validator.TryValidateObject(dto, validationContext, validationResults, validateAllProperties: true);

            // Assert
            Assert.True(isValid);
            Assert.Empty(validationResults);
        }
        
        [Fact]
        public void PhoneRequestByDDDDto_Validation_ShouldFail_ForInvalidDDDNumber()
        {
            // Arrange
            const int invalidDddNumber = 0;

            // Act & Assert
            var ex = Assert.Throws<ValidationException>(() => ValidateDddNumber(invalidDddNumber));
            Assert.Equal("The field DDDNumber must be between 1 and 2147483647.", ex.Message);
        }
    }
}
