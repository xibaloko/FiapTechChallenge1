using Bogus;
using FiapTechChallenge.Domain.Entities;

namespace FiapTechChallenge.Tests.Helpers
{
    public static class StateFaker
    {
        public static State GenerateState()
        {
            var faker = new Faker("pt_BR");

            var regionFaker = new Faker<Region>()
                .RuleFor(r => r.RegionName, f => f.Address.State())
                .RuleFor(r => r.Id, f => f.Random.Int(1, 10))
                .RuleFor(r => r.Created, f => f.Date.Past())
                .RuleFor(r => r.Modified, f => f.Date.Past());

            var stateFaker = new Faker<State>()
                .RuleFor(s => s.UF, f => f.Address.StateAbbr())
                .RuleFor(s => s.StateName, f => f.Address.State())
                .RuleFor(s => s.Region, f => regionFaker.Generate())
                .RuleFor(s => s.RegionId, (f, s) => s.Region.Id)
                .RuleFor(s => s.Created, f => f.Date.Past())
                .RuleFor(s => s.Modified, f => f.Date.Past());

            return stateFaker.Generate();
        }
    }
}