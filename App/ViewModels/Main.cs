using System;
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
                if (_testView!.DataContext is Test testViewModel)
                    testViewModel.Data = test;
                CurrentView = _testView;
            },
        };
        _testView = new Views.Test()
        {
            DataContext = new Test() { GoBack = () => CurrentView = _tabsView },
        };
        CurrentView = _tabsView;
    }

    [ObservableProperty]
    private UserControl _currentView;
    private readonly UserControl _tabsView;
    private readonly UserControl _testView;
    private readonly UserControl _questionView = new Views.Question()
    {
        DataContext = new Question(),
    };
    private readonly UserControl _resultView = new Views.Result() { DataContext = new Result() };
}
