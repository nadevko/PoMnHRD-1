using System.Collections.Generic;
using PMnHRD1.App.Models;
using ReactiveUI;

namespace PMnHRD1.App.ViewModels;

public partial class Question : ReactiveObject, IRoutableViewModel
{
    public IScreen HostScreen { get; }
    public string UrlPathSegment { get; set; } = "";

    // public ReactiveCommand<Unit, IRoutableViewModel> GoBack { get; }
    public ReactiveCommand<IQuestion, bool> ChangeQuestion { get; }

    public Question(IScreen screen, ITest test)
    {
        HostScreen = screen;
        _enumerator = test.GetEnumerator();
        Current = _enumerator.Current;
        ChangeQuestion = ReactiveCommand.Create<IQuestion, bool>(question =>
        {
            return _enumerator.MoveNext();
        });
    }

    private IEnumerator<IQuestion> _enumerator;
    private IQuestion _current = null!;
    public IQuestion Current
    {
        get => _current;
        set => this.RaiseAndSetIfChanged(ref _current, value);
    }
}
