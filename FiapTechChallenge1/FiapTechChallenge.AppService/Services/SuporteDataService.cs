using FiapTechChallenge.AppService.Interfaces;
using FiapTechChallenge.Domain.DTOs.ResponsesDto;
using FiapTechChallenge.Infra.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FiapTechChallenge.AppService.Services
{
    public class SuporteDataService : ISuporteDataService
    {
        private readonly IUnitOfWork _unitOfWork;

        public SuporteDataService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ICollection<RegionResponseDto>?> GetAllRegions()
        {
            var regions = await _unitOfWork.Region.GetAllAsync();

            var response = regions.Select(x => new RegionResponseDto()
            {
                RegionId = x.Id,
                RegionName = x.RegionName
            }).ToList();

            return response;
        }

        public async Task<ICollection<DDDResponseDto>?> GetAllDDDs()
        {
            var ddds = await _unitOfWork.DDD.GetAllAsync();
            var response = ddds.Select(x => new DDDResponseDto()
            {
                DDDId = x.Id,
                DDDNumber = x.DDDNumber
            }).ToList();
            return response;
        }

        public async Task<ICollection<StateResponseDto>> GetAllStates()
        {
            var states = await _unitOfWork.State.GetAllAsync();
            var response = states.Select(x => new StateResponseDto()
            {
                StateId = x.Id,
                StateName = x.StateName
            }).ToList();
            return response;
        }

        public async Task<ICollection<PhoneTypeDto>> GetAllPhoneTypes()
        {
            var phoneTypes = await _unitOfWork.PhoneType.GetAllAsync();
            var response = phoneTypes.Select(x => new PhoneTypeDto()
            {
                PhoneTypeId = x.Id,
                PhoneType = x.Description
            }).ToList();
            return response;
        }
    }
}
