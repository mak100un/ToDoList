using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Cirrious.FluentLayouts.Touch;
using CoreGraphics;
using Foundation;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Ios.Binding;
using MvvmCross.Platforms.Ios.Presenters.Attributes;
using ToDoList.Core.Definitions.Enums;
using ToDoList.Core.Definitions.Extensions;
using ToDoList.Core.ViewModels;
using ToDoList.iOS.Cells;
using ToDoList.iOS.Sources;
using ToDoList.iOS.Styles;
using ToDoList.iOS.Views;
using UIKit;

namespace ToDoList.iOS.ViewControllers
{
    [MvxRootPresentation(WrapInNavigationController = true)]
    public class ToDoListViewController : BaseNavigationItemViewController<ToDoListViewModel>
    {
        private StateContainer _stateContainer;
        private UIView _emptyView;
        private UITableView _todoListTableView;

        protected override string Image => "Add";
        protected override Expression<Func<ToDoListViewModel, object>> NavigationItemCommandExtractor => vm => vm.NewTaskCommand;

        protected override void CreateView()
        {
            base.CreateView();

            View.Add(_stateContainer = new StateContainer(new Dictionary<State, Func<UIView>>
            {
                [State.Default] = CreateTableView,
                [State.NoData] = CreateEmptyView,
                [State.None] = default,
            }));

            _stateContainer.SubviewsDoNotTranslateAutoresizingMaskIntoConstraints();
        }

        protected override void BindView()
        {
            base.BindView();

            var set = this.CreateBindingSet<ToDoListViewController, ToDoListViewModel>();

            set
                .Bind(_stateContainer)
                .For(v => v.State)
                .To(vm => vm.State);

            set.Apply();
        }

        protected override void LayoutView()
        {
            base.LayoutView();

            View.AddConstraints(
                // _stateContainer
                _stateContainer.AtBottomOfSafeArea(View),
                _stateContainer.AtTopOfSafeArea(View),
                _stateContainer.AtLeftOfSafeArea(View),
                _stateContainer.AtRightOfSafeArea(View)
            );
        }

        protected override void SetNavigationItemVisibility(MvxFluentBindingDescriptionSet<BaseNavigationItemViewController<ToDoListViewModel>, ToDoListViewModel> bindingSet)
        {
            bindingSet
                .Bind(NavigationItem.RightBarButtonItem)
                .For(v => v.Hidden)
                .To(vm => vm.State)
                .WithGenericConversion((State state) => state switch
                {
                    State.Default => false,
                    _ => true
                });

        }

        private UIView CreateTableView()
        {
            return _todoListTableView ??= CreateInnerTableView();

            UITableView CreateInnerTableView()
            {
                _todoListTableView = new DynamicFooterTableView(CGRect.Empty, UITableViewStyle.Plain)
                {
                    ScrollsToTop = false,
                    SeparatorStyle = UITableViewCellSeparatorStyle.None,
                    BackgroundColor = UIColor.Clear,
                    AutoresizingMask = UIViewAutoresizing.All,
                    EstimatedRowHeight = 44f,
                    RowHeight = UITableView.AutomaticDimension,
                    TranslatesAutoresizingMaskIntoConstraints = false,
                };

                _todoListTableView.TableHeaderView = new UIView(new CGRect(0, 0, 0, 10));

                _todoListTableView.RegisterClassForHeaderFooterViewReuse(typeof(LoaderView), nameof(LoaderView));
                _todoListTableView.RegisterClassForCellReuse(typeof(ToDoListItemCell), nameof(ToDoListItemCell));

                var todoListDataSource = new ToDoListSource(_todoListTableView, () => _todoListTableView.ScrollToRow(NSIndexPath.FromRowSection(0, 0), UITableViewScrollPosition.Top, true))
                {
                    DeselectAutomatically = true,
                    AddAnimation = UITableViewRowAnimation.Automatic,
                    RemoveAnimation = UITableViewRowAnimation.Automatic,
                    ReplaceAnimation = UITableViewRowAnimation.Automatic,
                    UseAnimations = true,
                };
                _todoListTableView.Source = todoListDataSource;

                var set = this.CreateBindingSet<ToDoListViewController, ToDoListViewModel>();

                set.Bind(todoListDataSource)
                    .For(x => x.LoadMoreCommand)
                    .To(vm => vm.LoadMoreCommand);

                set.Bind(todoListDataSource)
                    .For(x => x.IsLoadingMore)
                    .To(vm => vm.IsLoadingMore);

                set.Bind(todoListDataSource)
                    .For(x => x.SelectionChangedCommand)
                    .To(vm => vm.EditTaskCommand);

                set.Bind(todoListDataSource)
                    .For(x => x.LoadingOffset)
                    .To(vm => vm.LoadingOffset);

                set
                    .Bind(todoListDataSource)
                    .For(v => v.Items)
                    .To(vm => vm.Items);

                set
                    .Bind(todoListDataSource)
                    .For(v => v.ItemsSource)
                    .To(vm => vm.Items);

                set.Apply();

                return _todoListTableView;
            }
        }

        private UIView CreateEmptyView()
        {
            return _emptyView ??= CreateInnerEmptyView();

            UIView CreateInnerEmptyView()
            {
                _emptyView = new UIView
                {
                    TranslatesAutoresizingMaskIntoConstraints = false,
                };

                var emptyStackView = new UIStackView
                {
                    Axis = UILayoutConstraintAxis.Vertical,
                    Alignment = UIStackViewAlignment.Fill,
                    Spacing = 40f,
                };

                _emptyView.Add(emptyStackView);

                emptyStackView.AddArrangedSubview(ViewPalette.CreateTitleLabel(TextResources.NO_TASKS_YET, UITextAlignment.Center, UIFont.BoldSystemFontOfSize(32)));

                var newTaskButton = ViewPalette.CreateActionButton(TextResources.CREATE_FIRST_ONE);
                emptyStackView.AddArrangedSubview(newTaskButton);

                var set = this.CreateBindingSet<ToDoListViewController, ToDoListViewModel>();

                set
                    .Bind(newTaskButton)
                    .For(v => v.BindTouchUpInside())
                    .To(vm => vm.NewTaskCommand);

                set.Apply();

                _emptyView.AddConstraints(
                    // emptyStackView
                    emptyStackView.Top().GreaterThanOrEqualTo().TopOf(_emptyView),
                    emptyStackView.Bottom().LessThanOrEqualTo().BottomOf(_emptyView),
                    emptyStackView.Leading().EqualTo().LeadingOf(_emptyView).Plus(20),
                    emptyStackView.Trailing().EqualTo().TrailingOf(_emptyView).Minus(20),
                    emptyStackView.CenterY().EqualTo().CenterYOf(_emptyView)
                );

                _emptyView.SubviewsDoNotTranslateAutoresizingMaskIntoConstraints();

                return _emptyView;
            }
        }
    }
}
