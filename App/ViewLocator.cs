using System;
using Avalonia.Controls;
using Avalonia.Controls.Templates;
using PMnHRD1.App.ViewModels;

namespace PMnHRD1.App;

public class ViewLocator : IDataTemplate
{
    public Control? Build(object? data) => data is null ? null : CreateControl(data);
    public bool Match(object? data) => data is ViewModel;

    private static Control CreateControl(object data)
    {
        var name = data.GetType().FullName!.Replace("ViewModel", "View", StringComparison.Ordinal);
        var type = Type.GetType(name);
        return type == null
            ? new TextBlock { Text = "Not Found: " + name }
            : CreateControl(type, data);
    }

    private static Control CreateControl(Type type, object data)
    {
        var control = Activator.CreateInstance(type) as Control;
        control!.DataContext = data;
        return control!;
    }
}
