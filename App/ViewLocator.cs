using System;
using ReactiveUI;

namespace PMnHRD1.App;

public class ViewLocator : IViewLocator
{
    public IViewFor? ResolveView<T>(T? viewModel, string? contract = null) =>
        viewModel switch
        {
            ViewModels.Tabs context => new Views.Tabs { DataContext = context },
            _ => throw new ArgumentOutOfRangeException(nameof(viewModel)),
        };
}
