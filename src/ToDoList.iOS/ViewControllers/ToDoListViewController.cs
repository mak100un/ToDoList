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
    public class ToDoListViewController : BaseToolbarViewController<ToDoListViewModel>
    {
        private readonly IReadOnlyDictionary<string, Type> _itemTypesToCellsMapper = new Dictionary<string, Type>
        {
            [nameof(ToDoListItemType.Loading)] = typeof(LoaderCell),
            [nameof(ToDoListItemType.Task)] = typeof(ToDoListItemCell),
        };

        private StateContainer _stateContainer;

        public override string Image => "Add";

        protected override void CreateView()
        {
            base.CreateView();

            UIView emptyView = null;
            UITableView todoListTableView = null;

            View.Add(_stateContainer = new StateContainer(new Dictionary<State, Func<UIView>>
            {
                [State.Default] = CreateTableView,
                [State.NoData] = CreateEmptyView,
                [State.None] = default,
            }));

            _stateContainer.SubviewsDoNotTranslateAutoresizingMaskIntoConstraints();

            UIView CreateTableView()
            {
                return todoListTableView ??= CreateInnerTableView();

                UITableView CreateInnerTableView()
                {
                    todoListTableView = new UITableView(CGRect.Empty, UITableViewStyle.Plain)
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

                    var todoListDataSource = new ToDoListSource(todoListTableView);
                    todoListTableView.Source = todoListDataSource;

                    var set = this.CreateBindingSet<ToDoListViewController, ToDoListViewModel>();

                    set.Bind(todoListDataSource)
                        .For(x => x.LoadMoreCommand)
                        .To(vm => vm.LoadMoreCommand);

                    set.Bind(todoListDataSource)
                        .For(x => x.EditTaskCommand)
                        .To(vm => vm.EditTaskCommand);

                    set.Bind(todoListDataSource)
                        .For(x => x.LoadingOffset)
                        .To(vm => vm.LoadingOffset);

                    set
                        .Bind(todoListDataSource)
                        .For(v => v.Items)
                        .To(vm => vm.Items);

                    set.Apply();

                    return todoListTableView;
                }
            }

            UIView CreateEmptyView()
            {
                return emptyView ??= CreateInnerEmptyView();

                UIView CreateInnerEmptyView()
                {
                    emptyView = new UIView
                    {
                        TranslatesAutoresizingMaskIntoConstraints = false,
                    };

                    var emptyStackView = new UIStackView
                    {
                        Axis = UILayoutConstraintAxis.Vertical,
                        Alignment = UIStackViewAlignment.Fill,
                        Spacing = 40f,
                    };

                    emptyView.Add(emptyStackView);

                    emptyStackView.AddArrangedSubview(ViewPalette.CreateTitleLabel(TextResources.NO_TASKS_YET, UITextAlignment.Center, UIFont.BoldSystemFontOfSize(32)));

                    var newTaskButton = ViewPalette.CreateActionButton(TextResources.CREATE_FIRST_ONE);
                    emptyStackView.AddArrangedSubview(newTaskButton);

                    var set = this.CreateBindingSet<ToDoListViewController, ToDoListViewModel>();

                    set
                        .Bind(newTaskButton)
                        .For(v => v.BindTouchUpInside())
                        .To(vm => vm.ToolbarCommand);

                    set.Apply();

                    NSLayoutConstraint.ActivateConstraints(new[]
                    {
                        emptyStackView.TopAnchor.ConstraintGreaterThanOrEqualTo(emptyView.TopAnchor),
                        emptyStackView.BottomAnchor.ConstraintLessThanOrEqualTo(emptyView.BottomAnchor),
                        emptyStackView.LeadingAnchor.ConstraintEqualTo(emptyView.LeadingAnchor, 20),
                        emptyStackView.TrailingAnchor.ConstraintEqualTo(emptyView.TrailingAnchor, -20),
                        emptyStackView.CenterYAnchor.ConstraintEqualTo(emptyView.CenterYAnchor),
                    });

                    emptyView.SubviewsDoNotTranslateAutoresizingMaskIntoConstraints();

                    return emptyView;
                }
            }
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
            var safeAreaGuide = View.SafeAreaLayoutGuide;
            NSLayoutConstraint.ActivateConstraints(new[]
            {
                _stateContainer.BottomAnchor.ConstraintEqualTo(safeAreaGuide.BottomAnchor),
                _stateContainer.TopAnchor.ConstraintEqualTo(TopLayoutGuide.GetBottomAnchor()),
                _stateContainer.LeadingAnchor.ConstraintEqualTo(safeAreaGuide.LeadingAnchor),
                _stateContainer.TrailingAnchor.ConstraintEqualTo(safeAreaGuide.TrailingAnchor),
            });
        }
    }
}
