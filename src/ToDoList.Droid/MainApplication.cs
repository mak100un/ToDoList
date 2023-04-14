using System;
using Android.Runtime;
using MvvmCross.Platforms.Android.Views;
using ToDoList;
using ToDoList.Core;

namespace ToDoList.Droid
{
    public class MainApplication : MvxAndroidApplication<Setup, App>
    {
        public MainApplication(IntPtr javaReference, JniHandleOwnership transfer)
            : base(javaReference, transfer)
        {
        }
    }
}
