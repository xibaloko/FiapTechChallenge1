using System.ComponentModel.DataAnnotations;

namespace FiapTechChallenge.Domain.Entities
{
    public abstract class EntityCore
    {
        [Key]
        public int Id { get; set; }
        public DateTime Created { get; set; }
        public DateTime? Modified { get; set; }
    }
}
