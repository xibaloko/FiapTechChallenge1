using FiapTechChallenge.Domain.DTOs.RequestsDto;
using FiapTechChallenge.Domain.DTOs.ResponsesDto;

namespace FiapTechChallenge.AppService.Interfaces
{
    public interface IPersonService
    {
        Task<(bool, string, int)> CreateContactV1(PersonRequestByDDDDto personDto);
        Task<(bool, string, int)> CreateContactV2(PersonRequestByIdDto personDto);
        Task<(bool, string)> DeleteContact(int id);
        Task<ICollection<PersonResponseDto>>? GetAllContactsAsync();
        Task<PersonResponseDto?> GetContactById(int id);
        Task<ICollection<PersonResponseDto>?> GetContactsByDDD(int ddd);
        Task<ICollection<PersonResponseDto>?> GetContactsByRegion(int regionId);
        Task<(bool, string, PersonResponseDto?)> UpdateContactV1(int id, PersonRequestByDDDDto personDto);
        Task<(bool, string, PersonResponseDto)> UpdateContactV2(int id, PersonRequestByIdDto personDto);
    }
}