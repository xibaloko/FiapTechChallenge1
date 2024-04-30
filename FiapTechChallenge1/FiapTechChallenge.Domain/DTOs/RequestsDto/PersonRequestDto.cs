using FiapTechChallenge.Domain.Entities;

namespace FiapTechChallenge.Domain.DTOs.RequestsDto
{
    public class PersonRequestDto
    {
        public string Name { get; set; }
        public string CPF { get; set; }
        public DateTime Birthday { get; set; }
        public string Email { get; set; }
        public ICollection<PhoneRequestDto> Phones { get; set; }
    }
}
