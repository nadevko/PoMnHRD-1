using System;
using Avalonia;
using Avalonia.ReactiveUI;

namespace PMnHRD1.App;

class Program
{
    [STAThread]
    public static void Main(string[] args)
    {
        if (args.Length > 0)
        {
            switch (args[0])
            {
                case "--help":
                    Console.WriteLine("Usage: pmnhrd1-app [options]");
                    Console.WriteLine("Options:");
                    Console.WriteLine("  --help       Display this help message.");
                    Console.WriteLine("  --version    Display the application version.");
                    break;
                case "--version":
                    Console.WriteLine("PMnHRD1.App, version 1.0.0");
                    break;
                default:
                    Console.WriteLine("Unknown option. Use --help for usage information.");
                    break;
            }
            return;
        }
        BuildAvaloniaApp().StartWithClassicDesktopLifetime(args);
    }

    public static AppBuilder BuildAvaloniaApp() =>
        AppBuilder
            .Configure<App>()
            .WithInterFont()
            .UseReactiveUI()
            .UsePlatformDetect()
            .LogToTrace();
}
