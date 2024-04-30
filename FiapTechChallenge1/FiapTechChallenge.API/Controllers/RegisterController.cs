using FiapTechChallenge.Domain.Entities;
using FiapTechChallenge.Infra.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FiapTechChallenge.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegisterController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public RegisterController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet("all-contacts")]
        public async Task<IActionResult> GetAllContacts()
        {
            var contacts = await _unitOfWork.Person.GetAllAsync();
            // mudar pra response dto
            return Ok(contacts);
        }

        [HttpGet("contact-by-id/{id}")]
        public async Task<IActionResult> GetContactById(int id)
        {
            var contact = await _unitOfWork.Person.GetAllAsync(x => x.Id == id);
            // mudar pra response dto
            return Ok(contact);
        }

        [HttpGet("contact-by-region-id/{id}")]
        public async Task<IActionResult> GetContactsByRegion(int id)
        {
            var contact = await _unitOfWork.Person.GetAllAsync(x => x.Id == id);
            // mudar pra response dto
            return Ok(contact);
        }

        [HttpPost("create-contact")]
        public async Task<IActionResult> CreateContact([FromBody] Person person)
        {

            return Ok();
        }

        [HttpPut("update-contact/{id}")]
        public async Task<IActionResult> UpdateContact(int id, [FromBody] Person person)
        {
           
            return Ok();
        }

        [HttpDelete("delete-contact/{id}")]
        public async Task<IActionResult> DeleteContact(int id)
        {
           
            return Ok();
        }
    }
}
