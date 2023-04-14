using MvvmCross.Platforms.Android.Presenters.Attributes;
using ToDoList.Core.ViewModels;
using ToDoList.Core.ViewModels.Extra;

namespace ToDoList.Droid.Fragments;

[MvxFragmentPresentation(
    IsCacheableFragment = true,
    ActivityHostViewModelType = typeof(MainViewModel),
    FragmentContentId = Resource.Id.content_frame,
    AddToBackStack = false)]
public class NewTaskFragment : BaseFragment<NewTaskViewModel>
{
    protected override int ResourceId => Resource.Layout.new_task_layout;
}
