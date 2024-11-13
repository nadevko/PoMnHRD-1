using System.Diagnostics;
using Avalonia.Controls;
using PMnHRD1.App.Views;

namespace PMnHRD1.App.ViewModels;

public partial class Main : ViewModel
{
    public UserControl CurrentView
    {
        get =>
            _currentView switch
            {
                Views.Tabs => _tabsView,
                Views.Test => _testView,
                Views.Question => _questionView,
                Views.Result => _resultView,
                _ => throw new UnreachableException(),
            };
        set =>
            _currentView = value switch
            {
                Tabs => Views.Tabs,
                Test => Views.Test,
                Question => Views.Question,
                Result => Views.Result,
                _ => throw new UnreachableException(),
            };
    }

    private enum Views
    {
        Tabs,
        Test,
        Question,
        Result,
    }
    private Views _currentView = Views.Tabs;

    private UserControl _tabsView = new Tabs();
    private UserControl _testView = new Test();
    private UserControl _questionView = new Question();
    private UserControl _resultView = new Result();
}
