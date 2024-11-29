using System.Reactive;
using PMnHRD1.App.Models;
using ReactiveUI;

namespace PMnHRD1.App.ViewModels;

public partial class Test : ReactiveObject, IRoutableViewModel
{
    public IScreen HostScreen { get; }
    public string UrlPathSegment { get; } = "test/";
    public ReactiveCommand<Unit, IRoutableViewModel> GoBack { get; }
    public ReactiveCommand<ITest, IRoutableViewModel> GoQuestion { get; }

    public Test(IScreen screen, ITest test)
    {
        HostScreen = screen;
        Data = test;
        GoBack = ReactiveCommand.CreateFromObservable(
            () => HostScreen!.Router.NavigateBack.Execute(Unit.Default)
        )!;
        GoQuestion = ReactiveCommand.CreateFromObservable<ITest, IRoutableViewModel>(test =>
            HostScreen.Router.Navigate.Execute(new Question(HostScreen, test))
        );
    }

    public ITest Data { get; }
}
