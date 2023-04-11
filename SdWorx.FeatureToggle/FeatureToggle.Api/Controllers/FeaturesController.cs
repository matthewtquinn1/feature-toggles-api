using Microsoft.AspNetCore.Mvc;

namespace Toggle.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class FeaturesController : ControllerBase
{
    private static readonly string[] _features = new[]
    {
        "PayEngineApi.EditCollection", 
        "PayrollExporterApi.DeleteExport",
        "PayrollExporterApi.DuplicateExport",
        "PayResultsUi.NewSearchScreen",
    };

    private readonly ILogger<FeaturesController> _logger;

    public FeaturesController(ILogger<FeaturesController> logger)
    {
        _logger = logger;
    }

    [HttpGet]
    public IEnumerable<string> Get()
    {
        return _features;
    }

    [HttpGet("{id:Guid}")]
    public string GetById(Guid id)
    {
        return _features.First();
    }

    [HttpPost]
    public Guid Create(CreateFeature toggle)
    {
        return Guid.NewGuid();
    }

    [HttpPut]
    public Feature Update(UpdateFeature toggle)
    {
        return new Feature();
    }

    [HttpPatch]
    public Feature ToggleActiveState(Guid id, Environment environment, string active)
    {
        return new Feature();
    }

    [HttpDelete("{id:Guid}")]
    public void Delete(Guid id)
    {
        
    }
}

public sealed class CreateFeature
{
    public string Domain { get; set; } // Api, Frontend, both?
    public string Product { get; set; } // PayEngine, Exporter etc.
    public string Name { get; set; } // Api, Frontend, both?
}

public sealed class UpdateFeature
{
    public int DbId { get; set; }
    public Guid Id { get; set; }
    public string Domain { get; set; } // Api, Frontend, both?
    public string Product { get; set; } // PayEngine, Exporter etc.
    public string Name { get; set; } // Api, Frontend, both?
    public FeatureState EnvironmentStates { get; set; } // Whether it is turned on. Need to make this an object (for each env).
}

public sealed class Feature
{
    public int DbId { get; set; }
    public Guid Id { get; set; }
    public string Domain { get; set; } // Api, Frontend, both?
    public Product Product { get; set; } // PayEngine, Exporter etc.
    public string Name { get; set; } // Api, Frontend, both?
    public FeatureState FeatureState { get; set; } // Whether it is turned on. Need to make this an object (for each env).
}

public sealed class Product
{
    public int DbId { get; set; }
    public Guid Id { get; set; }
    public string Name { get; set; }
}

public sealed class FeatureState
{
    public Environment Environment { get; set; }
    public bool IsActive { get; set; }
}

public enum Environment
{
    Local,
    Dev,
    QA,
    PreProd,
    NonProd,
    Prod,
}