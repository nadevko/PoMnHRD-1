using System.Collections.Generic;

namespace PMnHRD1.App.Models;

public interface ITest : IEnumerable<IQuestion>
{
    string Name { get; set; }
    string Description { get; set; }
    List<string> Externals { get; set; }
    List<IQuestion> Questions { get; set; }

    IIterator GetIterator();
}
