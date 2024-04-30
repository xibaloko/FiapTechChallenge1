using FiapTechChallenge.AppService.Interfaces;
using FiapTechChallenge.Domain.Entities;

namespace FiapTechChallenge.AppService.Services
{
    public class PersonApplicationService : ApplicationServiceBase<Person>, IApplicationServiceBase<Person>
    {
    }
}
