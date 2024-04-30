namespace FiapTechChallenge.Domain.DTOs.ResponsesDto
{
    public class PersonResponseDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string CPF { get; set; }
        public DateTime Birthday { get; set; }
        public string Email { get; set; }
        public ICollection<PhoneResponseDto> Phones { get; set; }
    }
}
