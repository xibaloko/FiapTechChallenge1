using Bogus;
using FiapTechChallenge.Domain.DTOs.RequestsDto;

namespace FiapTechChallenge.Tests.Helpers
{
    public static class PhoneRequestDtoFaker
    {
        public static PhoneRequestByDDDDto GeneratePhoneRequest()
        {
            return new Faker<PhoneRequestByDDDDto>("pt_BR")
                .RuleFor(p => p.PhoneNumber, f => f.Phone.PhoneNumber("##########"))
                .RuleFor(p => p.DDDNumber, f => f.Random.Int(1, 99))
                .RuleFor(p => p.PhoneType, f => f.PickRandom("Residencial", "Comercial", "Celular"))
                .Generate();
        }
    }
}