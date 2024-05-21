using FiapTechChallenge.Domain.DTOs.RequestsDto;
using FiapTechChallenge.Domain.DTOs.ResponsesDto;

namespace FiapTechChallenge.AppService.Interfaces
{
    public interface IPersonService
    {
        Task<(bool, string, int)> CreateContact(PersonRequestByDDDDto personDto);
        Task<(bool, string)> DeleteContact(int id);
        Task<ICollection<PersonResponseDto>>? GetAllContactsAsync();
        Task<PersonResponseDto?> GetContactById(int id);
        Task<ICollection<PersonResponseDto>?> GetContactsByDDD(int ddd);
        Task<ICollection<PersonResponseDto>?> GetContactsByRegion(int regionId);
        Task<(bool, string, PersonResponseDto?)> UpdateContact(int id, PersonRequestByDDDDto personDto);
        
    }
}