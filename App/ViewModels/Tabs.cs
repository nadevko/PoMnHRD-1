using System.Collections.ObjectModel;
using System.Linq;
using PMnHRD1.App.Models;
using ReactiveUI;
using Splat;

namespace PMnHRD1.App.ViewModels;

public class Tabs : ReactiveObject, IRoutableViewModel
{
    public IScreen HostScreen => Locator.Current.GetService<Services.INavigate>()!.Screen;
    public string UrlPathSegment { get; } = "";
    public ReactiveCommand<Suite, IRoutableViewModel> GoSuite { get; }

    public Tabs()
    {
        GoSuite = ReactiveCommand.CreateFromObservable<Suite, IRoutableViewModel>(suite =>
            HostScreen.Router.Navigate.Execute(new Test(suite.Tests![0]))
        );
        // this.WhenAnyValue(x => x.SelectedTest).WhereNotNull().InvokeCommand(GoTest);
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
