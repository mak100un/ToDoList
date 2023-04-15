using System;
using System.Linq;
using Android.OS;
using Android.Views;
using Google.Android.Material.RadioButton;
using MvvmCross.Binding.BindingContext;
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
    AddToBackStack = true)]
public class EditTaskFragment : BaseMenuFragment<EditTaskViewModel>
{
    protected override int ResourceId => Resource.Layout.edit_task_layout;

    protected override bool HasBackButton => true;

    protected override int MenuResourceId => Resource.Menu.edit_task_menu;

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
            var radioButton = LayoutInflater.Inflate(Resource.Layout.segment_item, segmentedControl, false) as MaterialRadioButton;
            radioButton.Text = item;
            segmentedControl.AddView(radioButton);
        }

        // TODO CreateBindingSet() ?
        var set = this.CreateBindingSet<EditTaskFragment, EditTaskViewModel>();

        set
            .Bind(segmentedControl)
            .For(v => v.SelectedSegment)
            .To(vm => vm.SelectedSegment);

        set.Apply();

        return view;
    }
}
