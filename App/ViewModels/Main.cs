using System.Reactive;
using ReactiveUI;

namespace PMnHRD1.App.ViewModels;

public partial class Main : ReactiveObject, IScreen
{
    public RoutingState Router { get; } = new RoutingState();

    public Main() => Router.Navigate.Execute(new Tabs(this));
}
