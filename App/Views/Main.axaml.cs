using System.Collections.ObjectModel;
using System.Diagnostics;
using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using PMnHRD1.App.Models;
using PMnHRD1.App.Services;

namespace PMnHRD1.App.Views;

public partial class Main : Window
{
    public Main() => InitializeComponent();
}

public class MainViewModel : ObservableObject
{
    public MainViewModel()
    {
        _tabView = new() { DataContext = new TabViewModel() };
    }

    public enum Modes
    {
        Suites,
        Test,
    }

    private Modes _view = Modes.Suites;

    public ObservableCollection<Suite> Suites { get; set; } = Json.Instance.Suites;
    public UserControl View
    {
        get =>
            _view switch
            {
                Modes.Suites => _tabView,
                Modes.Test => throw new UnreachableException(),
                _ => throw new UnreachableException(),
            };
        set =>
            _view = value switch
            {
                TabView => Modes.Suites,
                TestView => Modes.Test,
                _ => throw new UnreachableException(),
            };
    }
    private TabView _tabView;
    // private TestView _testView;
}
