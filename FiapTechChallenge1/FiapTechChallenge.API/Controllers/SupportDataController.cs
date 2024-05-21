using FiapTechChallenge.AppService.Interfaces;
using FiapTechChallenge.Domain.DTOs.ResponsesDto;
using FiapTechChallenge.Infra.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FiapTechChallenge.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SupportDataController : ControllerBase
    {
        public ISuporteDataService _suporteService { get; set; }
        public SupportDataController(ISuporteDataService suporteDataService)
        {
            _suporteService = suporteDataService;
        }

        /// <summary>
        /// get all registered regions
        /// </summary>
        /// <response code="200">returns the list of regions</response>
        /// <response code="404">there is no contact registered</response>
        [HttpGet("all-regions")]
        public async Task<IActionResult> GetAllRegions()
        {
            return Ok(await _suporteService.GetAllRegions());
        }

        /// <summary>
        /// get all registered ddds
        /// </summary>
        /// <response code="200">returns the list of ddds</response>
        /// <response code="404">there is no ddd registered</response>
        [HttpGet("all-ddds")]
        public async Task<IActionResult> GetAllDDDs()
        {
            return Ok(await _suporteService.GetAllDDDs());
        }

        /// <summary>
        /// get all registered states
        /// </summary>
        /// <response code="200">returns the list of states</response>
        /// <response code="404">there is no state registered</response>
        [HttpGet("all-states")]
        public async Task<IActionResult> GetAllStates()
        {
            return Ok(await _suporteService.GetAllStates());
        }

        /// <summary>
        /// get all registered phone types
        /// </summary>
        /// <response code="200">returns the list of phone types</response>
        /// <response code="404">there is no phone type registered</response>
        [HttpGet("all-phone-types")]
        public async Task<IActionResult> GetAllPhoneTypes()
        {
            return Ok(await _suporteService.GetAllPhoneTypes());
        }
    }
}
