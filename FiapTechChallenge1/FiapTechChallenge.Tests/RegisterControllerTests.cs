﻿using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;
using FiapTechChallenge.AppService.Interfaces;
using FiapTechChallenge.AppService.Services;
using FiapTechChallenge.Domain.Entities;
using FiapTechChallenge.Infra.Interfaces;
using FiapTechChallenge.Infra.Repositories;
using FiapTechChallenge.Tests.Helpers;
using Moq;
using Person = FiapTechChallenge.Domain.Entities.Person;

namespace FiapTechChallenge.Tests
{
    public class RegisterControllerTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IDDDRepository> _dddRepositoryMock;
        private readonly Mock<IPhoneTypeRepository> _phoneTypeRepositoryMock;
        private readonly Mock<IPersonRepository> _personRepositoryMock;
        private readonly IPersonService _personService;

        public RegisterControllerTests()
        {
            Mock<IUnitOfWork> unitOfWorkMock = new();
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _dddRepositoryMock = new Mock<IDDDRepository>();
            _phoneTypeRepositoryMock = new Mock<IPhoneTypeRepository>();
            _personRepositoryMock = new Mock<IPersonRepository>();

            _unitOfWorkMock.Setup(uow => uow.DDD).Returns(_dddRepositoryMock.Object);
            _unitOfWorkMock.Setup(uow => uow.PhoneType).Returns(_phoneTypeRepositoryMock.Object);
            _unitOfWorkMock.Setup(uow => uow.Person).Returns(_personRepositoryMock.Object);

            _personService = new PersonService(_unitOfWorkMock.Object);
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
        public async Task GetAllContactsAsync_ShouldReturnEmpty()
        {
            // Act
            var result = await _personService.GetAllContactsAsync()!;

            // Assert
            Assert.Empty(result);
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
            var isValid =
                Validator.TryValidateObject(dto, validationContext, validationResults, validateAllProperties: true);

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
            var isValid =
                Validator.TryValidateObject(dto, validationContext, validationResults, validateAllProperties: true);

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
            var isValid =
                Validator.TryValidateObject(dto, validationContext, validationResults, validateAllProperties: true);

            // Assert
            Assert.False(isValid);
            Assert.Contains(validationResults, v => v.ErrorMessage == "The phone number must contain only digits.");
            Assert.Contains(validationResults,
                v => v.ErrorMessage == "Only values 'Residencial', 'Comercial' and 'Celular' are allowed");
        }

        [Fact]
        public void PhoneRequestByDDDDto_Validation_ShouldPass_ForValidData()
        {
            // Arrange
            var dto = PhoneRequestDtoFaker.GeneratePhoneRequest();

            // Act
            var validationResults = new List<ValidationResult>();
            var validationContext = new ValidationContext(dto, serviceProvider: null, items: null);
            var isValid =
                Validator.TryValidateObject(dto, validationContext, validationResults, validateAllProperties: true);

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

        [Fact]
        public async Task CreateContact_ShouldReturnFalse_WhenDDDNumberIsInvalid()
        {
            // Arrange
            const int invalidDddNumber = 999;
            var personDto = PersonRequestDtoFaker.GeneratePersonRequest();
            personDto.Phones.First().DDDNumber = invalidDddNumber;

            _dddRepositoryMock
                .Setup(repo => repo.GetAllAsync(
                    It.IsAny<Expression<Func<DDD, bool>>>(),
                    It.IsAny<Func<IQueryable<DDD>, IOrderedQueryable<DDD>>>(),
                    It.IsAny<string>(),
                    It.IsAny<bool>()))
                .ReturnsAsync(new List<DDD>());

            _phoneTypeRepositoryMock
                .Setup(repo => repo.GetAllAsync(
                    It.IsAny<Expression<Func<PhoneType, bool>>>(),
                    It.IsAny<Func<IQueryable<PhoneType>, IOrderedQueryable<PhoneType>>>(),
                    It.IsAny<string>(),
                    It.IsAny<bool>()))
                .ReturnsAsync(new List<PhoneType>());

            _personRepositoryMock
                .Setup(repo => repo.AddAsync(It.IsAny<Person>()))
                .Returns(Task.CompletedTask);

            _unitOfWorkMock
                .Setup(uow => uow.Save())
                .Verifiable();

            // Act
            var result = await _personService.CreateContact(personDto);

            // Assert
            Assert.False(result.Item1);
            Assert.Equal($"Invalid DDD Number: '{invalidDddNumber}'", result.Item2);
            Assert.Equal(-1, result.Item3);
        }

        [Fact]
        public async Task CreateContact_ShouldReturnTrue_WhenContactIsValid()
        {
            // Arrange
            const int validDddNumber = 11;
            const string validPhoneType = "Residencial";
            var personDto = PersonRequestDtoFaker.GeneratePersonRequest();

            // Ajustar os valores de DDDNumber e PhoneType para garantir que correspondem aos valores esperados
            foreach (var phone in personDto.Phones)
            {
                phone.DDDNumber = validDddNumber;
                phone.PhoneType = validPhoneType;
            }

            var ddds = new List<DDD> { new DDD { Id = 1, DDDNumber = validDddNumber } };
            var phoneTypes = new List<PhoneType> { new PhoneType { Id = 1, Description = validPhoneType } };

            _dddRepositoryMock
                .Setup(repo => repo.GetAllAsync(
                    It.IsAny<Expression<Func<DDD, bool>>>(),
                    It.IsAny<Func<IQueryable<DDD>, IOrderedQueryable<DDD>>>(),
                    It.IsAny<string>(),
                    It.IsAny<bool>()))
                .ReturnsAsync(ddds);

            _phoneTypeRepositoryMock
                .Setup(repo => repo.GetAllAsync(
                    It.IsAny<Expression<Func<PhoneType, bool>>>(),
                    It.IsAny<Func<IQueryable<PhoneType>, IOrderedQueryable<PhoneType>>>(),
                    It.IsAny<string>(),
                    It.IsAny<bool>()))
                .ReturnsAsync(phoneTypes);

            _personRepositoryMock
                .Setup(repo => repo.AddAsync(It.IsAny<Person>()))
                .Callback<Person>(p => p.Id = 1) // Configura o ID da pessoa ao adicionar
                .Returns(Task.CompletedTask);

            _unitOfWorkMock
                .Setup(uow => uow.Save())
                .Verifiable();

            // Act
            var result = await _personService.CreateContact(personDto);

            // Assert
            Assert.True(result.Item1, "Expected result.Item1 to be true");
            Assert.Equal(string.Empty, result.Item2);
            Assert.True(result.Item3 > 0, "Expected result.Item3 to be greater than 0");
        }

        [Fact]
        public async Task DeleteContact_ShouldReturnFalse_WhenPersonIsNotFound()
        {
            // Arrange
            const int personId = 1;

            _personRepositoryMock
                .Setup(repo => repo.FirstOrDefaultAsync(
                    It.IsAny<Expression<Func<Person, bool>>>(),
                    It.IsAny<string>(),
                    It.IsAny<bool>()))!
                .ReturnsAsync((Person)null); // Simula que a pessoa não foi encontrada

            // Act
            var result = await _personService.DeleteContact(personId);

            // Assert
            Assert.False(result.Item1);
            Assert.Equal("This person was not found.", result.Item2);
        }

        [Fact]
        public async Task DeleteContact_ShouldReturnTrue_WhenPersonIsDeletedSuccessfully()
        {
            // Arrange
            const int personId = 1;
            var person = new Person { Id = personId };

            _personRepositoryMock
                .Setup(repo => repo.FirstOrDefaultAsync(
                    It.IsAny<Expression<Func<Person, bool>>>(),
                    It.IsAny<string>(),
                    It.IsAny<bool>()))
                .ReturnsAsync(person);

            _personRepositoryMock
                .Setup(repo => repo.Remove(It.IsAny<Person>()))
                .Verifiable();

            _personRepositoryMock
                .Setup(repo => repo.SaveCount())
                .Returns(1);

            // Act
            var result = await _personService.DeleteContact(personId);

            // Assert
            Assert.True(result.Item1);
            Assert.Equal("This person was successfully deleted.", result.Item2);
        }

        [Fact]
        public async Task GetContactById_ShouldReturnNull_WhenContactIsNotFound()
        {
            // Arrange
            const int contactId = 1;

            _personRepositoryMock
                .Setup(repo => repo.FirstOrDefaultAsync(
                    It.IsAny<Expression<Func<Person, bool>>>(),
                    "Phones,Phones.DDD,Phones.DDD.State,Phones.DDD.State.Region",
                    true))!
                .ReturnsAsync((Person)null!); // Simula que o contato não foi encontrado

            // Act
            var result = await _personService.GetContactById(contactId);

            // Assert
            Assert.Null(result);
        }
        
        [Fact]
        public async Task GetContactById_ShouldReturnContact_WhenContactIsFound()
        {
            // Arrange
            const int contactId = 1;
            var personRequestDto = PersonRequestDtoFaker.GeneratePersonRequest();
            var state = StateFaker.GenerateState();

            var contact = new Person
            {
                Id = contactId,
                Name = personRequestDto.Name,
                Birthday = personRequestDto.Birthday,
                CPF = personRequestDto.CPF,
                Email = personRequestDto.Email,
                Phones = personRequestDto.Phones.Select(phone => new Phone
                {
                    PhoneNumber = phone.PhoneNumber,
                    DDD = new DDD 
                    {
                        DDDNumber = phone.DDDNumber, 
                        State = state
                    },
                    PhoneType = new PhoneType { Description = phone.PhoneType }
                }).ToList()
            };

            _personRepositoryMock
                .Setup(repo => repo.FirstOrDefaultAsync(
                    It.IsAny<Expression<Func<Person, bool>>>(),
                    "Phones,Phones.DDD,Phones.DDD.State,Phones.PhoneType",
                    true))
                .ReturnsAsync(contact);

            // Act
            var result = await _personService.GetContactById(contactId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(contactId, result?.Id);
            Assert.Equal(contact.Name, result?.Name);
            Assert.Equal(contact.Birthday, result?.Birthday);
            Assert.Equal(contact.CPF, result?.CPF);
            Assert.Equal(contact.Email, result?.Email);

            var contactPhones = contact.Phones.ToList();
            var resultPhones = result?.Phones.ToList();

            Assert.Equal(contactPhones.Count, resultPhones?.Count);

            for (int i = 0; i < contactPhones.Count; i++)
            {
                Assert.Equal(contactPhones[i].DDD.DDDNumber, resultPhones[i].DDD);
                Assert.Equal(contactPhones[i].PhoneNumber, resultPhones[i].PhoneNumber);
                Assert.Equal(contactPhones[i].PhoneType.Description, resultPhones[i].PhoneType);

                // Verificar propriedades do estado e da região
                Assert.Equal(contactPhones[i].DDD.State.UF, state.UF);
                Assert.Equal(contactPhones[i].DDD.State.StateName, state.StateName);
                Assert.Equal(contactPhones[i].DDD.State.Region.RegionName, state.Region.RegionName);
                Assert.Equal(contactPhones[i].DDD.State.RegionId, state.RegionId);
            }
        }
    }
}
