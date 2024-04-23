namespace FiapTechChallenge.Domain.Entities
{
    public class Region : EntityCore
    {
        public string RegionName { get; set; }
        public ICollection<State> States { get; set; }
    }
}
