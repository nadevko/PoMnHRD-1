using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using ReactiveUI;

namespace PMnHRD1.App.Views;

public partial class Result : ReactiveUserControl<ViewModels.Result>
{
    public Result()
    {
        this.WhenActivated(disposables => { });
        AvaloniaXamlLoader.Load(this);
    }
}
