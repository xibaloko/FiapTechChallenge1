using FiapTechChallenge.AppService.Interfaces;
using FiapTechChallenge.Domain.DTOs.RequestsDto;
using FiapTechChallenge.Domain.DTOs.ResponsesDto;
using FiapTechChallenge.Domain.Entities;
using FiapTechChallenge.Infra.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FiapTechChallenge.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegisterController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPersonService _contactsServices;
        public RegisterController(IUnitOfWork unitOfWork, IPersonService contactsServices)
        {
            _unitOfWork = unitOfWork;
            _contactsServices = contactsServices;
        }

        /// <summary>
        /// get all registered contacts
        /// </summary>
        /// <response code="200">returns the list of contacts</response>
        /// <response code="404">there is no contact registered</response>
        [HttpGet("all-contacts")]
        public async Task<IActionResult> GetAllContacts()
        {
            ICollection<PersonResponseDto>? res = await _contactsServices.GetAllContactsAsync();
            if (res.Any())
            {
                return Ok(res);
            }
            return NotFound();
        }

        /// <summary>
        /// get contact by an informed id
        /// </summary>
        /// <response code="200">returns a single contact based on the informed id</response>
        /// <response code="404">the contact was not found</response>
        [HttpGet("contact-by-id/{id}")]
        public async Task<IActionResult> GetContactById(int id)
        {
            var contact = await _contactsServices.GetContactById(id);
            if (contact != null)
            {
                return Ok(contact);
            }
            return NotFound();
        }

        /// <summary>
        /// get contact by an informed id region
        /// </summary>
        /// <response code="200">returns a list of contacts based on the region id informed</response>
        /// <response code="404">there is no contact in this region</response>
        [HttpGet("contacts-by-region-id/{id}")]
        public async Task<IActionResult> GetContactsByRegion(int id)
        {
            var contacts = await _contactsServices.GetContactsByRegion(id);
            if (contacts != null)
            {
                return Ok(contacts);
            }
            return NotFound();
        }

        /// <summary>
        /// get contact by an informed ddd number
        /// </summary>
        /// <response code="200">returns a list of contacts based on the ddd number informed</response>
        /// <response code="404">there is no contact with this ddd</response>
        [HttpGet("contacts-by-ddd/{ddd}")]
        public async Task<IActionResult> GetContactsByDDD(int ddd)
        {
            var contacts = await _contactsServices.GetContactsByDDD(ddd);
            if (contacts != null)
            {
                return Ok(contacts);
            }
            return NotFound();
        }

        /// <summary>
        /// fill the required fields to create a new contact, remember to inform 'DDDNumber' and the exact description of the 'PhoneType'
        /// </summary>
        /// <response code="201">returns the route to access the created contact</response>
        /// <response code="400">there are missing fields or fields with errors</response>
        [HttpPost("create-contact-v1")]
        public async Task<IActionResult> CreateContactV1([FromBody] PersonRequestByDDDDto personDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            (bool status, string msg, int personID) = await _contactsServices.CreateContactV1(personDto);
            if (status)
            {
                return CreatedAtAction(nameof(GetContactById), new { id = personID }, null);
            }
            else
            {
                return BadRequest(msg);
            }
        }

        /// <summary>
        /// fill the required fields to create a new contact, remember to inform the DDDId and the PhoneTypeId 
        /// </summary>
        /// <response code="201">returns the route to access the created contact</response>
        /// <response code="400">there are missing fields or fields with errors</response>
        [HttpPost("create-contact-v2")]
        public async Task<IActionResult> CreateContactV2([FromBody] PersonRequestByIdDto personDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            (bool status, string msg, int personID) = await _contactsServices.CreateContactV2(personDto);
            if (status)
            {
                return CreatedAtAction(nameof(GetContactById), new { id = personID }, null);
            }
            return BadRequest(msg);
        }

        /// <summary>
        /// inform an id and fill the fields you want to modify to update a contact, remember to inform 'DDDNumber' and the exact description of the 'PhoneType'
        /// </summary>
        /// <response code="200">returns the modified contact</response>
        /// <response code="400">there are missing fields or fields with errors</response>
        [HttpPut("update-contact-v1/{id}")]
        public async Task<IActionResult> UpdateContactV1(int id, [FromBody] PersonRequestByDDDDto personDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            (bool status, string msg, PersonResponseDto? person) = await _contactsServices.UpdateContactV1(id, personDto);
            if (status)
            {
                return Ok(person);
            }
            return BadRequest(msg);
        }

        /// <summary>
        /// inform an id and fill the fields you want to modify to update a contact, remember to inform the DDDId and the PhoneTypeId 
        /// </summary>
        /// <response code="200">returns the modified contact</response>
        /// <response code="400">there are missing fields or fields with errors</response>
        [HttpPut("update-contact-v2/{id}")]
        public async Task<IActionResult> UpdateContactV2(int id, [FromBody] PersonRequestByIdDto personDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            (bool status, string msg, PersonResponseDto? person) = await _contactsServices.UpdateContactV2(id, personDto);
            if (status)
            {
                return Ok(person);
            }
            return BadRequest(msg);
        }

        /// <summary>
        /// inform an id to delete a contact
        /// </summary>
        /// <response code="200">returns a successful message</response>
        /// <response code="404">the contact was not found.</response>
        [HttpDelete("delete-contact/{id}")]
        public async Task<IActionResult> DeleteContact(int id)
        {
            (bool status, string msg) = await _contactsServices.DeleteContact(id);
            if (status)
            {
                return Ok(new
                {
                    Message = "This person was successfully deleted."
                });

            }
            return BadRequest(msg);
        }
    }
}
