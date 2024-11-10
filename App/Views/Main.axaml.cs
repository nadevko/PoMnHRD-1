using System.Collections.ObjectModel;
using Avalonia.Controls;
using Avalonia.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using PMnHRD1.App.Models;
using PMnHRD1.App.Services;

namespace PMnHRD1.App.Views;

public partial class Main : Window
{
    public Main() => InitializeComponent();

    private void OnSuiteTap(object sender, TappedEventArgs e)
    {
        // if (sender is TextBlock { DataContext: Suite suite })
        // var suiteView = new SuiteView { DataContext = suite };
    }

    private void OnTestTap(object sender, TappedEventArgs e)
    {
        //     if (sender is Grid { DataContext: Test test })
        // DisplayTestDetails(test);
    }
}

public class MainViewModel : ObservableObject
{
    public ObservableCollection<Suite> Suites { get; set; } = Json.Instance.Suites;
}
