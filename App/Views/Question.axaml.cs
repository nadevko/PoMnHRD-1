using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using ReactiveUI;

namespace PMnHRD1.App.Views;

public partial class Question : ReactiveUserControl<ViewModels.Question>
{
    public Question()
    {
        this.WhenActivated(disposables => { });
        AvaloniaXamlLoader.Load(this);
    }
}
