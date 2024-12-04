using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Splat;

namespace PMnHRD1.App;

public sealed class App : Application
{
    public override void Initialize() => AvaloniaXamlLoader.Load(this);

    public override void OnFrameworkInitializationCompleted()
    {
        Locator.CurrentMutable.RegisterConstant(new Services.Data(), typeof(Services.IData));
        Locator.CurrentMutable.RegisterConstant(new Services.Stats(), typeof(Services.IStats));
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            desktop.MainWindow = new Views.Main { DataContext = new ViewModels.Main() };
        base.OnFrameworkInitializationCompleted();
    }
}
