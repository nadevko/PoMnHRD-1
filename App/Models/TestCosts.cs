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
    public required int Id { get; set; }
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

        private int index;
        private int[] answerIds;

        public EnumeratorCosts(TestCosts test)
        {
            _test = test;
            _list = [.. _test._questionCosts.OrderBy(_ => _random.Next())];
            answerIds = new int[_list.Count];
            for (int i = 0; i < answerIds.Length; i++)
                answerIds[i] = -1;
            _current = _list[index = 0];
        }

        private QuestionCosts _current;
        public IQuestion Current => _current;
        object IEnumerator.Current => _current;

        public void Dispose() { }

        public void Reset()
        {
            _list = [.. _list.OrderBy(_ => _random.Next())];
            answerIds = new int[_list.Count];
            for (int i = 0; i < answerIds.Length; i++)
                answerIds[i] = -1;
            _current = _list[index = 0];
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

        public IResult GetResult(TimeSpan time, string name, int id)
        {
            ResultCosts? result = null;
            int sum = answerIds
                .Select(
                    (elem, i) =>
                        elem == -1
                            ? 0
                            : ((_test.Questions[i] as QuestionCosts)!.Costs ?? _test.Costs)[elem]
                )
                .Sum();
            foreach (var i in _test.Results)
                if (i.From <= sum && sum < i.To)
                    result = i;
            if (result == null)
                throw new NotImplementedException($"There are no results for {sum}");
            return new ResultCosts()
            {
                Text = result.Text,
                Date = DateTime.Now,
                Time = time,
                Name = name,
                Id = id,
            };
        }

        public int SetAnswer(string answer) =>
            answerIds[index] = (_current.Answers ?? _test.Answers).IndexOf(answer);

        public string? GetAnswer() =>
            answerIds[index] == -1 ? null : (_current.Answers ?? _test.Answers)[answerIds[index]];

        public string[] GetAnswers() => _current.Answers ?? _test.Answers;

        public bool IsEnabled(string answer) =>
            answerIds[index] == (_current.Answers ?? _test.Answers).IndexOf(answer);
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
    public TimeSpan Time { get; set; }
    public DateTime Date { get; set; }
    public int Id { get; set; }
    public string? Name { get; set; }
}
