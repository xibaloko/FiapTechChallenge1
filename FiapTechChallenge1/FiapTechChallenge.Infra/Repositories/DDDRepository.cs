using FiapTechChallenge.Domain.Entities;
using FiapTechChallenge.Infra.Data;
using FiapTechChallenge.Infra.Interfaces;

namespace FiapTechChallenge.Infra.Repositories
{
    public class DDDRepository : BaseRepository<DDD>, IDDDRepository
    {
        public DDDRepository(AppDbContext db) : base(db)
        {
        }
    }
}
