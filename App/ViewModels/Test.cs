using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PMnHRD1.App.Models;

namespace PMnHRD1.App.ViewModels;

public partial class Test : ViewModel
{
    public Test()
    {
        GoBackCommand = new RelayCommand(OnGoBack);
    }

    [ObservableProperty]
    public ITest _data = null!;

    public ICommand GoBackCommand { get; }
    public delegate void GoBackDelegate();
    public GoBackDelegate GoBack { get; set; } = null!;
    public void OnGoBack() => GoBack.Invoke();
}
