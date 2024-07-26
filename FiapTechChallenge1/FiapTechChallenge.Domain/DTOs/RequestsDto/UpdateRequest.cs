using System.ComponentModel.DataAnnotations;

namespace FiapTechChallenge.Domain.DTOs.RequestsDto;

public class UpdateRequest
{
    [Required] public string Name { get; set; }

    [Required] public string CPF { get; set; }

    [Required] public DateTime Birthday { get; set; }

    [Required] [EmailAddress] public string Email { get; set; }

    public ICollection<PhoneRequestByDDDDto>? Phones { get; set; }
}