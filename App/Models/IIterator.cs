using System;
using System.Collections.Generic;

namespace PMnHRD1.App.Models;

public interface IIterator : IEnumerator<IQuestion>
{
    bool MovePrevious();
    int SetAnswer(string answer);
    string? GetAnswer();
    string[] GetAnswers();
    IResult GetResult(TimeSpan time, string name, int id);
}
