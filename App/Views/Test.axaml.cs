using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using ReactiveUI;

namespace PMnHRD1.App.Views;

public partial class Test : ReactiveUserControl<ViewModels.Test>
{
    public Test()
    {
        this.WhenActivated(disposables => { });
        AvaloniaXamlLoader.Load(this);
    }
}
