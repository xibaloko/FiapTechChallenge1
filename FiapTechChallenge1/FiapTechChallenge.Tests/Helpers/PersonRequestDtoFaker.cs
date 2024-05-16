using Bogus;
using Bogus.Extensions.Brazil;
using FiapTechChallenge.Domain.DTOs.RequestsDto;
using System;
using System.Collections.Generic;

namespace FiapTechChallenge.Tests.Helpers
{
    public static class PersonRequestDtoFaker
    {
        public static PersonRequestByIdDto GeneratePersonRequest()
        {
            var faker = new Faker("pt_BR");

            var phoneFaker = new Faker<PhoneRequestByIdDto>()
                .RuleFor(p => p.PhoneNumber, f => f.Phone.PhoneNumber("#########"))
                .RuleFor(p => p.PhoneTypeId, f => f.Random.Int(1, 3))
                .RuleFor(p => p.DDDId, f => f.Random.Int(1, 99));

            var personRequestDto = new Faker<PersonRequestByIdDto>()
                .RuleFor(p => p.Name, f => f.Name.FullName())
                .RuleFor(p => p.Birthday, f => f.Date.Past(30, DateTime.Now.AddYears(-18)))
                .RuleFor(p => p.CPF, f => f.Person.Cpf(false))
                .RuleFor(p => p.Email, f => f.Internet.Email())
                .RuleFor(p => p.Phones, f => phoneFaker.Generate(2));

            return personRequestDto.Generate();
        }
    }
}
