using System.Collections.ObjectModel;
using System.Reactive;
using PMnHRD1.App.Models;
using ReactiveUI;

namespace PMnHRD1.App.ViewModels;

public partial class Tabs : ReactiveObject, IRoutableViewModel
{
    public IScreen HostScreen { get; }
    public string UrlPathSegment { get; } = "/";
    public ReactiveCommand<Unit, IRoutableViewModel> GoTest { get; }

    public Tabs(IScreen screen)
    {
        HostScreen = screen;
        // GoTest = ReactiveCommand.CreateFromObservable(
        //     () => HostScreen!.Router.Navigate.Execute(new Test(HostScreen))
        // );
        // GoTest = ReactiveCommand.CreateFromObservable<ITest>(test =>
        //     HostScreen!.Router.Navigate.Execute(new Test(HostScreen, test))
        // );
    }

    public ObservableCollection<Suite> Suites { get; set; } = Services.Data.Instance.Suites;
}
