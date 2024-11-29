using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace PMnHRD1.App.Models;

public partial class TestCosts : ITest
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

    public IEnumerator<IQuestion> GetEnumerator() => new EnumeratorCosts(this);

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    private class EnumeratorCosts : IIterator
    {
        private readonly TestCosts _test;
        private Stack<QuestionCosts> _stack;
        private readonly Random _random = new();
        private QuestionCosts? _current;
        private int sum;

        public EnumeratorCosts(TestCosts test)
        {
            _test = test;
            _stack = new Stack<QuestionCosts>();
            Reset();
            MoveNext();
        }

        public IQuestion Current => _current!;

        object IEnumerator.Current => Current;

        public void Dispose() { }

        public bool MoveNext()
        {
            if (_stack.Count == 0)
                return false;

            _current = _stack.Pop();
            return true;
        }

        public void Reset() =>
            _stack = new Stack<QuestionCosts>(_test._questionCosts.OrderBy(_ => _random.Next()));

        public IResult? MoveNext(int answer)
        {
            sum += _current!.Costs![answer];
            return MoveNext() ? null : Finish();
        }

        public IResult Finish()
        {
            ResultCosts? result = null;
            foreach (var i in _test.Results)
            {
                if (i.From <= sum && sum < i.To)
                    result = i;
            }
            if (result == null)
                throw new Exception($"There are no results for {sum}");
            return new ResultCosts() { Text = result.Text };
        }
    }
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
