using System.Collections.Generic;

namespace PMnHRD1.App.Models;

public interface IIterator : IEnumerator<IQuestion>
{
    IResult? MoveNext(int answer);
    IResult Finish();
}
