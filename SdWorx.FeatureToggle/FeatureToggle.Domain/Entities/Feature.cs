namespace FeatureToggle.Domain.Entities;

public sealed class Feature
{
    public int DbId { get; set; }
    public Guid Id { get; set; }
    public string Domain { get; set; } // Api, Frontend, both?
    public string Name { get; set; } // Api, Frontend, both?
    
    public IEnumerable<FeatureState> FeatureState { get; set; } // Whether it is turned on. Need to make this an object (for each env).

    public int ProductDbId { get; set; } // FK.
    public Product Product { get; set; } // PayEngine, Exporter etc.
}
