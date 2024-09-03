using FiapTechChallenge.Domain.DTOs.RequestsDto;
using FiapTechChallenge.Domain.DTOs.ResponsesDto;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;

namespace FiapTechChallenge.Producer.Controllers
{
    public class ContactProducerController : Controller
    {
        private readonly IBus _bus;
        private readonly IConfiguration _configuration;
        public ContactProducerController(IBus bus, IConfiguration configuration)
        {
            _bus = bus;
            _configuration = configuration;
        }

        /// <summary>
        /// fill the required fields to create a new contact, remember to inform 'DDDNumber' and the exact description of the 'PhoneType'
        /// </summary>
        /// <response code="201">returns the route to access the created contact</response>
        /// <response code="400">there are missing fields or fields with errors</response>
        [HttpPost("create-contact")]
        public async Task<IActionResult> CreateContact([FromBody] PersonRequestByDDDDto personDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var nomeFila = _configuration.GetSection("MassTransit")["NomeFila"] ?? string.Empty;
            var endpoint = await _bus.GetSendEndpoint(new Uri($"queue:{nomeFila}"));          
            await endpoint.Send(personDto);
            return Ok();
        }
       
        /// <summary>
        /// inform an id and fill the fields you want to modify to update a contact, remember to inform 'DDDNumber' and the exact description of the 'PhoneType'
        /// </summary>
        /// <response code="200">returns the modified contact</response>
        /// <response code="400">there are missing fields or fields with errors</response>
        [HttpPut("update-contact")]
        public async Task<IActionResult> UpdateContact([FromBody] UpdateRequest personDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var nomeFila = _configuration.GetSection("MassTransit")["NomeFila"] ?? string.Empty;
            var endpoint = await _bus.GetSendEndpoint(new Uri($"queue:{nomeFila}"));
            await endpoint.Send(personDto);
            return Ok();           
        }

        /// <summary>
        /// inform an id to delete a contact
        /// </summary>
        /// <response code="200">returns a successful message</response>
        /// <response code="404">the contact was not found.</response>
        [HttpDelete("delete-contact")]
        public async Task<IActionResult> DeleteContact([FromBody]DeletePersonRequest personDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var nomeFila = _configuration.GetSection("MassTransit")["NomeFila"] ?? string.Empty;
            var endpoint = await _bus.GetSendEndpoint(new Uri($"queue:{nomeFila}"));
            await endpoint.Send(personDto);
            return Ok();
        }
    }
}