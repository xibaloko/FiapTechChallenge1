using FiapTechChallenge.Infra.Data;
using FiapTechChallenge.Infra.Interfaces;

namespace FiapTechChallenge.Infra.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _db;
        public IDDDRepository DDD { get; private set; }
        public IPersonRepository Person { get; private set; }
        public IPhoneRepository Phone { get; private set; }
        public IPhoneTypeRepository PhoneType { get; private set; }
        public IRegionRepository Region { get; private set; }
        public IStateRepository State { get; private set; }

        public UnitOfWork(AppDbContext db)
        {
            _db = db;
            DDD = new DDDRepository(_db);
            Person = new PersonRepository(_db);
            Phone = new PhoneRepository(_db);
            PhoneType = new PhoneTypeRepository(_db);
            Region = new RegionRepository(_db);
            State = new StateRepository(_db);
        }

        public void Dispose()
        {
            _db.Dispose();
        }

        public void Save()
        {
            _db.SaveChanges();
        }
    }
}
