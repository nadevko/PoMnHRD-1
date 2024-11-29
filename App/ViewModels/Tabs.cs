using System.Collections.ObjectModel;
using PMnHRD1.App.Models;
using ReactiveUI;
using Splat;

namespace PMnHRD1.App.ViewModels;

public partial class Tabs : ReactiveObject, IRoutableViewModel
{
    public IScreen HostScreen { get; }
    public string UrlPathSegment { get; } = "/";
    public ReactiveCommand<ITest, IRoutableViewModel> GoTest { get; }

    public Tabs(IScreen screen)
    {
        HostScreen = screen;
        GoTest = ReactiveCommand.CreateFromObservable<ITest, IRoutableViewModel>(test =>
            HostScreen.Router.Navigate.Execute(new Test(HostScreen, test))
        );
        this.WhenAnyValue(x => x.SelectedTest).WhereNotNull().InvokeCommand(GoTest);
    }

    private Test? _selectedTest;

    public Test? SelectedTest
    {
        get => _selectedTest;
        set => this.RaiseAndSetIfChanged(ref _selectedTest, value);
    }

    public ObservableCollection<Suite> Suites { get; } =
        Locator.Current.GetService<Services.IData>()!.Suites;
}
