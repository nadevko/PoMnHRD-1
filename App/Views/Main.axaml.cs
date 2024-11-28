using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using ReactiveUI;

namespace PMnHRD1.App.Views;

public partial class Main : ReactiveWindow<ViewModels.Main>
{
    public Main()
    {
        this.WhenActivated(disposables => { });
        AvaloniaXamlLoader.Load(this);
    }
}
