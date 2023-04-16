using Microsoft.Extensions.Logging;
using MvvmCross.IoC;
using MvvmCross.Platforms.Ios.Core;
using Serilog;
using Serilog.Extensions.Logging;
using ToDoList.Core;
using ToDoList.Core.Services.Interfaces;
using ToDoList.iOS.Services;
using ToDoList.iOS.Services.Interfaces;

namespace ToDoList.iOS
{
    public class Setup : MvxIosSetup<App>
    {
        protected override void InitializeFirstChance(IMvxIoCProvider iocProvider)
        {
            base.InitializeFirstChance(iocProvider);
            iocProvider.LazyConstructAndRegisterSingleton<IDialogService, DialogService>();
            iocProvider.LazyConstructAndRegisterSingleton<INativeDialogService, NativeDialogService>();
            iocProvider.LazyConstructAndRegisterSingleton<KeyboardInsetTracker, KeyboardInsetTracker>();
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
                .WriteTo.NSLog()
                .CreateLogger();

            return new SerilogLoggerFactory();
        }
    }
}
