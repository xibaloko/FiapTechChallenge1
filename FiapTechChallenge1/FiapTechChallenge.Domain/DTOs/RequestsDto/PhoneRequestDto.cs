using System.ComponentModel.DataAnnotations;

namespace FiapTechChallenge.Domain.DTOs.RequestsDto
{
    public class PhoneRequestDto
    {
        [Required]
        [StringLength(20)]
        [RegularExpression(@"^\d+$", ErrorMessage = "The phone number must contain only digits.")]
        public required string PhoneNumber { get; set; }
        [Required]
        public int DDDId { get; set; }
        [Required]
        public int PhoneTypeId { get; set; }
    }
}
