using FiapTechChallenge.AppService.Interfaces;
using FiapTechChallenge.AppService.Services;
using FiapTechChallenge.Domain.DTOs.RequestsDto;
using FiapTechChallenge.Domain.DTOs.ResponsesDto;
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
            var contacts = await _unitOfWork.Person.GetAllAsync(includeProperties: "Phones,Phones.DDD,Phones.DDD.State,Phones.PhoneType");

            if (contacts != null)
            {
                var response = contacts.Select(x => new PersonResponseDto()
                {
                    Id = x.Id,
                    Name = x.Name,
                    Birthday = x.Birthday,
                    CPF = x.CPF,
                    Email = x.Email,
                    Phones = x.Phones.Select(s => new PhoneResponseDto()
                    {
                        PhoneType = s.PhoneType.Description,
                        DDD = s.DDD.DDDNumber,
                        PhoneNumber = s.PhoneNumber
                    }).ToList()
                }).ToList();

                return Ok(response);
            }

            return NoContent();
        }

        [HttpGet("contact-by-id/{id}")]
        public async Task<IActionResult> GetContactById(int id)
        {
            var contact = await _unitOfWork.Person.FirstOrDefaultAsync(x => x.Id == id, includeProperties: "Phones,Phones.DDD,Phones.DDD.State,Phones.PhoneType");

            if (contact != null)
            {
                var response = new PersonResponseDto()
                {
                    Id = contact.Id,
                    Name = contact.Name,
                    Birthday= contact.Birthday,
                    CPF = contact.CPF,
                    Email= contact.Email,
                    Phones = contact.Phones.Select(x => new PhoneResponseDto()
                    {
                        DDD = x.DDD.DDDNumber,
                        PhoneNumber = x.PhoneNumber,
                        PhoneType = x.PhoneType.Description,
                    }).ToList()
                };

                return Ok(response);
            }

            return NoContent();
        }

        [HttpGet("contacts-by-region-id/{id}")]
        public async Task<IActionResult> GetContactsByRegion(int id)
        {
            var contacts = await _unitOfWork.Person.GetAllAsync(x => x.Phones.Any(y => y.DDD.State.RegionId == id), includeProperties: "Phones,Phones.DDD,Phones.DDD.State,Phones.PhoneType");

            if (contacts != null)
            {
                var response = contacts.Select(x => new PersonResponseDto()
                {
                    Id = x.Id,
                    Name = x.Name,
                    Birthday = x.Birthday,
                    CPF = x.CPF,
                    Email = x.Email,
                    Phones = x.Phones.Select(s => new PhoneResponseDto()
                    {
                        PhoneType = s.PhoneType.Description,
                        DDD = s.DDD.DDDNumber,
                        PhoneNumber = s.PhoneNumber
                    }).ToList()
                }).ToList();

                return Ok(response);
            }

            return NoContent();
        }

        [HttpPost("create-contact")]
        public async Task<IActionResult> CreateContact([FromBody] PersonRequestDto personDto)
        {
            var person = new Person()
            {
                Name = personDto.Name,
                Birthday = personDto.Birthday,
                CPF = personDto.CPF,
                Email = personDto.Email,
                Created = DateTime.Now,
                Modified = DateTime.Now,
                Phones = personDto.Phones.Select(x => new Phone()
                {
                    PhoneNumber = x.PhoneNumber,
                    PhoneTypeId = x.PhoneTypeId,
                    DDDId = x.DDDId,
                }).ToList()
            };

            await _unitOfWork.Person.AddAsync(person);
            _unitOfWork.Save();

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
