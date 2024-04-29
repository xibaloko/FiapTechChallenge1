using FiapTechChallenge.Domain.Entities;
using FiapTechChallenge.Infra.Data;
using FiapTechChallenge.Infra.Interfaces;

namespace FiapTechChallenge.Infra.Repositories
{
    public class PhoneRepository : BaseRepository<Phone>, IPhoneRepository
    {
        public PhoneRepository(AppDbContext db) : base(db)
        {
        }
    }
}
