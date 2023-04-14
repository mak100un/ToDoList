using System;
using System.ComponentModel;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using Android.Content;
using Android.Runtime;
using Android.Util;
using Android.Widget;
using ReactiveUI;
using ToDoList.Droid.Extensions;

namespace ToDoList.Droid.Widgets;

public class SegmentedControl : RadioGroup, INotifyPropertyChanged
{
    private readonly CompositeDisposable _compositeDisposable = new ();

    protected SegmentedControl(IntPtr javaReference, JniHandleOwnership transfer)
        : base(javaReference, transfer) => Init();

    public SegmentedControl(Context context, IAttributeSet attrs)
        : base(context, attrs) => Init();

    public SegmentedControl(Context context)
        : base(context) => Init();

    private void Init()
    {
        this
            .WhenAnyValue(c => c.CheckedRadioButtonId)
            .Where(id => id != -1)
            .Select(_ => FindViewById(CheckedRadioButtonId))
            .WhereNotNull()
            .Subscribe(rB => SelectedSegment = IndexOfChild(rB))
            .DisposeWith(_compositeDisposable);

        this
            .WhenAnyValue(c => c.SelectedSegment)
            .Where(c => c != -1)
            .Select(_ => this.GetChildAt<RadioButton>(SelectedSegment))
            .WhereNotNull()
            .Subscribe(rB => rB.Checked = true)
            .DisposeWith(_compositeDisposable);
    }

    public int SelectedSegment { get; set; } = -1;

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            _compositeDisposable?.Clear();
        }
        base.Dispose(disposing);
    }

    public event PropertyChangedEventHandler PropertyChanged;
}
