using beyond.park.client.Controls.ActionBars;
using beyond.park.client.Controls.Popups;
using beyond.park.client.ViewModels.Base;
using beyond.park.client.ViewModels.Base.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using Xamarin.Forms;

namespace beyond.park.client.Views.Base {

    public partial class ContentPageBaseView : ContentPage {
        private TapGestureRecognizer _popupBlockBackingTapGesture = new TapGestureRecognizer();

        private TapGestureRecognizer _bottomItemTapGestureRecognizer = new TapGestureRecognizer();

        private static readonly string BUSY_BINDING_PATH = "IsBusy";

        private static readonly string IS_POPUP_VISIBLE_BINDING_PATH = "IsPopupsVisible";

        private static readonly string POPUPS_BINDING_PATH = "Popups";

        private static readonly string IS_PULL_TO_REFRESH_ENABLED_BINDING_PATH = "IsPullToRefreshEnabled";

        private static readonly string IS_REFRESHING_BINDING_PATH = "IsRefreshing";

        private static readonly string REFRESH_COMMAND_BINDING_PATH = "RefreshCommand";

        public static readonly BindableProperty MainContentProperty =
            BindableProperty.Create(nameof(MainContent),
                typeof(View),
                typeof(ContentPageBaseView),
                defaultValue: null,
                propertyChanged: (BindableObject bindable, object oldValue, object newValue) => {
                    if (bindable is ContentPageBaseView declarer) {
                        declarer._contentBox_Grid.Children.Add(newValue as View);
                    }
                });

        public static readonly BindableProperty ActionBarProperty =
            BindableProperty.Create(nameof(ActionBar),
                typeof(ActionBarBaseView),
                typeof(ContentPageBaseView),
                defaultValue: null,
                propertyChanged: (BindableObject bindable, object oldValue, object newValue) => {
                    if (bindable is ContentPageBaseView declarer) {
                        declarer._actionBarSpot_ContentView.Content = newValue as ActionBarBaseView;
                    }
                });

        public static readonly BindableProperty IsBusyAwaitingProperty =
            BindableProperty.Create(nameof(IsBusyAwaiting),
                typeof(bool),
                typeof(ContentPageBaseView),
                defaultValue: false,
                propertyChanged: (BindableObject bindable, object oldValue, object newValue) => {
                    if (bindable is ContentPageBaseView declarer) {
                        declarer._busyIndicator_Indicator.IsVisible = declarer.IsBusyAwaiting;

                        if (declarer.IsBusyAwaiting) {
                            Grid.SetRow(declarer._busyIndicator_Indicator, 0);
                        } else {
                            Grid.SetRow(declarer._busyIndicator_Indicator, 1);
                        }
                    }
                });

        public static readonly BindableProperty IsPopupVisibleProperty =
            BindableProperty.Create(nameof(IsPopupVisible),
                typeof(bool),
                typeof(ContentPageBaseView),
                defaultValue: false,
                propertyChanged: async (BindableObject bindable, object oldValue, object newValue) => {
                    if (bindable is ContentPageBaseView declarer) {
                        if (declarer.IsPopupVisible) {
                            declarer._popupSpot_ContentView.Opacity = 0;
                            Grid.SetRow(declarer._popupSpot_ContentView, 0);
                            await declarer._popupSpot_ContentView.FadeTo(1);
                        } else {
                            declarer._popupSpot_ContentView.Opacity = 1;
                            await declarer._popupSpot_ContentView.FadeTo(0);
                            Grid.SetRow(declarer._popupSpot_ContentView, 1);

                            declarer._popupsKeeper_PopupsBlockView.CloseAllPopups();
                        }
                    }
                });

        public static readonly BindableProperty IsPullToRefreshEnabledProperty =
            BindableProperty.Create(nameof(IsPullToRefreshEnabled),
                typeof(bool),
                typeof(ContentPageBaseView),
                defaultValue: default(bool));

