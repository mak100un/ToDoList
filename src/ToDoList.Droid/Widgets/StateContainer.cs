using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reactive.Linq;
using Android.Content;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using Java.Interop;
using MvvmCross.Commands;
using ReactiveUI;
using ToDoList.Core.Definitions;
using ToDoList.Core.Definitions.Extensions;
using ToDoList.Droid.Extensions;

namespace ToDoList.Droid.Widgets;

public class StateContainer : FrameLayout, INotifyPropertyChanged
{
    private IDisposable _disposable;
    private State _previousState;

    protected StateContainer(IntPtr javaReference, JniHandleOwnership transfer)
      : base(javaReference, transfer) => Init();

    public StateContainer(Context context, IAttributeSet attrs)
      : base(context, attrs) => Init();

    public StateContainer(Context context, IAttributeSet attrs, int defStyleAttr)
      : base(context, attrs, defStyleAttr) => Init();

    public StateContainer(
        Context context,
        IAttributeSet attrs,
        int defStyleAttr,
        int defStyleRes)
        : base(context, attrs, defStyleAttr, defStyleRes) => Init();

    public StateContainer(Context context)
        : base(context) => Init();

    private void Init()
    {
        var changeStateCommand = new MvxCommand(OnStateChanged);

        _disposable = this
            .WhenAnyValue(c => c.State, c => c.States)
            .Where(c => c.Item2 != null)
            .InvokeCommand(changeStateCommand);
    }

    private void OnStateChanged()
    {
        if (States == null
            || State == _previousState)
        {
            return;
        }

        _previousState = State;

        var currentView = this.GetAllChildViews()?.FirstOrDefault();
        var newView = States[State];

        if (ReferenceEquals(newView, currentView)
            || newView == null)
        {
            return;
        }

        currentView?.Then(RemoveView);

        using var layoutParams =
            new LayoutParams(ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.MatchParent);

        AddView(newView, layoutParams);
    }

    public State State { get; set; }

    public IReadOnlyDictionary<State, View> States { get; set; }

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
