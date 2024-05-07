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

        public RegisterController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// get all registered contacts
        /// </summary>
        /// <response code="200">returns the list of contacts</response>
        /// <response code="404">there is no contact registered</response>
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
            var contact = await _unitOfWork.Person.FirstOrDefaultAsync(x => x.Id == id, includeProperties: "Phones,Phones.DDD,Phones.DDD.State,Phones.PhoneType");

            if (contact != null)
            {
                var response = new PersonResponseDto()
                {
                    Id = contact.Id,
                    Name = contact.Name,
                    Birthday = contact.Birthday,
                    CPF = contact.CPF,
                    Email = contact.Email,
                    Phones = contact.Phones.Select(x => new PhoneResponseDto()
                    {
                        DDD = x.DDD.DDDNumber,
                        PhoneNumber = x.PhoneNumber,
                        PhoneType = x.PhoneType.Description,
                    }).ToList()
                };

                return Ok(response);
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
            var contacts = await _unitOfWork.Person.GetAllAsync(x => x.Phones.Any(y => y.DDD.DDDNumber == ddd), includeProperties: "Phones,Phones.DDD,Phones.DDD.State,Phones.PhoneType");

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

            var ddds = await _unitOfWork.DDD.GetAllAsync();

            foreach (var item in personDto.Phones)
            {
                if (!ddds.Any(x => x.DDDNumber == item.DDDNumber))
                {
                    ModelState.AddModelError("DDD Number", $"Invalid DDD Number: '{item.DDDNumber}'");
                    return BadRequest(ModelState);
                }
            }

            var phoneTypes = await _unitOfWork.PhoneType.GetAllAsync();
            
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
                    PhoneTypeId = phoneTypes.FirstOrDefault(p => p.Description.ToUpper() == x.PhoneType.ToUpper()).Id,
                    DDDId = ddds.FirstOrDefault(d => d.DDDNumber == x.DDDNumber).Id,
                }).ToList()
            };

            await _unitOfWork.Person.AddAsync(person);
            _unitOfWork.Save();

            return CreatedAtAction(nameof(GetContactById), new { id = person.Id }, null);
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

            return CreatedAtAction(nameof(GetContactById), new { id = person.Id }, null);
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

            var ddds = await _unitOfWork.DDD.GetAllAsync();

            foreach (var item in personDto.Phones)
            {
                if (!ddds.Any(x => x.DDDNumber == item.DDDNumber))
                {
                    ModelState.AddModelError("DDD Number", $"Invalid DDD Number: '{item.DDDNumber}'");
                    return BadRequest(ModelState);
                }
            }

            var phoneTypes = await _unitOfWork.PhoneType.GetAllAsync();

            var person = await _unitOfWork.Person.FirstOrDefaultAsync(x => x.Id == id, includeProperties: "Phones,Phones.DDD,Phones.DDD.State,Phones.PhoneType");

            person.Name = personDto.Name;
            person.Birthday = personDto.Birthday;
            person.CPF = personDto.CPF;
            person.Email = personDto.Email;
            person.Modified = DateTime.Now;

            if (personDto.Phones.Count > 0)
            {
                person.Phones.Clear();

                person.Phones = personDto.Phones.Select(x => new Phone()
                {
                    PhoneNumber = x.PhoneNumber,
                    PhoneTypeId = phoneTypes.FirstOrDefault(p => p.Description == x.PhoneType).Id,
                    DDDId = ddds.FirstOrDefault(d => d.DDDNumber == x.DDDNumber).Id,
                }).ToList();
            }

            _unitOfWork.Person.Update(person);
            _unitOfWork.Save();

            var response = new PersonResponseDto()
            {
                Id = person.Id,
                Name = person.Name,
                Birthday = person.Birthday,
                CPF = person.CPF,
                Email = person.Email,
                Phones = person.Phones.Select(x => new PhoneResponseDto()
                {
                    DDD = x.DDD.DDDNumber,
                    PhoneNumber = x.PhoneNumber,
                    PhoneType = phoneTypes.FirstOrDefault(p => p.Id == x.PhoneTypeId).Description,
                }).ToList()
            };

            return Ok(response);
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

            var person = await _unitOfWork.Person.FirstOrDefaultAsync(x => x.Id == id, includeProperties: "Phones,Phones.DDD,Phones.DDD.State,Phones.PhoneType");

            person.Name = personDto.Name;
            person.Birthday = personDto.Birthday;
            person.CPF = personDto.CPF;
            person.Email = personDto.Email;
            person.Modified = DateTime.Now;

            if (personDto.Phones.Count > 0)
            {
                person.Phones.Clear();

                person.Phones = personDto.Phones.Select(x => new Phone()
                {
                    PhoneNumber = x.PhoneNumber,
                    PhoneTypeId = x.PhoneTypeId,
                    DDDId = x.DDDId,
                }).ToList();
            }

            _unitOfWork.Person.Update(person);
            _unitOfWork.Save();

            var phoneTypes = await _unitOfWork.PhoneType.GetAllAsync();

            var response = new PersonResponseDto()
            {
                Id = person.Id,
                Name = person.Name,
                Birthday = person.Birthday,
                CPF = person.CPF,
                Email = person.Email,
                Phones = person.Phones.Select(x => new PhoneResponseDto()
                {
                    DDD = x.DDD.DDDNumber,
                    PhoneNumber = x.PhoneNumber,
                    PhoneType = phoneTypes.FirstOrDefault(p => p.Id == x.PhoneTypeId).Description,
                }).ToList()
            };

            return Ok(response);
        }

        /// <summary>
        /// inform an id to delete a contact
        /// </summary>
        /// <response code="200">returns a successful message</response>
        /// <response code="404">the contact was not found.</response>
        [HttpDelete("delete-contact/{id}")]
        public async Task<IActionResult> DeleteContact(int id)
        {
            var person = await _unitOfWork.Person.FirstOrDefaultAsync(x => x.Id == id);

            if (person == null)
            {
                return NotFound(new
                {
                    Message = "This person was not found."
                });
            }

            _unitOfWork.Person.Remove(person);
            var ret = _unitOfWork.Person.SaveCount();

            return Ok(new
            {
                Message = "This person was successfully deleted."
            });
        }
    }
}
