using System;
using System.Linq;
using System.Windows.Input;
using Android.OS;
using Android.Views;
using Google.Android.Material.RadioButton;
using Google.Android.Material.TextField;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Android.Presenters.Attributes;
using ToDoList.Core.Definitions.Converters;
using ToDoList.Core.Definitions.Enums;
using ToDoList.Core.ViewModels;
using ToDoList.Core.ViewModels.Extra;
using ToDoList.Droid.Definitions.Converters;
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

    protected override ICommand MenuCommand => ViewModel.DeleteCommand;

    public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
    {
        var view = base.OnCreateView(inflater, container, savedInstanceState);

        SegmentedControl segmentedControl = view.FindViewById<SegmentedControl>(Resource.Id.segmented_control);
        TextInputLayout titleTextInputLayout = view.FindViewById<TextInputLayout>(Resource.Id.title_text_input_layout);
        TextInputLayout descriptionTextInputLayout = view.FindViewById<TextInputLayout>(Resource.Id.description_text_input_layout);

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

        var set = this.CreateBindingSet<EditTaskFragment, EditTaskViewModel>();

        set
            .Bind(segmentedControl)
            .For(v => v.SelectedSegment)
            .To(vm => vm.SelectedSegment);

        set
            .Bind(titleTextInputLayout)
            .For(v => v.HintEnabled)
            .To(vm => vm.Title)
            .WithConversion<IsNullOrEmptyConverter>();

        set
            .Bind(titleTextInputLayout)
            .For(v => v.Error)
            .To(vm => vm.TitleError);

        set
            .Bind(titleTextInputLayout)
            .For(v => v.ErrorEnabled)
            .To(vm => vm.TitleError)
            .WithConversion<IsNotNullOrEmptyConverter>();

        set
            .Bind(descriptionTextInputLayout)
            .For(v => v.HintEnabled)
            .To(vm => vm.Description)
            .WithConversion<IsNullOrEmptyConverter>();

        set
            .Bind(descriptionTextInputLayout)
            .For(v => v.Error)
            .To(vm => vm.DescriptionError);

        set
            .Bind(descriptionTextInputLayout)
            .For(v => v.ErrorEnabled)
            .To(vm => vm.DescriptionError)
            .WithConversion<IsNotNullOrEmptyConverter>();

        set.Apply();

        return view;
    }
}
