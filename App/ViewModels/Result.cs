using PMnHRD1.App.Models;
using ReactiveUI;
using Splat;

namespace PMnHRD1.App.ViewModels;

public class Result : ReactiveObject, IRoutableViewModel
{
    public string? UrlPathSegment { get; } = "resultId";
    public IScreen HostScreen => Locator.Current.GetService<Services.INavigate>()!.Screen;

    public Result(IResult result) { }
}
