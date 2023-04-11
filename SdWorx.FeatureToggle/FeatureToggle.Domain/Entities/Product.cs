namespace FeatureToggle.Domain.Entities;

public sealed class Product
{
    public int DbId { get; set; }
    public Guid Id { get; set; }
    public string Name { get; set; }

    public ICollection<Feature> Features { get; set; } = new HashSet<Feature>();
}
