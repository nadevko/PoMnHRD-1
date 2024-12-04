using System.Collections.ObjectModel;
using PMnHRD1.App.Models;

namespace PMnHRD1.App.Services;

interface IStats
{
    void Push(IResult result);
    ObservableCollection<IResult> Get();
}
