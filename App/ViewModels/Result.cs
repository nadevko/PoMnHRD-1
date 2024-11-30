using PMnHRD1.App.Models;
using ReactiveUI;

namespace PMnHRD1.App.ViewModels;

public class Result : ReactiveObject, IRoutableViewModel
{
    public string? UrlPathSegment { get; } = "resultId";
    public IScreen HostScreen { get; set; }

    public Result(IScreen screen, IResult result)
    {
        HostScreen = screen;
    }
}
