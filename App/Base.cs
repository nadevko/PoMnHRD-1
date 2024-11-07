using System;
using System.Collections.Generic;

namespace PMnHRD1.App.Models;

enum Types : uint
{
    Costs,
}

public abstract class Base
{
    public required string Name { get; set; }
    public required string Description { get; set; }
    public required uint Topic { get; set; }
    public required List<string> Externals { get; set; }
}

public class Suite : Base
{
    public List<Test>? Tests { get; set; }
}

public abstract class Test : Base
{
    public List<Question> Questions { get; set; } = [];
    public Stack<Result> Results { get; set; } = [];
}

public abstract class Question
{
    public required string Text { get; set; }
}

public abstract class Result : Question
{
    public TimeSpan Duration { get; set; }
}
