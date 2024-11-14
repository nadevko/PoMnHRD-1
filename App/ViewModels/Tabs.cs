using System.Collections.ObjectModel;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using PMnHRD1.App.Models;
using PMnHRD1.App.Services;

namespace PMnHRD1.App.ViewModels;

public partial class Tabs : ViewModel
{
    // public Tabs()
    // {
    //     GoTestCommand = new RelayCommand(OnGoTest);
    // }

    // public ICommand GoTestCommand { get; }
    // public delegate void GoTestDelegate();
    // public GoTestDelegate GoTest { get; set; }= null!;
    // public void OnGoTest() => GoTest.Invoke();

    public ObservableCollection<Suite> Suites { get; set; } = Data.Instance.Suites;
}
