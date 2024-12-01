using ReactiveUI;
using Splat;

namespace PMnHRD1.App.ViewModels;

public class Main : ReactiveObject, IScreen
{
    public RoutingState Router { get; } = new RoutingState();

    public Main()
    {
        Locator.CurrentMutable.RegisterConstant(
            new Services.Navigate(this),
            typeof(Services.INavigate)
        );
        Router.Navigate.Execute(new Tabs());
    }
}
