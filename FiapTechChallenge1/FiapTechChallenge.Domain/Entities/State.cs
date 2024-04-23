using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FiapTechChallenge.Domain.Entities
{
    public class State : EntityCore
    {
        public string UF { get; set; }
        public string StateName { get; set; }
        [ForeignKey("RegionId")]
        public virtual Region Region { get; set; }
        [Required]
        public int RegionId { get; set; }
        public ICollection<DDD> DDDs { get; set; }
    }
}