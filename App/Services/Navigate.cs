using ReactiveUI;

namespace PMnHRD1.App.Services;

class Navigate : INavigate
{
    public IScreen Screen { get; }

    public Navigate(IScreen screen) => Screen = screen;
}
