using FiapTechChallenge.Domain.DTOs.ResponsesDto;

namespace FiapTechChallenge.AppService.Interfaces
{
    public interface IPersonService
    {
        Task<ICollection<PersonResponseDto>>? GetAllContactsAsync();
    }
}