using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;

namespace beyond.park.client.ViewModels.Base {
    public abstract class NestedViewModel : ViewModelBase {

        protected void UpdatePopupScopeVisibility(bool isPopupVisible) {
            if (IsThisInVisualScope()) {
                ((ContentPageBaseViewModel)NavigationService.LastPageViewModel).IsPopupsVisible = isPopupVisible;
            }
        }

        protected void UpdateBusyVisualState(bool isBusy) {
            if (IsThisInVisualScope()) {
                ((ContentPageBaseViewModel)NavigationService.LastPageViewModel).IsBusy = isBusy;
            }
        }

        protected void UpdateBusyVisualState(Guid busyKey, bool isBusy) {
            if (IsThisInVisualScope()) {
                ((ContentPageBaseViewModel)NavigationService.LastPageViewModel).SetBusy(busyKey, isBusy);
            }
        }

        protected void UpdateRefreshingVisualState(bool isRefreshing) {
            if (IsThisInVisualScope()) {
                ((ContentPageBaseViewModel)NavigationService.LastPageViewModel).IsRefreshing = isRefreshing;
            }
        }

        protected async void ExecuteActionWithBusy(Func<Task> asyncFunc) {
            if (asyncFunc == null) {
                Debugger.Break();
                return;
            }

            Guid busyKey = Guid.NewGuid();
            UpdateBusyVisualState(busyKey, true);

            await asyncFunc();

            UpdateBusyVisualState(busyKey, false);
        }

        private bool IsThisInVisualScope() {
            if (NavigationService.LastPageViewModel != null && NavigationService.LastPageViewModel is ContentPageBaseViewModel contentPageBaseViewModel) {
                return true;
                //if (contentPageBaseViewModel.BottomModeBarViewModel != null
                //    && contentPageBaseViewModel.BottomModeBarViewModel.IsBottomBarEnabled
                //    && contentPageBaseViewModel.BottomModeBarViewModel.BottomBarItems.ElementAt(contentPageBaseViewModel.BottomModeBarViewModel.SelectedTabIndex) == this) {
                //}
            }

            return false;
        }
    }
}
