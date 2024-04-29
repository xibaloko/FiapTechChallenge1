using FiapTechChallenge.Domain.Entities;
using FiapTechChallenge.Infra.Data;
using FiapTechChallenge.Infra.Interfaces;

namespace FiapTechChallenge.Infra.Repositories
{
    public class RegionRepository : BaseRepository<Region>, IRegionRepository
    {
        public RegionRepository(AppDbContext db) : base(db)
        {
        }
    }
}
