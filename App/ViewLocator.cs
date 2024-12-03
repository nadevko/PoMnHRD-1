using System;
using ReactiveUI;

namespace PMnHRD1.App;

public class ViewLocator : IViewLocator
{
    public IViewFor? ResolveView<T>(T? viewModel, string? contract = null) =>
        viewModel switch
        {
            ViewModels.Question context => new Views.Question { DataContext = context },
            ViewModels.Result context => new Views.Result { DataContext = context },
            ViewModels.Suite context => new Views.Suite { DataContext = context },
            ViewModels.Tabs context => new Views.Tabs { DataContext = context },
            _ => throw new ArgumentOutOfRangeException(nameof(viewModel)),
        };
}
