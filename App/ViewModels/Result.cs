using System.Reactive;
using PMnHRD1.App.Models;
using ReactiveUI;
using Splat;

namespace PMnHRD1.App.ViewModels;

public class Result : ReactiveObject, IRoutableViewModel
{
    public string? UrlPathSegment { get; } = "resultId";
    public IScreen HostScreen => Locator.Current.GetService<Services.INavigate>()!.Screen;
    public IResult Current { get; }

    public ReactiveCommand<Unit, IRoutableViewModel?> GoHome { get; }

    public Result(IResult result)
    {
        Current = result;
        GoHome = ReactiveCommand.CreateFromObservable<Unit, IRoutableViewModel?>(unit =>
        {
            while (HostScreen.Router.NavigationStack.Count > 2)
                HostScreen.Router.NavigationStack.RemoveAt(1);
            return HostScreen.Router.NavigateBack.Execute();
        });
    }
}
