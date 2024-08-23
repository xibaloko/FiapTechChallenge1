using System.ComponentModel.DataAnnotations;

namespace FiapTechChallenge.Producer.DTOs
{
    public class PersonRequestByDDDDto
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
        public required ICollection<PhoneRequestByDDDDto> Phones { get; set; }
    }
}
