using beyond.park.client.Models.Arguments.BottomTabs;
using beyond.park.client.ViewModels.Base.Contracts;
using Microsoft.AppCenter.Crashes;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms.Internals;

namespace beyond.park.client.ViewModels.Base {
    public class ContentPageBaseViewModel : ViewModelBase {

        private readonly Dictionary<Guid, bool> _busySequence = new Dictionary<Guid, bool>();

        List<IBottomBarTab> _bottomBarItems;
        public List<IBottomBarTab> BottomBarItems {
            get => _bottomBarItems;
            protected set {
                _bottomBarItems?.ForEach(bottomItem => bottomItem.Dispose());
                SetProperty(ref _bottomBarItems, value);
            }
        }

        ObservableCollection<PopupBaseViewModel> _popups = new ObservableCollection<PopupBaseViewModel>();
        public ObservableCollection<PopupBaseViewModel> Popups {
            get => _popups;
            private set => SetProperty(ref _popups, value);
        }

        int _electedBottomItemIndex;
        public int SelectedBottomItemIndex {
            get => _electedBottomItemIndex;
            set {
                try {                   
                    BottomBarItems?[value].InitializeAsync(new SelectedBottomBarTabArgs());

                    if (BottomBarItems != null && BottomBarItems.Any()) {
                        SomeBottomTabWasSelectedArgs someBottomTabWasSelectedArgs = new SomeBottomTabWasSelectedArgs(BottomBarItems?[value].GetType());
                        BottomBarItems.ForEach(barItem => barItem.InitializeAsync(someBottomTabWasSelectedArgs));
                    }
                } catch (Exception ex) {
                    Crashes.TrackError(ex);
                    Debugger.Break();
                }

                SetProperty(ref _electedBottomItemIndex, value);
            }
        }

        bool _isPullToRefreshEnabled;
        public bool IsPullToRefreshEnabled {
            get => _isPullToRefreshEnabled;
            protected set => SetProperty(ref _isPullToRefreshEnabled, value);
        }

        bool _isRefreshing;
        public bool IsRefreshing {
            get => _isRefreshing;
            set => SetProperty(ref _isRefreshing, value);
        }

        bool _isPopupsVisible;
        public bool IsPopupsVisible {
            get => _isPopupsVisible;
            set => SetProperty<bool>(ref _isPopupsVisible, value);
        }

        ActionBarBaseViewModel _actionBarViewModel;
        public ActionBarBaseViewModel ActionBarViewModel {
            get => _actionBarViewModel;
            protected set {
                _actionBarViewModel?.Dispose();
                SetProperty(ref _actionBarViewModel, value);
            }
        }

        public override void Dispose() {
            base.Dispose();

            ActionBarViewModel?.Dispose();
            Popups?.ForEach(popup => popup.Dispose());
        }

        public override Task InitializeAsync(object navigationData) {

            ActionBarViewModel?.InitializeAsync(navigationData);

            return base.InitializeAsync(navigationData);
        }

        public void SetBusy(Guid guidKey, bool isBusy) {
            if (_busySequence.ContainsKey(guidKey)) {
                _busySequence[guidKey] = isBusy;
            } else {
                _busySequence.Add(guidKey, isBusy);
            }

            _busySequence.Where(kVP => !kVP.Value).Select(kVP => kVP.Key).ToArray().ForEach(g => _busySequence.Remove(g));

            IsBusy = _busySequence.Any();
        }
    }
}
