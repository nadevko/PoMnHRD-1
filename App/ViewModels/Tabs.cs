using System;
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
    public ReactiveCommand<Models.Suite, IRoutableViewModel> GoSuite { get; }

    public Tabs()
    {
        GoSuite = ReactiveCommand.CreateFromObservable<Models.Suite, IRoutableViewModel>(suite =>
            HostScreen.Router.Navigate.Execute(new Suite(suite))
        );
    }

    private Suite? _selectedTest;

    public Suite? SelectedTest
    {
        get => _selectedTest;
        set => this.RaiseAndSetIfChanged(ref _selectedTest, value);
    }

    public ObservableCollection<Models.Suite> Suites { get; } =
        Locator.Current.GetService<Services.IData>()!.Suites;

    public ObservableCollection<IResult> Results { get; } =
        Locator.Current.GetService<Services.IStats>()!.Get();
}