        public static readonly BindableProperty IsRefreshingProperty =
            BindableProperty.Create(nameof(IsRefreshing),
                typeof(bool),
                typeof(ContentPageBaseView),
                defaultValue: default(bool));

        public static readonly BindableProperty RefreshCommandProperty =
            BindableProperty.Create(nameof(RefreshCommand),
                typeof(ICommand),
                typeof(ContentPageBaseView));

        public static readonly BindableProperty BottomBarItemsProperty =
            BindableProperty.Create(nameof(BottomBarItems),
                typeof(IEnumerable<IBottomBarTab>),
                typeof(ContentPageBaseView),
                propertyChanged: (BindableObject bindable, object oldValue, object newValue) => {
                    if (bindable is ContentPageBaseView declarer) {
                        declarer._bottomBarSpot_Grid.Children.Clear();
                        declarer._bottomBarSpot_Grid.ColumnDefinitions.Clear();
                        declarer._contentBox_Grid.Children.Clear();

                        if (newValue is IEnumerable<IBottomBarTab> queueNewValue) {
                            for (int i = 0; i < queueNewValue.Count(); i++) {
                                BottomItemViewBase singleVisualBottomItem = (BottomItemViewBase)new DataTemplate(queueNewValue.ElementAt(i).BottomTasselViewType).CreateContent();
                                singleVisualBottomItem.TabIndex = i;
                                singleVisualBottomItem.BindingContext = queueNewValue.ElementAt(i);
                                singleVisualBottomItem.GestureRecognizers.Add(declarer._bottomItemTapGestureRecognizer);
                                Grid.SetColumn(singleVisualBottomItem, i);

                                declarer._bottomBarSpot_Grid.ColumnDefinitions.Add(new ColumnDefinition() {
                                    Width = new GridLength(1, GridUnitType.Star)
                                });
                                declarer._bottomBarSpot_Grid.Children.Add(singleVisualBottomItem);

                                if (!(singleVisualBottomItem.BindingContext is ViewLessTabViewModel)) {
                                    declarer._contentBox_Grid.Children.Add(singleVisualBottomItem.AppropriateItemContentView);
                                }
                            }

                            if (queueNewValue.Any()) {
                                declarer.SelectedBottomItemIndex = 0;
                            }
                        }
                    }
                });

        public static readonly BindableProperty SelectedBottomItemIndexProperty =
            BindableProperty.Create(nameof(SelectedBottomItemIndex),
                typeof(int),
                typeof(ContentPageBaseView),
                defaultValue: -1,
                propertyChanged: (BindableObject bindable, object oldValue, object newValue) => {
                    if (bindable is ContentPageBaseView declarer) {

                        if (declarer.BottomBarItems != null && declarer.BottomBarItems.Any()) {
                            IBottomBarTab targetTab = declarer.BottomBarItems.ElementAt((int)newValue);

                            if (targetTab is IActionBottomBarTab actionBottomBarTab) {
                                actionBottomBarTab.TabActionCommand?.Execute(null);
                            }

                            if (targetTab is ViewLessTabViewModel) {
                                if ((int)oldValue != (int)newValue) {
                                    declarer.SelectedBottomItemIndex = (int)oldValue;
                                }
                            } else {
                                IEnumerable<BottomItemViewBase> bottomItems = declarer._bottomBarSpot_Grid.Children.OfType<BottomItemViewBase>();

                                for (int i = 0; i < bottomItems.Count(); i++) {
                                    if (!(bottomItems.ElementAt(i).BindingContext is ViewLessTabViewModel)) {
                                        bottomItems.ElementAt(i).IsSelected = (i == declarer.SelectedBottomItemIndex);
                                        bottomItems.ElementAt(i).AppropriateItemContentView.TranslationX = (bottomItems.ElementAt(i).IsSelected) ? 0 : short.MaxValue;
                                    }
                                }
                            }
                        }
                    }
                });     

