using Android.Views;
using MvvmCross.DroidX.RecyclerView;
using MvvmCross.Platforms.Android.Binding.BindingContext;

namespace ToDoList.Droid.ViewHolder;

public class LoadingRecyclerViewHolder : MvxRecyclerViewHolder
{
    public LoadingRecyclerViewHolder(View itemView, IMvxAndroidBindingContext context)
        : base(itemView, context)
    {
    }
}
