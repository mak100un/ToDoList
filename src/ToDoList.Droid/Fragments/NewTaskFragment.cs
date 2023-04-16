using Android.OS;
using Android.Views;
using Google.Android.Material.TextField;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Android.Presenters.Attributes;
using ToDoList.Core.Definitions.Converters;
using ToDoList.Core.ViewModels;
using ToDoList.Core.ViewModels.Extra;
using ToDoList.Droid.Definitions.Converters;

namespace ToDoList.Droid.Fragments;

[MvxFragmentPresentation(
    IsCacheableFragment = true,
    ActivityHostViewModelType = typeof(MainViewModel),
    FragmentContentId = Resource.Id.content_frame,
    AddToBackStack = true)]
public class NewTaskFragment : BaseFragment<NewTaskViewModel>
{
    protected override int ResourceId => Resource.Layout.new_task_layout;

    protected override bool HasBackButton => true;

    public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
    {
        var view = base.OnCreateView(inflater, container, savedInstanceState);

        TextInputLayout titleTextInputLayout = view.FindViewById<TextInputLayout>(Resource.Id.title_text_input_layout);
        TextInputLayout descriptionTextInputLayout = view.FindViewById<TextInputLayout>(Resource.Id.description_text_input_layout);

        var set = this.CreateBindingSet<NewTaskFragment, NewTaskViewModel>();

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