        public ContentPageBaseView() {
            InitializeComponent();

            _popupBlockBackingTapGesture.Command = new Command(() => {
                IsPopupVisible = false;
            });
            _bottomItemTapGestureRecognizer.Tapped += OnBottomItemTapGestureRecognizerTapped;

            SetBinding(IsBusyAwaitingProperty, new Binding(BUSY_BINDING_PATH));
            SetBinding(IsPopupVisibleProperty, new Binding(IS_POPUP_VISIBLE_BINDING_PATH, BindingMode.TwoWay));
            SetBinding(IsPullToRefreshEnabledProperty, new Binding(IS_PULL_TO_REFRESH_ENABLED_BINDING_PATH, BindingMode.OneWay));
            SetBinding(IsRefreshingProperty, new Binding(IS_REFRESHING_BINDING_PATH, BindingMode.TwoWay));
            SetBinding(RefreshCommandProperty, new Binding(REFRESH_COMMAND_BINDING_PATH, BindingMode.OneWay));
            SetBinding(BottomBarItemsProperty, new Binding(nameof(BottomBarItems), BindingMode.OneWay));
            SetBinding(SelectedBottomItemIndexProperty, new Binding(nameof(SelectedBottomItemIndex), BindingMode.TwoWay));

            _popupsKeeper_PopupsBlockView.SetBinding(PopupsBlockView.PopupsProperty, new Binding(POPUPS_BINDING_PATH, mode: BindingMode.OneWay));
            _popupsKeeper_PopupsBlockView.Backing.GestureRecognizers.Add(_popupBlockBackingTapGesture);
        }

        public ICommand RefreshCommand {
            get { return (ICommand)GetValue(RefreshCommandProperty); }
            set { SetValue(RefreshCommandProperty, value); }
        }

        public IEnumerable<IBottomBarTab> BottomBarItems {
            get => (IEnumerable<IBottomBarTab>)GetValue(BottomBarItemsProperty);
            set => SetValue(BottomBarItemsProperty, value);
        }

        public int SelectedBottomItemIndex {
            get => (int)GetValue(SelectedBottomItemIndexProperty);
            set => SetValue(SelectedBottomItemIndexProperty, value);
        }

        public bool IsRefreshing {
            get => (bool)GetValue(IsRefreshingProperty);
            set => SetValue(IsRefreshingProperty, value);
        }

        public bool IsPullToRefreshEnabled {
            get => (bool)GetValue(IsPullToRefreshEnabledProperty);
            set => SetValue(IsPullToRefreshEnabledProperty, value);
        }

        public bool IsPopupVisible {
            get => (bool)GetValue(IsPopupVisibleProperty);
            set => SetValue(IsPopupVisibleProperty, value);
        }

        public View MainContent {
            get => (View)GetValue(MainContentProperty);
            set => SetValue(MainContentProperty, value);
        }

        public ActionBarBaseView ActionBar {
            get => (ActionBarBaseView)GetValue(ActionBarProperty);
            set => SetValue(ActionBarProperty, value);
        }

        public bool IsBusyAwaiting {
            get => (bool)GetValue(IsBusyAwaitingProperty);
            set => SetValue(IsBusyAwaitingProperty, value);
        }

        private void OnBottomItemTapGestureRecognizerTapped(object sender, EventArgs e) {
            object context = ((BottomItemViewBase)sender).BindingContext;

            if ((context is IBottomBarTab bottomBar) && bottomBar.RelativeViewType != null) {
                if (SelectedBottomItemIndex != ((SingleBottomItem)sender).TabIndex) {
                    SelectedBottomItemIndex = ((SingleBottomItem)sender).TabIndex;
                }
            } else {
                if (context is IActionBottomBarTab actionBottomBarTab) {
                    actionBottomBarTab.TabActionCommand?.Execute(null);
                }
            }
        }
    }
}