using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using PMnHRD1.App.Models;

namespace PMnHRD1.App.Views;

public partial class TestView : UserControl
{
    public TestView(ITest suite) => InitializeComponent();
}

public class TestViewModel : ObservableObject { }
