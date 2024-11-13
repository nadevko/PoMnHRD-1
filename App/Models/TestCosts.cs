using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace PMnHRD1.App.Models;

public class TestCosts : ITest
{
    public required string Name { get; set; }
    public required string Description { get; set; }
    public required List<string> Externals { get; set; }

    public Stack<ResultCosts> Results { get; set; } = [];
    public required int[] Costs { get; set; }
    public required string[] Answers { get; set; }

    public List<IQuestion> Questions
    {
        get => _questionCosts.Cast<IQuestion>().ToList();
        set => _questionCosts = value.OfType<QuestionCosts>().ToList();
    }
    private List<QuestionCosts> _questionCosts = [];
}

public class QuestionCosts : IQuestion
{
    public required string Text { get; set; }
    public int[]? Costs { get; set; }
    public string[]? Answers { get; set; }
}

public class ResultCosts : IResult
{
    public required string Text { get; set; }
    public int From { get; set; }
    public int To { get; set; }
}
