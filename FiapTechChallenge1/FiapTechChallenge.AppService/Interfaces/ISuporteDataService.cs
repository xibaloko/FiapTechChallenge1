using FiapTechChallenge.Domain.DTOs.ResponsesDto;

namespace FiapTechChallenge.AppService.Interfaces
{
    public interface ISuporteDataService
    {
        Task<ICollection<DDDResponseDto>?> GetAllDDDs();
        Task<ICollection<PhoneTypeDto>> GetAllPhoneTypes();
        Task<ICollection<RegionResponseDto>?> GetAllRegions();
        Task<ICollection<StateResponseDto>> GetAllStates();
    }
}