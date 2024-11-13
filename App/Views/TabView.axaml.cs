using System;
using System.Collections.ObjectModel;
using Avalonia.Controls;
using Avalonia.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using PMnHRD1.App.Models;
using PMnHRD1.App.Services;

namespace PMnHRD1.App.Views;

public partial class TabView : UserControl
{
    public TabView() => InitializeComponent();

    public void OnTestTapped(object sender, TappedEventArgs e)
    {
        if (sender is Control control && DataContext is TabViewModel svm)
        {
            // DataContext;
        }
    }
}

public class TabViewModel : ObservableObject
{
    public ObservableCollection<Suite> Suites { get; private set; } = Json.Instance.Suites;

}
