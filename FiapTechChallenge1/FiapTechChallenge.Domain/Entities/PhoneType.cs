namespace FiapTechChallenge.Domain.Entities
{
    public class PhoneType : EntityCore
    {
        public string Description { get; set; }
        public ICollection<Phone> Phones { get; set; }
    }

}
