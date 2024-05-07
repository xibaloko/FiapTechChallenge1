using FiapTechChallenge.Domain.DTOs.ResponsesDto;
using FiapTechChallenge.Infra.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FiapTechChallenge.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SupportDataController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public SupportDataController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// get all registered regions
        /// </summary>
        /// <response code="200">returns the list of regions</response>
        /// <response code="404">there is no contact registered</response>
        [HttpGet("all-regions")]
        public async Task<IActionResult> GetAllRegions()
        {
            var regions = await _unitOfWork.Region.GetAllAsync();

            if (regions != null)
            {
                var response = regions.Select(x => new RegionResponseDto()
                {
                    RegionId = x.Id,
                    RegionName = x.RegionName
                });

                return Ok(response);
            }

            return NoContent();
        }

        /// <summary>
        /// get all registered ddds
        /// </summary>
        /// <response code="200">returns the list of ddds</response>
        /// <response code="404">there is no ddd registered</response>
        [HttpGet("all-ddds")]
        public async Task<IActionResult> GetAllDDDs()
        {
            var ddds = await _unitOfWork.DDD.GetAllAsync();

            if (ddds != null)
            {
                var response = ddds.Select(x => new DDDResponseDto()
                {
                    DDDId = x.Id,
                    DDDNumber = x.DDDNumber
                });

                return Ok(response);
            }

            return NoContent();
        }

        /// <summary>
        /// get all registered states
        /// </summary>
        /// <response code="200">returns the list of states</response>
        /// <response code="404">there is no state registered</response>
        [HttpGet("all-states")]
        public async Task<IActionResult> GetAllStates()
        {
            var states = await _unitOfWork.State.GetAllAsync();

            if (states != null)
            {
                var response = states.Select(x => new StateResponseDto()
                {
                    StateId = x.Id,
                    StateName = x.StateName
                });

                return Ok(response);
            }

            return NoContent();
        }

        /// <summary>
        /// get all registered phone types
        /// </summary>
        /// <response code="200">returns the list of phone types</response>
        /// <response code="404">there is no phone type registered</response>
        [HttpGet("all-phone-types")]
        public async Task<IActionResult> GetAllPhoneTypes()
        {
            var phoneTypes = await _unitOfWork.PhoneType.GetAllAsync();

            if (phoneTypes != null)
            {
                var response = phoneTypes.Select(x => new PhoneTypeDto()
                {
                    PhoneTypeId = x.Id,
                    PhoneType = x.Description
                });

                return Ok(response);
            }

            return NoContent();
        }
    }
}
