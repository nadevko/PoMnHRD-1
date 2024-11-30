using System;
using System.Reactive.Linq;
using DynamicData;
using PMnHRD1.App.Models;
using ReactiveUI;

namespace PMnHRD1.App.ViewModels;

public class Question : ReactiveObject, IRoutableViewModel
{
    public IScreen HostScreen { get; }
    public string UrlPathSegment { get; set; } = "1";

    public ReactiveCommand<string, IResult?> ChangeQuestion { get; }
    public ReactiveCommand<IResult, IRoutableViewModel> GoResult { get; }

    public Question(IScreen screen, ITest test)
    {
        HostScreen = screen;
        _enumerator = test.GetIterator();
        Current = _enumerator.Current;
        ChangeQuestion = ReactiveCommand.Create<string, IResult?>(question =>
        {
            var result = _enumerator.MoveNext(Current.Answers!.IndexOf(question));
            Current = _enumerator.Current;
            return result;
        });
        GoResult = ReactiveCommand.CreateFromObservable<IResult, IRoutableViewModel>(result =>
        {
            UrlPathSegment = "result";
            return HostScreen.Router.Navigate.Execute(new Result(HostScreen, result));
        });
        ChangeQuestion.Where(result => result != null).InvokeCommand(GoResult!);
    }

    private IIterator _enumerator;
    private int _n = 1;
    private IQuestion _current = null!;
    public IQuestion Current
    {
        get => _current;
        set
        {
            _n++;
            UrlPathSegment = _n.ToString();
            this.RaiseAndSetIfChanged(ref _current, value);
        }
    }
}
