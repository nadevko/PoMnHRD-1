using System;
using System.Diagnostics;
using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;

namespace PMnHRD1.App.ViewModels;

public partial class Main : ViewModel
{
    public Main()
    {
        _tabsView = new Views.Tabs()
        {
            DataContext = new Tabs(),
            selectionChangedEventHandler = (test) =>
            {
                Console.WriteLine($"Selected test: {test}");
                CurrentView = _testView;
            },
        };
        CurrentView = _tabsView;
    }


    [ObservableProperty]
    private UserControl _currentView;
    private readonly UserControl _tabsView;
    private readonly UserControl _testView = new Views.Test() { DataContext = new Test() };
    private readonly UserControl _questionView = new Views.Question()
    {
        DataContext = new Question(),
    };
    private readonly UserControl _resultView = new Views.Result() { DataContext = new Result() };
}
