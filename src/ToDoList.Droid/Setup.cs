using Android.Views;
using Android.Widget;
using Microsoft.Extensions.Logging;
using MvvmCross.Binding.Bindings.Target.Construction;
using MvvmCross.IoC;
using MvvmCross.Platforms.Android.Core;
using Serilog;
using Serilog.Extensions.Logging;
using ToDoList.Droid.Services;
using ToDoList.Core;
using ToDoList.Core.Services.Interfaces;
using ToDoList.Droid.Bindings;

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

        protected override void FillTargetFactories(IMvxTargetBindingFactoryRegistry registry)
        {
            base.FillTargetFactories(registry);

            // TODO registry.RegisterCustomBindingFactory<>()
            // TODO Using strings ("TextColor") is impractical
            
            // TODO MvxImageViewResourceNameTargetBinding
            registry.RegisterFactory(new MvxCustomBindingFactory<ImageView>("DrawableResourceId", imageView => new ImageViewDrawableIdTargetBinding(imageView)));
            registry.RegisterFactory(new MvxCustomBindingFactory<TextView>("TextColor", textView => new TextViewTextColorTargetBinding(textView)));
            registry.RegisterFactory(new MvxCustomBindingFactory<View>("Background", view => new ViewBackgroundTargetBinding(view)));
        }

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
