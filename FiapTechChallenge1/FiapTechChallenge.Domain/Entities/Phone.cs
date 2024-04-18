namespace FiapTechChallenge.Domain.Entities
{
    public class Phone : EntityCore
    {
        public DDD DDD { get; set; }
        public string PhoneNumber { get; set; }
        public PhoneType PhoneType { get; set; }
    }

}
