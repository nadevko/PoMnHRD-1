using System.Collections.ObjectModel;
using PMnHRD1.App.Models;

namespace PMnHRD1.App.Services;

interface IData
{
    ObservableCollection<Suite> Suites { get; }
}
