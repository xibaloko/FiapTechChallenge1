using FiapTechChallenge.Domain.Entities;
using FiapTechChallenge.Infra.Data;
using FiapTechChallenge.Infra.Interfaces;

namespace FiapTechChallenge.Infra.Repositories
{
    public class PhoneTypeRepository : BaseRepository<PhoneType>, IPhoneTypeRepository
    {
        public PhoneTypeRepository(AppDbContext db) : base(db)
        {
        }
    }
}
