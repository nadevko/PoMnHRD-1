using System;
using System.Reactive;
using System.Reactive.Linq;
using Avalonia.Controls;
using Avalonia.Threading;
using PMnHRD1.App.Models;
using ReactiveUI;
using Splat;

namespace PMnHRD1.App.ViewModels;

public class Question : ReactiveObject, IRoutableViewModel
{
    public IScreen HostScreen => Locator.Current.GetService<Services.INavigate>()!.Screen;
    public string UrlPathSegment { get; set; } = "1";

    public ReactiveCommand<string, int> ChooseAnswer { get; }
    public ReactiveCommand<IResult, IRoutableViewModel> GoResult { get; }
    public ReactiveCommand<Unit, IRoutableViewModel?> GoHome { get; }
    public ReactiveCommand<Views.Question, Unit> GoNext { get; }
    public ReactiveCommand<Views.Question, Unit> GoPrevious { get; }

    private int _questionsCount;
    public int QuestionsCount
    {
        get => _questionsCount;
        set { this.RaiseAndSetIfChanged(ref _questionsCount, value); }
    }
    private int _currentCount = 1;
    public int CurrentCount
    {
        get => _currentCount;
        set { this.RaiseAndSetIfChanged(ref _currentCount, value); }
    }

    DispatcherTimer _timer = new() { Interval = TimeSpan.FromSeconds(1) };
    private TimeSpan _timeSpan = TimeSpan.Zero;
    string _time = "00:00:00";
    public string Time
    {
        get => _time;
        set => this.RaiseAndSetIfChanged(ref _time, value);
    }

    public Question(ITest test)
    {
        _enumerator = test.GetIterator();
        QuestionsCount = test.Questions.Count;
        Current = _enumerator.Current;
        ChooseAnswer = ReactiveCommand.Create<string, int>(question =>
            _enumerator.SetAnswer(question)
        );
        GoResult = ReactiveCommand.CreateFromObservable<IResult, IRoutableViewModel>(result =>
        {
            UrlPathSegment = "result";
            return HostScreen.Router.Navigate.Execute(new Result(result));
        });
        GoHome = ReactiveCommand.CreateFromObservable<Unit, IRoutableViewModel?>(unit =>
        {
            while (HostScreen.Router.NavigationStack.Count > 2)
                HostScreen.Router.NavigationStack.RemoveAt(1);
            return HostScreen.Router.NavigateBack.Execute();
        });
        GoNext = ReactiveCommand.Create<Views.Question, Unit>(question =>
        {
            if (!_enumerator.MoveNext())
            {
                HostScreen.Router.Navigate.Execute(
                    new Result(_enumerator.GetResult(_timeSpan, test.Name, test.Id))
                );
                return new Unit();
            }
            CurrentCount++;
            Current = _enumerator.Current;
            var answer = _enumerator.GetAnswer();
            if (answer != null)
                question.Find<Button>(answer)!.IsEnabled = true;
            return new Unit();
        });
        GoPrevious = ReactiveCommand.Create<Views.Question, Unit>(question =>
        {
            if (!_enumerator.MovePrevious())
            {
                HostScreen.Router.NavigateBack.Execute();
                return new Unit();
            }
            CurrentCount--;
            Current = _enumerator.Current;
            var answer = _enumerator.GetAnswer();
            if (answer != null)
                question.Find<Button>(answer)!.IsEnabled = true;
            return new Unit();
        });
        _timer.Tick += Tick;
        _timer.Start();
        ChooseAnswer.Select(id => id != -1).InvokeCommand(GoNext);
    }

    private IIterator _enumerator;
    private IQuestion _current = null!;
    public IQuestion Current
    {
        get => _current;
        set
        {
            UrlPathSegment = CurrentCount.ToString();
            this.RaiseAndSetIfChanged(ref _current, value);
        }
    }

    private void Tick(object? sender, EventArgs e)
    {
        Time = (_timeSpan = _timeSpan.Add(TimeSpan.FromSeconds(1))).ToString(@"hh\:mm\:ss");
    }
}
