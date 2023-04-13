using System;
using System.Collections.Generic;
using Cirrious.FluentLayouts.Touch;
using CoreGraphics;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Ios.Binding;
using MvvmCross.Platforms.Ios.Presenters.Attributes;
using ToDoList.Core.Definitions;
using ToDoList.Core.Definitions.Enums;
using ToDoList.Core.ViewModels;
using ToDoList.iOS.Cells;
using ToDoList.iOS.Sources;
using ToDoList.iOS.Styles;
using ToDoList.iOS.Views;
using UIKit;

namespace ToDoList.iOS.ViewControllers
{
    [MvxRootPresentation(WrapInNavigationController = true)]
    public class ToDoListViewController : BaseViewController<ToDoListViewModel>
    {
        private readonly IReadOnlyDictionary<string, Type> _itemTypesToCellsMapper = new Dictionary<string, Type>
        {
            [nameof(ToDoListItemType.Loading)] = typeof(LoaderCell),
            [nameof(ToDoListItemType.Task)] = typeof(ToDoListItemCell),
        };

        private ToDoListSource _todoListDataSource;
        private UIButton _newTaskButton;
        private StateContainer _stateContainer;
        private UIView _emptyView;
        private UIStackView _emptyStackView;

        protected override void CreateView()
        {
            base.CreateView();

            _emptyView = new UIView
            {
                TranslatesAutoresizingMaskIntoConstraints = false,
            };

            _emptyView.Add(_emptyStackView = new UIStackView
            {
                Axis = UILayoutConstraintAxis.Vertical,
                Alignment = UIStackViewAlignment.Fill,
                Spacing = 40f,
            });

            _emptyStackView.AddArrangedSubview(ViewPalette.CreateTitleLabel(TextResources.NO_TASKS_YET, UITextAlignment.Center, UIFont.BoldSystemFontOfSize(32)));

            _emptyStackView.AddArrangedSubview(_newTaskButton = ViewPalette.CreateActionButton(TextResources.CREATE_FIRST_ONE));

            var todoListTableView = new UITableView(CGRect.Empty, UITableViewStyle.Plain)
            {
                ScrollsToTop = false,
                SeparatorStyle = UITableViewCellSeparatorStyle.None,
                BackgroundColor = UIColor.Clear,
                AutoresizingMask = UIViewAutoresizing.All,
                EstimatedRowHeight = 44f,
                RowHeight = UITableView.AutomaticDimension,
                TranslatesAutoresizingMaskIntoConstraints = false,
            };

            foreach (var itemCell in _itemTypesToCellsMapper)
            {
                todoListTableView.RegisterClassForCellReuse(itemCell.Value, itemCell.Key);
            }

            todoListTableView.Source = (_todoListDataSource = new ToDoListSource(todoListTableView));

            View.Add(_stateContainer = new StateContainer(new Dictionary<State, UIView>
            {
                [State.Default] = todoListTableView,
                [State.NoData] = _emptyView,
                [State.None] = default,
            }));

            _stateContainer.SubviewsDoNotTranslateAutoresizingMaskIntoConstraints();
            _emptyView.SubviewsDoNotTranslateAutoresizingMaskIntoConstraints();
        }

        protected override void BindView()
        {
            base.BindView();

            var set = this.CreateBindingSet<ToDoListViewController, ToDoListViewModel>();

            set.Bind(_todoListDataSource)
                .For(x => x.LoadMoreCommand)
                .To(vm => vm.LoadMoreCommand);

            set.Bind(_todoListDataSource)
                .For(x => x.EditTaskCommand)
                .To(vm => vm.EditTaskCommand);

            set.Bind(_todoListDataSource)
                .For(x => x.LoadingOffset)
                .To(vm => vm.LoadingOffset);

            set
                .Bind(_todoListDataSource)
                .For(v => v.Items)
                .To(vm => vm.Items);

            set
                .Bind(_newTaskButton)
                .For(v => v.BindTouchUpInside())
                .To(vm => vm.NewTaskCommand);

            set
                .Bind(_stateContainer)
                .For(v => v.State)
                .To(vm => vm.State);

            set.Apply();
        }

        protected override void LayoutView()
        {
            base.LayoutView();
            var safeAreaGuide = View.SafeAreaLayoutGuide;
            NSLayoutConstraint.ActivateConstraints(new[]
            {
                _stateContainer.BottomAnchor.ConstraintEqualTo(safeAreaGuide.BottomAnchor),
                _stateContainer.TopAnchor.ConstraintEqualTo(TopLayoutGuide.GetBottomAnchor()),
                _stateContainer.LeadingAnchor.ConstraintEqualTo(safeAreaGuide.LeadingAnchor),
                _stateContainer.TrailingAnchor.ConstraintEqualTo(safeAreaGuide.TrailingAnchor),

                _emptyStackView.TopAnchor.ConstraintGreaterThanOrEqualTo(_emptyView.TopAnchor),
                _emptyStackView.BottomAnchor.ConstraintLessThanOrEqualTo(_emptyView.BottomAnchor),
                _emptyStackView.LeadingAnchor.ConstraintEqualTo(_emptyView.LeadingAnchor, 20),
                _emptyStackView.TrailingAnchor.ConstraintEqualTo(_emptyView.TrailingAnchor, -20),
                _emptyStackView.CenterYAnchor.ConstraintEqualTo(_emptyView.CenterYAnchor),
            });
        }
    }
}
