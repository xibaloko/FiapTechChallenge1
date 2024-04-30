namespace FiapTechChallenge.Infra.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IDDDRepository DDD { get; }
        IPersonRepository Person { get; }
        IPhoneRepository Phone { get; }
        IPhoneTypeRepository PhoneType { get; }
        IRegionRepository Region { get; }
        IStateRepository State { get; }
        void Save();
    }
}
