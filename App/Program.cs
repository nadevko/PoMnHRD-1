using System;
using Avalonia;
using Avalonia.ReactiveUI;

namespace PMnHRD1.App;

class Program
{
    [STAThread]
    public static void Main(string[] args) =>
        BuildAvaloniaApp().StartWithClassicDesktopLifetime(args);

    public static AppBuilder BuildAvaloniaApp() =>
        AppBuilder
            .Configure<App>()
            .WithInterFont()
            .UseReactiveUI()
            .UsePlatformDetect()
            .LogToTrace();
}
