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

            CreatableTypes()
                .EndingWith("Service")
                .AsInterfaces()
                .RegisterAsLazySingleton();

            Mvx.IoCProvider.LazyConstructAndRegisterSingleton<ILiteDatabase>(() => new LiteDatabase(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), DbConstants.DB_NAME)));
            Mvx.IoCProvider.LazyConstructAndRegisterSingleton<IToDoListRepository, ToDoListRepository>();
            Mvx.IoCProvider.LazyConstructAndRegisterSingleton(Mapper.Build);

            RegisterAppStart<ToDoListViewModel>();
        }
    }
}
