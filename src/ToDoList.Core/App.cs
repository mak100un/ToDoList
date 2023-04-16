using System;
using System.IO;
using LiteDB;
using MvvmCross;
using MvvmCross.IoC;
using MvvmCross.ViewModels;
using ToDoList.Core.Definitions.Constants;
using ToDoList.Core.Repositories;
using ToDoList.Core.Repositories.Interfaces;
using ToDoList.Core.ViewModels;

namespace ToDoList.Core
{
    public class App : MvxApplication
    {
        public override void Initialize()
        {
            base.Initialize();

            Mvx.IoCProvider.LazyConstructAndRegisterSingleton<ILiteDatabase>(() => new LiteDatabase(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), DbConstants.DB_NAME)));
            Mvx.IoCProvider.RegisterSingleton(() => new Lazy<ILiteDatabase>(Mvx.IoCProvider.Resolve<ILiteDatabase>));

            Mvx.IoCProvider.LazyConstructAndRegisterSingleton<IToDoListRepository, ToDoListRepository>();
            Mvx.IoCProvider.RegisterSingleton(() => new Lazy<IToDoListRepository>(Mvx.IoCProvider.Resolve<IToDoListRepository>));

            Mvx.IoCProvider.LazyConstructAndRegisterSingleton(Mapper.Build);

            RegisterAppStart<ToDoListViewModel>();
        }
    }
}
