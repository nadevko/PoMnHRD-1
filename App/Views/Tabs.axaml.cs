using Avalonia.Controls;
using PMnHRD1.App.Models;

namespace PMnHRD1.App.Views;

public partial class Tabs : UserControl
{
    public Tabs() => InitializeComponent();

    private void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (sender is not ListBox ListBox)
            return;
        if (ListBox.SelectedItem is not ITest test)
            return;
        selectionChangedEventHandler?.Invoke(test);
    }

    public delegate void SelectionChangedEventHandler(ITest test);
    public required SelectionChangedEventHandler selectionChangedEventHandler { get; set; }
}
