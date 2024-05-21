using Bogus;
using Bogus.Extensions.Brazil;
using FiapTechChallenge.Domain.DTOs.RequestsDto;

namespace FiapTechChallenge.Tests.Helpers
{
    public static class PersonRequestDtoFaker
    {
        public static PersonRequestByDDDDto GeneratePersonRequest()
        {
            var faker = new Faker("pt_BR");

            var phoneFaker = new Faker<PhoneRequestByDDDDto>()
                .RuleFor(p => p.PhoneNumber, f => f.Phone.PhoneNumber("##########"))
                .RuleFor(p => p.DDDNumber, f => f.Random.Int(1, 99))
                .RuleFor(p => p.PhoneType, f => f.PickRandom("Residencial", "Comercial", "Celular"));

            var personRequestDto = new Faker<PersonRequestByDDDDto>()
                .RuleFor(p => p.Name, f => f.Name.FullName())
                .RuleFor(p => p.Birthday, f => f.Date.Past(30, DateTime.Now.AddYears(-18)))
                .RuleFor(p => p.CPF, f => f.Person.Cpf(false))
                .RuleFor(p => p.Email, f => f.Internet.Email())
                .RuleFor(p => p.Phones, f => phoneFaker.Generate(2));

            return personRequestDto.Generate();
        }
    }
}
