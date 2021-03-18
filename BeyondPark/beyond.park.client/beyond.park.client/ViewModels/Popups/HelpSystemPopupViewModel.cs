using beyond.park.client.Builders.DataItems.CarparkItems;
using beyond.park.client.Builders.DataItems.FilterItems;
using beyond.park.client.Builders.DataItems.HelpItems;
using beyond.park.client.Extensions;
using beyond.park.client.Models.HelpSystem;
using beyond.park.client.Models.Rest.Carpark;
using beyond.park.client.ViewModels.Base;
using beyond.park.client.ViewModels.Items;
using beyond.park.client.Views.Popups;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace beyond.park.client.ViewModels.Popups {
    public sealed class HelpSystemPopupViewModel : PopupBaseViewModel {

        private LinkedList<HelpItem> _helpItems = null;

        private readonly IHelpItemBuilder _helpItemBuilder;

        private readonly IFilterItemBuilder _filterItemBuilder;

        private readonly ICarparkItemBuilder _carparkItemBuilder;

        public override Type RelativeViewType => typeof(HelpSystemPopupView);

        LinkedListNode<HelpItem> _currentHelpItem;
        public LinkedListNode<HelpItem> CurrentHelpItem {
            get => _currentHelpItem;
            set => SetProperty(ref _currentHelpItem, value);
        }

        bool _closestLocationPopupShown;
        public bool ClosestLocationPopupShown {
            get => _closestLocationPopupShown;
            set => SetProperty(ref _closestLocationPopupShown, value);
        }

        bool _firstPopupShown;
        public bool FirstPopupShown {
            get => _firstPopupShown;
            set => SetProperty(ref _firstPopupShown, value);
        }

        bool _filterPopupShown;
        public bool FilterPopupShown {
            get => _filterPopupShown;
            set => SetProperty(ref _filterPopupShown, value);
        }

        bool _reroutePopupShown;
        public bool ReroutePopupShown {
            get => _reroutePopupShown;
            set => SetProperty(ref _reroutePopupShown, value);
        }

        bool _mapPinPopupShown;
        public bool MapPinPopupShown {
            get => _mapPinPopupShown;
            set => SetProperty(ref _mapPinPopupShown, value);
        }

        bool _moreTipsPopupShown;
        public bool MoreTipsPopupShown {
            get => _moreTipsPopupShown;
            set => SetProperty(ref _moreTipsPopupShown, value);
        }

        ObservableCollection<FilterItemViewModel> _filterItemViewModels;
        public ObservableCollection<FilterItemViewModel> FilterItemViewModels {
            get => _filterItemViewModels;
            set => SetProperty(ref _filterItemViewModels, value);
        }

        ObservableCollection<CarparkItemViewModel> _carparkItemViewModels;
        public ObservableCollection<CarparkItemViewModel> CarparkItemViewModels {
            get => _carparkItemViewModels;
            set => SetProperty(ref _carparkItemViewModels, value);
        }

        ObservableCollection<CarparkItemViewModel> _carparkItemViewModelsCopy;
        public ObservableCollection<CarparkItemViewModel> CarparkItemViewModelsCopy {
            get => _carparkItemViewModelsCopy;
            set => SetProperty(ref _carparkItemViewModelsCopy, value);
        }

        public ICommand NextHelpCommand => new Command(() => OnNextHelp());

        public ICommand BackHelpCommand => new Command(() => OnBackHelp());

        /// <summary>
        ///     ctor().
        /// </summary>
        /// <param name="filterItemBuilder"></param>
        public HelpSystemPopupViewModel(IFilterItemBuilder filterItemBuilder, ICarparkItemBuilder carparkItemBuilder, IHelpItemBuilder helpItemBuilder) {
            _helpItemBuilder = helpItemBuilder;
            _filterItemBuilder = filterItemBuilder;
            _carparkItemBuilder = carparkItemBuilder;
        }

        public override Task InitializeAsync(object navigationData) {

            GetData();

            return base.InitializeAsync(navigationData);
        }

        public override void Dispose() {
            base.Dispose();

            FilterItemViewModels?.Clear();
            CarparkItemViewModels?.Clear();
            CarparkItemViewModelsCopy?.Clear();
            CurrentHelpItem = null;
            FilterItemViewModels = null;
            CarparkItemViewModels = null;
            CarparkItemViewModelsCopy = null;
        }

        private void GetData() {
            if (FilterItemViewModels == null && CarparkItemViewModels == null && CurrentHelpItem == null) {
                FilterItemViewModels = _filterItemBuilder.BuildItems().ToObservableCollection();
                SelectSomeItem();

                // For ClosestLocation popup.
                var carParkItems = _carparkItemBuilder.BuildItems();
                CarparkItemViewModels = MapData(carParkItems);

                // For Rerote popup.
                carParkItems.Remove(carParkItems.Last());
                CarparkItemViewModelsCopy = MapData(carParkItems);

                _helpItems = _helpItemBuilder.BuildItems();

                SetFirstHelpItem();
            } else {
                UpdateTip(HelpPopups.HideAll);

                SetFirstHelpItem();
            }
        }

        private void SetFirstHelpItem() {
            CurrentHelpItem = _helpItems.First;
            FirstPopupShown = true;
        }

        private void SelectSomeItem() {
            var someFilter = FilterItemViewModels.First();
            someFilter.IsSelected = true;
        }

        private void OnNextHelp() {
            CurrentHelpItem = CurrentHelpItem.Next ?? CurrentHelpItem;
            UpdateTip(CurrentHelpItem.Value.HelpPopup);
        }

        private void OnBackHelp() {
            CurrentHelpItem = CurrentHelpItem.Previous ?? CurrentHelpItem;
            UpdateTip(CurrentHelpItem.Value.HelpPopup);
        }

        private ObservableCollection<CarparkItemViewModel> MapData(List<CarparkBody> items) {
            ObservableCollection<CarparkItemViewModel> viewModels = new ObservableCollection<CarparkItemViewModel>();
            foreach (var item in items) {
                CarparkItemViewModel carparkItemViewModel = new CarparkItemViewModel {
                    Name = item.Name,
                    Address = item.Address,
                    Distance = item.Distance,
                    Fee = item.Fee
                };
                viewModels.Add(carparkItemViewModel);
            }
            return viewModels;
        }

        private void UpdateTip(HelpPopups helpPopup) {
            switch (helpPopup) {
                case HelpPopups.First:
                    FirstPopupShown = true;
                    ClosestLocationPopupShown = false;
                    FilterPopupShown = false;
                    ReroutePopupShown = false;
                    MapPinPopupShown = false;
                    MoreTipsPopupShown = false;
                    break;
                case HelpPopups.ClosestLocation:
                    ClosestLocationPopupShown = true;
                    FirstPopupShown = false;
                    FilterPopupShown = false;
                    ReroutePopupShown = false;
                    MapPinPopupShown = false;
                    MoreTipsPopupShown = false;
                    break;
                case HelpPopups.Filter:
                    FilterPopupShown = true;
                    FirstPopupShown = false;
                    ClosestLocationPopupShown = false;
                    ReroutePopupShown = false;
                    MoreTipsPopupShown = false;
                    break;
                case HelpPopups.Reroute:
                    ReroutePopupShown = true;
                    FirstPopupShown = false;
                    ClosestLocationPopupShown = false;
                    FilterPopupShown = false;
                    MapPinPopupShown = false;
                    MoreTipsPopupShown = false;
                    break;
                case HelpPopups.MapPin:
                    MapPinPopupShown = true;
                    FirstPopupShown = false;
                    ClosestLocationPopupShown = false;
                    FilterPopupShown = false;
                    ReroutePopupShown = false;
                    MoreTipsPopupShown = false;
                    break;
                case HelpPopups.MoreTips:
                    MoreTipsPopupShown = true;
                    FirstPopupShown = false;
                    ClosestLocationPopupShown = false;
                    FilterPopupShown = false;
                    ReroutePopupShown = false;
                    MapPinPopupShown = false;
                    break;
                case HelpPopups.HideAll:
                    ClosestLocationPopupShown = false;
                    FilterPopupShown = false;
                    ReroutePopupShown = false;
                    MapPinPopupShown = false;
                    MoreTipsPopupShown = false;
                    FirstPopupShown = false;
                    break;
                default:
                    break;
            }
        }
    }
}
