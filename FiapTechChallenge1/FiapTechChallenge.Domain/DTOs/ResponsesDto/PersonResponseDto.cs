namespace FiapTechChallenge.Domain.DTOs.ResponsesDto
{
    public class PersonResponseDto
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string CPF { get; set; }
        public DateTime Birthday { get; set; }
        public required string Email { get; set; }
        public required ICollection<PhoneResponseDto> Phones { get; set; }
    }
}
