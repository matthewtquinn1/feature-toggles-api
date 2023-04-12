namespace FeatureToggle.Domain.Entities;

public sealed class Feature
{
    public int DbId { get; set; }
    public Guid Id { get; set; }
    public string Name { get; set; }

    public ICollection<FeatureState> FeatureStates { get; set; } = new HashSet<FeatureState>();

    public int ProductDbId { get; set; }
    public Product Product { get; set; }
}
