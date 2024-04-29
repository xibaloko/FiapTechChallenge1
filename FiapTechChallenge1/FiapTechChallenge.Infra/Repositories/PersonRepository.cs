using FiapTechChallenge.Domain.Entities;
using FiapTechChallenge.Infra.Data;
using FiapTechChallenge.Infra.Interfaces;

namespace FiapTechChallenge.Infra.Repositories
{
    public class PersonRepository : BaseRepository<Person>, IPersonRepository
    {
        public PersonRepository(AppDbContext db) : base(db)
        {
        }
    }
}
