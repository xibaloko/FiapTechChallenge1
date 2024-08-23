using System.ComponentModel.DataAnnotations;

namespace FiapTechChallenge.Producer.DTOs
{
    public class PhoneRequestByDDDDto
    {
        [Required]
        [StringLength(20)]
        [RegularExpression(@"^\d+$", ErrorMessage = "The phone number must contain only digits.")]
        public required string PhoneNumber { get; set; }
        [Required]
        public int DDDNumber { get; set; }
        [Required]
        [AllowedValues("Residencial", "Comercial", "Celular", ErrorMessage = "Only values 'Residencial', 'Comercial' and 'Celular' are allowed")]
        public required string PhoneType { get; set; }
    }
}
