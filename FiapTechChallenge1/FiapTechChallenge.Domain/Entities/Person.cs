namespace FiapTechChallenge.Domain.Entities
{
    public class Person : EntityCore
    {
        public string Name { get; set; }
        public string CPF { get; set; }
        public DateTime Birthday { get; set; }
        public string Email { get; set; }
        public virtual ICollection<Phone>? Phones { get; set; }
    }

}
