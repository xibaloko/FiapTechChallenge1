using FiapTechChallenge.Producer.DTOs;
using MassTransit;
using Microsoft.AspNetCore.Mvc;

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
            var nomeFila = _configuration.GetSection("MassTransit")["NomeFila"]??string.Empty;
            var endpoint = await _bus.GetSendEndpoint(new Uri("queue:fila"));
            await endpoint.Send(personDto);
            //(bool status, string msg, int personID) = await _contactsServices.CreateContact(personDto);
            return Ok();
        }
    }
}