using FiapTechChallenge.Domain.Entities;
using System.ComponentModel.DataAnnotations;

namespace FiapTechChallenge.Domain.DTOs.RequestsDto
{
    public class PersonRequestByIdDto
    {
        [Required]
        public required string Name { get; set; }
        [Required]
        public required string CPF { get; set; }
        [Required]
        public DateTime Birthday { get; set; }
        [Required]
        [EmailAddress]
        public required string Email { get; set; }
        public required ICollection<PhoneRequestByIdDto> Phones { get; set; }
    }
}
