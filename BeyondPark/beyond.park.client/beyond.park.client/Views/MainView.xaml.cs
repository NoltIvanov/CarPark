using beyond.park.client.Models.EventMessages;
using beyond.park.client.ViewModels;
using beyond.park.client.ViewModels.Base;
using beyond.park.client.Views.Base;
using SlideOverKit;
using System;

namespace beyond.park.client.Views {

    public partial class MainView : ContentPageBaseView, IMenuContainerPage, IDisposable {
        private SideMenuView _sideMenuView = new SideMenuView();

        public SlideMenuView SlideMenu { get; set; }

        public Action ShowMenuAction { get; set; }

        public Action HideMenuAction { get; set; }

        public MainView() {
            InitializeComponent();

            SlideMenu = _sideMenuView;

            PubSub.Hub.Default.Subscribe<MenuMessage>(OnOpenMenu);
        }

        protected override void OnBindingContextChanged() {
            base.OnBindingContextChanged();

            _sideMenuView.BindingContext = DependencyLocator.Resolve<SideMenuViewModel>();
        }

        private void OnOpenMenu(MenuMessage message) {
            if (SlideMenu.IsShown) {
                HideMenuAction?.Invoke();
            } else {
                ShowMenuAction?.Invoke();
            }
        }

        public void Dispose() {
            PubSub.Hub.Default.Unsubscribe<MenuMessage>(OnOpenMenu);
        }
    }
}