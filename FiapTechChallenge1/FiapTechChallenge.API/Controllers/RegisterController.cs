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

        [HttpGet("contacts")]
        public async Task<IActionResult> GetAllContacts()
        {
            var contacts = await _unitOfWork.Person.GetAllAsync();
            // mudar pra response dto
            return Ok(contacts);
        }

        [HttpGet("contacts/{id}")]
        public async Task<IActionResult> GetContactById(int id)
        {
            var contact = await _unitOfWork.Person.GetAllAsync(x => x.Id == id);
            // mudar pra response dto
            return Ok(contact);
        }
    }
}
