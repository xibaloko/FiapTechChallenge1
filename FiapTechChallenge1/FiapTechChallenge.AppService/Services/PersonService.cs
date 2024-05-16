using FiapTechChallenge.AppService.Interfaces;
using FiapTechChallenge.Domain.DTOs.RequestsDto;
using FiapTechChallenge.Domain.DTOs.ResponsesDto;
using FiapTechChallenge.Domain.Entities;
using FiapTechChallenge.Infra.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FiapTechChallenge.AppService.Services
{
    public class PersonService : IPersonService
    {
        private readonly IUnitOfWork _unitOfWork;

        public PersonService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ICollection<PersonResponseDto>>? GetAllContactsAsync()
        {
            var contacts = await _unitOfWork.Person.GetAllAsync(includeProperties: "Phones,Phones.DDD,Phones.DDD.State,Phones.PhoneType");

            var lst = contacts.Select(x => new PersonResponseDto()
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
            return lst;
        }

        public async Task<PersonResponseDto?> GetContactById(int id)
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
                return response;
            }
            return null;
        }

        public async Task<ICollection<PersonResponseDto>?> GetContactsByRegion(int regionId)
        {
            var contacts = await _unitOfWork.Person.GetAllAsync(x => x.Phones.Any(y => y.DDD.State.RegionId == regionId), includeProperties: "Phones,Phones.DDD,Phones.DDD.State,Phones.PhoneType");

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

                return response;
            }

            return null;
        }

        public async Task<ICollection<PersonResponseDto>?> GetContactsByDDD(int ddd)
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

                return response;
            }

            return null;
        }

        public async Task<(bool, string, int)> CreateContactV1(PersonRequestByDDDDto personDto)
        {
            var ddds = await _unitOfWork.DDD.GetAllAsync();
            foreach (var item in personDto.Phones)
            {
                if (!ddds.Any(x => x.DDDNumber == item.DDDNumber))
                {
                    return (false, $"Invalid DDD Number: '{item.DDDNumber}'", -1);
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

            return (true, string.Empty, person.Id);
        }

        public async Task<(bool, string, int)> CreateContactV2(PersonRequestByIdDto personDto)
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

            return (true, string.Empty, person.Id);
        }

        public async Task<(bool, string, PersonResponseDto?)> UpdateContactV1(int id, PersonRequestByDDDDto personDto)
        {
            var ddds = await _unitOfWork.DDD.GetAllAsync();

            foreach (var item in personDto.Phones)
            {
                if (!ddds.Any(x => x.DDDNumber == item.DDDNumber))
                {
                    return (false, $"Invalid DDD Number: '{item.DDDNumber}'", null);
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

            return (true, string.Empty, response);
        }

        public async Task<(bool, string, PersonResponseDto)> UpdateContactV2(int id, PersonRequestByIdDto personDto)
        {
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

            return (true, string.Empty, response);
        }


        public async Task<(bool, string)> DeleteContact(int id)
        {
            var person = await _unitOfWork.Person.FirstOrDefaultAsync(x => x.Id == id);

            if (person == null)
            {
                return (false, "This person was not found.");
            }

            _unitOfWork.Person.Remove(person);
            var ret = _unitOfWork.Person.SaveCount();

            return (true, "This person was successfully deleted.");
        }

    }
}
