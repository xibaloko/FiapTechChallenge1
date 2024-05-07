namespace FiapTechChallenge.Domain.DTOs.ResponsesDto
{
    public class PhoneResponseDto
    {
        public int DDD { get; set; }
        public required string PhoneNumber { get; set; }
        public required string PhoneType { get; set; }
    }
}
