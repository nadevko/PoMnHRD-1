using System.Collections.ObjectModel;
using PMnHRD1.App.Models;
using PMnHRD1.App.Services;

namespace PMnHRD1.App.ViewModels;

public partial class Tabs : ViewModel
{
    public ObservableCollection<Suite> Suites { get; set; } = Data.Instance.Suites;
}
