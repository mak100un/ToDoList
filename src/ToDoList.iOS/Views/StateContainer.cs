using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Cirrious.FluentLayouts.Touch;
using MvvmCross.Commands;
using ReactiveUI;
using ToDoList.Core.Definitions;
using UIKit;

namespace ToDoList.iOS.Views;

public class StateContainer : UIView, INotifyPropertyChanged
{
    private readonly IReadOnlyDictionary<State, Func<UIView>> _states;
    private IDisposable _disposable;
    private State _previousState;

    public StateContainer(IReadOnlyDictionary<State, Func<UIView>> states)
    {
        _states = states;

        var changeStateCommand = new MvxCommand(OnStateChanged);

        _disposable = this
            .WhenAnyValue(c => c.State)
            .InvokeCommand(changeStateCommand);
    }

    private void OnStateChanged()
    {
        if (State == _previousState)
        {
            return;
        }
        _previousState = State;

        var currentView = Subviews?.FirstOrDefault();
        var newView = _states[State]();

        if (ReferenceEquals(newView, currentView)
            || newView == null)
        {
            return;
        }

        currentView?.RemoveFromSuperview();

        Add(newView);

        this.AddConstraints(
            // newView
            newView.AtTopOf(this),
            newView.AtLeadingOf(this),
            newView.AtTrailingOf(this),
            newView.AtBottomOf(this)
        );
    }

    public State State { get; set; }

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            _disposable?.Dispose();
            _disposable = null;
        }
        base.Dispose(disposing);
    }

    public event PropertyChangedEventHandler PropertyChanged;
}
