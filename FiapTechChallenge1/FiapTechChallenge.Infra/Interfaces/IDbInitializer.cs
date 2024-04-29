namespace FiapTechChallenge.Infra.Interfaces
{
    public interface IDbInitializer
    {
        void Initialize(IServiceProvider serviceProvider);
    }
}
