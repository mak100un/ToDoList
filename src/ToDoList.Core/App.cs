using MvvmCross.IoC;
using MvvmCross.ViewModels;
using ToDoList.Core.ViewModels.Main;

namespace ToDoList.Core
{
    public class App : MvxApplication
    {
        public override void Initialize()
        {
            CreatableTypes()
                .EndingWith("Service")
                .AsInterfaces()
                .RegisterAsLazySingleton();

            RegisterAppStart<MainViewModel>();
        }
    }
}
