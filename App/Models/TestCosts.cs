using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DynamicData;

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

    public IIterator GetIterator() => new EnumeratorCosts(this);

    private class EnumeratorCosts : IIterator
    {
        private readonly Random _random = new();
        private readonly TestCosts _test;

        private List<QuestionCosts> _list;

        private int index = 0;
        private int?[] results;

        public EnumeratorCosts(TestCosts test)
        {
            _test = test;
            _list = new(_test._questionCosts.OrderBy(_ => _random.Next()));
            results = new int?[_list.Count];
            for (int i = 0; i < results.Length; i++)
                results[i] = null;
            _current = _list[index];
        }

        private QuestionCosts _current;
        public IQuestion Current => _current;
        object IEnumerator.Current => _current;

        public void Dispose() { }

        public void Reset()
        {
            _list = [.. _list.OrderBy(_ => _random.Next())];
            results = new int?[_list.Count];
            index = 0;
        }

        public bool MoveNext()
        {
            if (++index == _list.Count)
            {
                index--;
                return false;
            }
            _current = _list[index];
            return true;
        }

        public bool MovePrevious()
        {
            if (--index == -1)
            {
                index++;
                return false;
            }
            _current = _list[index];
            return true;
        }

        public IResult GetResult()
        {
            ResultCosts? result = null;
            int? sum = results.Sum();
            foreach (var i in _test.Results)
                if (i.From <= sum && sum < i.To)
                    result = i;
            if (result == null)
                throw new Exception($"There are no results for {sum}");
            return new ResultCosts() { Text = result.Text };
        }

        public int SetAnswer(string answer)
        {
            var answerId = (_current.Answers ?? _test.Answers).IndexOf(answer);
            if (answerId == -1)
                return -1;
            results[index] = (_current.Costs ?? _test.Costs)[answerId];
            return answerId;
        }

        public string? GetAnswer() =>
            results[index] == null
                ? null
                : (_current.Answers ?? _test.Answers)![(int)results[index]!];
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
