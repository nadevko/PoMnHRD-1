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

    public IEnumerator<IQuestion> GetEnumerator() => new EnumeratorCosts(_questionCosts);

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    private class EnumeratorCosts : IEnumerator<IQuestion>
    {
        private readonly List<QuestionCosts> _list;
        private Stack<IQuestion> _stack;
        private readonly Random _random = new();
        private IQuestion? _current;

        public EnumeratorCosts(List<QuestionCosts> list)
        {
            _list = list;
            _stack = new Stack<IQuestion>();
            Reset();
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

        public void Reset() => _stack = new Stack<IQuestion>(_list.OrderBy(_ => _random.Next()));
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
