using System;
using System.Collections.Generic;
using System.Linq;
using Android.Graphics;
using Android.OS;
using Android.Views;
using Android.Widget;
using MvvmCross.Binding.BindingContext;
using MvvmCross.DroidX.RecyclerView;
using MvvmCross.Platforms.Android.Binding.BindingContext;
using MvvmCross.Platforms.Android.Presenters.Attributes;
using ToDoList.Core.Definitions.Enums;
using ToDoList.Core.ViewModels;
using ToDoList.Core.ViewModels.Extra;
using ToDoList.Droid.Widgets;

namespace ToDoList.Droid.Fragments;

[MvxFragmentPresentation(
    IsCacheableFragment = true,
    ActivityHostViewModelType = typeof(MainViewModel),
    FragmentContentId = Resource.Id.content_frame,
    AddToBackStack = false)]
public class EditTaskFragment : BaseFragment<EditTaskViewModel>
{
    protected override int ResourceId => Resource.Layout.edit_task_layout;

    public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
    {
        var view = base.OnCreateView(inflater, container, savedInstanceState);

        var segmentedControl = view.FindViewById<SegmentedControl>(Resource.Id.segmented_control);

        var items = Enum.GetValues(typeof(ToDoTaskStatus))
            .Cast<ToDoTaskStatus>()
            .Select(status => status.ToString().ToUpper())
            .ToArray();

        foreach (var item in items)
        {
            var radioButton = new RadioButton(Activity);
            radioButton.Text = item;
            radioButton.SetTextColor(Color.Black);
            radioButton.SetBackgroundResource(Resource.Drawable.segmented_control_item_background);
            segmentedControl.AddView(radioButton);
        }

        var set = this.CreateBindingSet<EditTaskFragment, EditTaskViewModel>();

        set
            .Bind(segmentedControl)
            .For(v => v.SelectedSegment)
            .To(vm => vm.SelectedSegment);

        set.Apply();

        return view;
    }
}
