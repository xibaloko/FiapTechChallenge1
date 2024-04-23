using System.ComponentModel.DataAnnotations.Schema;

namespace FiapTechChallenge.Domain.Entities
{
    public class DDD : EntityCore
    {
        public int DDDNumber { get; set; }
        [ForeignKey("StateId")]
        public virtual State State { get; set; }
        public ICollection<Phone> Phones { get; set; }
        public int StateId { get; set; }
    }

}
