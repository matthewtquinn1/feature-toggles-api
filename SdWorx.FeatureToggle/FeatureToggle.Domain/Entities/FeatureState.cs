using FeatureToggle.Domain.Enums;

namespace FeatureToggle.Domain.Entities;

public sealed class FeatureState
{
    public int DbId { get; set; }
    public Guid Id { get; set; }
    public FeatureEnvironment Environment { get; set; }
    public bool IsActive { get; set; }

    public int FeatureDbId { get; set; } // FK.
    public Feature Feature { get; set; }
}
