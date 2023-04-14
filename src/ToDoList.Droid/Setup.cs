using Microsoft.Extensions.Logging;
using MvvmCross.IoC;
using MvvmCross.Platforms.Android.Core;
using Serilog;
using Serilog.Extensions.Logging;
using ToDoList.Droid.Services;
using ToDoList;
using ToDoList.Core;
using ToDoList.Core.Services.Interfaces;

namespace ToDoList.Droid
{
    public class Setup : MvxAndroidSetup<App>
    {
        protected override void InitializeFirstChance(IMvxIoCProvider iocProvider)
        {
            base.InitializeFirstChance(iocProvider);
            iocProvider.LazyConstructAndRegisterSingleton<IDialogService, DialogService>();
        }

        protected override IMvxIocOptions CreateIocOptions() => new MvxIocOptions
        {
            PropertyInjectorOptions = MvxPropertyInjectorOptions.MvxInject
        };

        protected override ILoggerProvider CreateLogProvider() => new SerilogLoggerProvider();

        protected override ILoggerFactory CreateLogFactory()
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.AndroidLog()
                .CreateLogger();

            return new SerilogLoggerFactory();
        }
    }
}
