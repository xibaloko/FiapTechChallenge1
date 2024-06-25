using Bogus;
using FiapTechChallenge.Domain.Entities;
using Person = FiapTechChallenge.Domain.Entities.Person;

namespace FiapTechChallenge.Tests.Helpers;

public class TestDataFactory
{
    public static List<Person> GeneratePersons(int count)
    {
        var regionFaker = new Faker<Region>()
            .RuleFor(r => r.Id, f => f.Random.Int(1, 10))
            .RuleFor(r => r.RegionName, f => f.Address.State());

        var stateFaker = new Faker<State>()
            .RuleFor(s => s.Id, f => f.Random.Int(1, 10))
            .RuleFor(s => s.UF, f => f.Address.StateAbbr())
            .RuleFor(s => s.StateName, f => f.Address.State())
            .RuleFor(s => s.Region, f => regionFaker.Generate())
            .RuleFor(s => s.RegionId, (f, s) => s.Region.Id);

        var dddFaker = new Faker<DDD>()
            .RuleFor(d => d.Id, f => f.Random.Int(1, 10))
            .RuleFor(d => d.DDDNumber, f => f.Random.Int(10, 99))
            .RuleFor(d => d.State, f => stateFaker.Generate())
            .RuleFor(d => d.StateId, (f, d) => d.State.Id);

        var phoneTypeFaker = new Faker<PhoneType>()
            .RuleFor(pt => pt.Id, f => f.Random.Int(1, 10))
            .RuleFor(pt => pt.Description, f => f.PickRandom("Mobile", "Home", "Work"));

        var phoneFaker = new Faker<Phone>()
            .RuleFor(p => p.Id, f => f.Random.Int(1, 1000))
            .RuleFor(p => p.PhoneNumber, f => f.Phone.PhoneNumber("#########"))
            .RuleFor(p => p.DDD, f => dddFaker.Generate())
            .RuleFor(p => p.DDDId, (f, p) => p.DDD.Id)
            .RuleFor(p => p.PhoneType, f => phoneTypeFaker.Generate())
            .RuleFor(p => p.PhoneTypeId, (f, p) => p.PhoneType.Id)
            .RuleFor(p => p.PersonId, f => f.Random.Int(1, 1000));

        var personFaker = new Faker<Person>()
            .RuleFor(p => p.Id, f => f.Random.Int(1, 1000))
            .RuleFor(p => p.Name, f => "Raphael Muller")
            .RuleFor(p => p.CPF, f => f.Random.Replace("###########"))
            .RuleFor(p => p.Birthday, f => f.Date.Past(30, DateTime.Now.AddYears(-20)))
            .RuleFor(p => p.Email, f => f.Internet.Email())
            .RuleFor(p => p.Phones, f => phoneFaker.Generate(f.Random.Int(1, 3)));

        return personFaker.Generate(count);
    }
}