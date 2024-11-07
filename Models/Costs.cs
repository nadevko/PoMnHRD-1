using System.Collections.Generic;

namespace PMnHRD1.Models;

public class TestCosts : Test {
    public new Stack<ResultCosts> Results { get; set; } = [];
}

public class QuestionCosts : Question
{
    public required int[] Costs { get; set; }
    public required string[] Answers { get; set; }
}

public class ResultCosts : Result
{
    public int From { get; set; }
    public int To { get; set; }
}
