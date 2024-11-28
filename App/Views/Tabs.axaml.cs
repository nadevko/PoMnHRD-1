using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using ReactiveUI;

namespace PMnHRD1.App.Views;

public partial class Tabs : ReactiveUserControl<ViewModels.Tabs>
{
    public Tabs()
    {
        this.WhenActivated(disposables => { });
        AvaloniaXamlLoader.Load(this);
    }
}
