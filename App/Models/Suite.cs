using System.Collections.Generic;

namespace PMnHRD1.App.Models;

public class Suite
{
    public required string Name { get; set; }
    public required string Description { get; set; }
    public required uint Id { get; set; }
    public required List<string> Externals { get; set; }
    public List<ITest>? Tests { get; set; }
}
