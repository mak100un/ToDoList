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
using ToDoList.Droid.Definitions.Constants;
using ToDoList.Droid.Services.Interfaces;

namespace ToDoList.Droid
{
    public class Setup : MvxAndroidSetup<App>
    {
        protected override void InitializeFirstChance(IMvxIoCProvider iocProvider)
        {
            base.InitializeFirstChance(iocProvider);
            iocProvider.LazyConstructAndRegisterSingleton<IDialogService, DialogService>();
            iocProvider.LazyConstructAndRegisterSingleton<INativeDialogService, NativeDialogService>();
        }

        protected override IMvxIocOptions CreateIocOptions() => new MvxIocOptions
        {
            PropertyInjectorOptions = MvxPropertyInjectorOptions.MvxInject
        };

        protected override void FillTargetFactories(IMvxTargetBindingFactoryRegistry registry)
        {
            base.FillTargetFactories(registry);

            registry.RegisterFactory(new MvxCustomBindingFactory<ImageView>(BindingConstants.IMAGEVIEW_DRAWABLE_RESOURCE_ID, imageView => new ImageViewDrawableIdTargetBinding(imageView)));
            registry.RegisterFactory(new MvxCustomBindingFactory<TextView>(BindingConstants.TEXTVIEW_TEXT_COLOR, textView => new TextViewTextColorTargetBinding(textView)));
            registry.RegisterFactory(new MvxCustomBindingFactory<View>(BindingConstants.VIEW_BACKGROUND, view => new ViewBackgroundTargetBinding(view)));
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
