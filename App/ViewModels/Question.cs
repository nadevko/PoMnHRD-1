using System.Collections.Generic;
using CommunityToolkit.Mvvm.ComponentModel;
using PMnHRD1.App.Models;

namespace PMnHRD1.App.ViewModels;

public partial class Question : ViewModel
{
    public Question(ITest test)
    {
        _enumerator = test.GetEnumerator();
        Current = _enumerator.Current;
    }

    [ObservableProperty]
    private IQuestion _current;

    private readonly IEnumerator<IQuestion> _enumerator;
}
