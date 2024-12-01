using System.Reactive;
using PMnHRD1.App.Models;
using ReactiveUI;
using Splat;

namespace PMnHRD1.App.ViewModels;

public class Test : ReactiveObject, IRoutableViewModel
{
    public IScreen HostScreen => Locator.Current.GetService<Services.INavigate>()!.Screen;
    public string UrlPathSegment { get; } = "test";
    public ReactiveCommand<Unit, IRoutableViewModel> GoBack { get; }
    public ReactiveCommand<ITest, IRoutableViewModel> GoQuestion { get; }

    public Test(ITest test)
    {
        Data = test;
        GoBack = ReactiveCommand.CreateFromObservable(
            () => HostScreen!.Router.NavigateBack.Execute(Unit.Default)
        )!;
        GoQuestion = ReactiveCommand.CreateFromObservable<ITest, IRoutableViewModel>(test =>
            HostScreen.Router.Navigate.Execute(new Question(test))
        );
    }

    public ITest Data { get; }
}
