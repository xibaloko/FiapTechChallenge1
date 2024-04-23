using System.ComponentModel.DataAnnotations.Schema;

namespace FiapTechChallenge.Domain.Entities
{
    public class Phone : EntityCore
    {
        public string PhoneNumber { get; set; }
        [ForeignKey("DDDId")]
        public virtual DDD DDD { get; set; }
        [ForeignKey("PhoneTypeId")]
        public virtual PhoneType PhoneType { get; set; }
        [ForeignKey("PersonId")]
        public virtual Person Person { get; set; }
        public int PhoneTypeId { get; set; }
        public int DDDId { get; set; }
        public int PersonId { get; set; }
    }

}
