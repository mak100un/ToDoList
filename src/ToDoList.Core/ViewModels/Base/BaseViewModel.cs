using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MvvmCross.IoC;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using ReactiveUI;
using ToDoList.Core.Services.Interfaces;
using ToDoList.Core.ViewModels.Interfaces;

namespace ToDoList.Core.ViewModels.Base
{
    public abstract class BaseViewModel : ReactiveObject, IBaseViewModel
    {
        protected BaseViewModel(ILogger logger)
        {
            Logger = logger;
        }

        [MvxInject]
        public IMvxNavigationService NavigationService { get; set; }

        [MvxInject]
        public IDialogService DialogService { get; set; }

        public ILogger Logger { get; }

        public abstract string PageTitle { get; }

        protected async Task RunSafeTaskAsync(Func<Task> task, Action<Exception> onException = null)
        {
            try
            {
                await task();
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, ex.Message);
                onException?.Invoke(ex);
            }
        }

        public virtual void ViewCreated()
        {
        }

        public virtual void ViewAppearing()
        {
        }

        public virtual void ViewAppeared()
        {
        }

        public virtual void ViewDisappearing()
        {
        }

        public virtual void ViewDisappeared()
        {
        }

        public virtual void ViewDestroy(bool viewFinishing = true)
        {
        }

        public void Init(IMvxBundle parameters)
        {
            InitFromBundle(parameters);
        }

        public void ReloadState(IMvxBundle state)
        {
            ReloadFromBundle(state);
        }

        public virtual void Start()
        {
        }

        public void SaveState(IMvxBundle state)
        {
            SaveStateToBundle(state);
        }

        protected virtual void InitFromBundle(IMvxBundle parameters)
        {
        }

        protected virtual void ReloadFromBundle(IMvxBundle state)
        {
        }

        protected virtual void SaveStateToBundle(IMvxBundle bundle)
        {
        }

        public virtual void Prepare()
        {
        }

        public virtual Task Initialize()
        {
            return Task.FromResult(true);
        }

        private MvxNotifyTask _initializeTask;

        // TODO Where is used?
        public MvxNotifyTask InitializeTask
        {
            get => _initializeTask;
            set => SetProperty(ref _initializeTask, value);
        }

        public virtual void OnPropertyChanged([CallerMemberName] string propertyName = "")
            => this.RaisePropertyChanged(propertyName);

        protected virtual bool SetProperty<T>(ref T storage, T value, [CallerMemberName] string? propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(storage, value))
            {
                return false;
            }

            this.RaisePropertyChanging(propertyName);

            storage = value;

            OnPropertyChanged(propertyName);
            return true;
        }
    }
}
