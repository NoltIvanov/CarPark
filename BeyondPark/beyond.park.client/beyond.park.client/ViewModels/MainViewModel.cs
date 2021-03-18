using beyond.park.client.Builders.DataItems.CarparkItems;
using beyond.park.client.Builders.DataItems.FilterItems;
using beyond.park.client.Extensions;
using beyond.park.client.Models.EventMessages;
using beyond.park.client.Models.Rest.Carpark;
using beyond.park.client.Services.GoogleMap;
using beyond.park.client.Services.Permission;
using beyond.park.client.ViewModels.ActionBars;
using beyond.park.client.ViewModels.Base;
using beyond.park.client.ViewModels.Items;
using beyond.park.client.ViewModels.Popups;
using FFImageLoading.Svg.Forms;
using Microsoft.AppCenter.Crashes;
using PubSub;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reactive.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.GoogleMaps;
using Xamarin.Forms.GoogleMaps.Bindings;
using static Xamarin.Essentials.Permissions;

namespace beyond.park.client.ViewModels {
    public sealed class MainViewModel : ContentPageBaseViewModel {

        private CancellationTokenSource _searchCarParsCancellationTokenSource = new CancellationTokenSource();

        private readonly IFilterItemBuilder _filterItemBuilder;

        private readonly ICarparkItemBuilder _carparkItemBuilder;

        private readonly IPermissionService _permissionService;

        private readonly IGoogleMapService _googleMapService;

        string _targetValue;
        public string TargetValue {
            get { return _targetValue; }
            set { SetProperty(ref _targetValue, value); }
        }

        MoveToRegionRequest _moveToRegionRequest = new MoveToRegionRequest();
        public MoveToRegionRequest MoveToRegionRequest {
            get => _moveToRegionRequest;
            set => SetProperty(ref _moveToRegionRequest, value);
        }

        MapSpan _visibleRegion;
        public MapSpan VisibleRegion {
            get => _visibleRegion;
            set => SetProperty(ref _visibleRegion, value);
        }

        CarparksPopupViewModel _carparksPopupViewModel;
        public CarparksPopupViewModel CarparksPopupViewModel {
            get => _carparksPopupViewModel;
            set {
                _carparksPopupViewModel?.Dispose();
                SetProperty(ref _carparksPopupViewModel, value);
            }
        }

        HelpSystemPopupViewModel _helpSystemPopupViewModel;
        public HelpSystemPopupViewModel HelpSystemPopupViewModel {
            get => _helpSystemPopupViewModel;
            set {
                _helpSystemPopupViewModel?.Dispose();
                SetProperty(ref _helpSystemPopupViewModel, value);
            }
        }

        NotSignUpPopupViewModel _notSignUpPopupViewModel;
        public NotSignUpPopupViewModel NotSignUpPopupViewModel {
            get => _notSignUpPopupViewModel;
            set {
                _notSignUpPopupViewModel?.Dispose();
                SetProperty(ref _notSignUpPopupViewModel, value);
            }
        }

        ObservableCollection<Pin> _pins = new ObservableCollection<Pin>();
        public ObservableCollection<Pin> Pins {
            get => _pins;
            set => SetProperty(ref _pins, value);
        }

        ObservableCollection<FilterItemViewModel> _filterItemViewModels;
        public ObservableCollection<FilterItemViewModel> FilterItemViewModels {
            get => _filterItemViewModels;
            set => SetProperty(ref _filterItemViewModels, value);
        }

        public ICommand SearchCommand => new Command(async () => await OnSearch());

        /// <summary>
        ///     ctor().
        /// </summary>
        public MainViewModel(IFilterItemBuilder filterItemBuilder,
                             ICarparkItemBuilder carparkItemBuilder,
                             IPermissionService permissionService,
                             IGoogleMapService googleMapService) {

            _filterItemBuilder = filterItemBuilder;
            _carparkItemBuilder = carparkItemBuilder;
            _permissionService = permissionService;
            _googleMapService = googleMapService;

            //CheckPermission();

            ActionBarViewModel = DependencyLocator.Resolve<CommonActionBarViewModel>();
            ((CommonActionBarViewModel)ActionBarViewModel).InitializeAsync(this);

            CarparksPopupViewModel = DependencyLocator.Resolve<CarparksPopupViewModel>();
            CarparksPopupViewModel.InitializeAsync(this);

            HelpSystemPopupViewModel = DependencyLocator.Resolve<HelpSystemPopupViewModel>();
            HelpSystemPopupViewModel.InitializeAsync(this);

            NotSignUpPopupViewModel = DependencyLocator.Resolve<NotSignUpPopupViewModel>();
            NotSignUpPopupViewModel.InitializeAsync(this);

            GetItems();
        }

        public override void Dispose() {
            base.Dispose();

            ResetCancellationTokenSource(ref _searchCarParsCancellationTokenSource);
            HelpSystemPopupViewModel?.Dispose();
            CarparksPopupViewModel?.Dispose();
            FilterItemViewModels?.Clear();
        }

        protected override void OnSubscribeOnAppEvents() {
            base.OnSubscribeOnAppEvents();

            Hub.Default.Subscribe<FilterMessage>(SelectedFilter);
            Hub.Default.Subscribe<HelpSystemMessage>(OpenHelpSystem);
            Hub.Default.Subscribe<NotSignUpMessage>(OpenNotSignUpInfo);
        }

        protected override void OnUnsubscribeFromAppEvents() {
            base.OnUnsubscribeFromAppEvents();

            Hub.Default.Unsubscribe<FilterMessage>(SelectedFilter);
            Hub.Default.Unsubscribe<HelpSystemMessage>(OpenHelpSystem);
            Hub.Default.Unsubscribe<NotSignUpMessage>(OpenNotSignUpInfo);
        }

        public override Task InitializeAsync(object navigationData) {
            CarparksPopupViewModel?.InitializeAsync(navigationData);
            HelpSystemPopupViewModel?.InitializeAsync(navigationData);
            NotSignUpPopupViewModel?.InitializeAsync(navigationData);

            UpdateVisibleRegion();
            GetCarparks();

            return base.InitializeAsync(navigationData);
        }

        private void UpdateVisibleRegion() {
            VisibleRegion = MapSpan.FromCenterAndRadius(new Position(-37.6895924, 144.8754449), Distance.FromMiles(0.25));

            MoveToRegionRequest.MoveToRegion(VisibleRegion);
        }

        private async void GetCarparks() {
            try {
                Assembly assembly = GetType().GetTypeInfo().Assembly;
                Stream stream = assembly.GetManifestResourceStream("beyond.park.client.Resources.Images.star.png");

                Pins.Add(new Pin {
                    Type = PinType.Place,
                    Address = "GardenDrive, Melbourne",
                    Label = "GardenDrive, Melbourne",
                    Position = new Position(-37.6895924, 144.8754449),
                    Icon = BitmapDescriptorFactory.FromStream(stream)
                });
                Pins.Add(new Pin {
                    Type = PinType.Place,
                    Address = "Tullamarine, Melbourne",
                    Label = "Tullamarine, Melbourne",
                    Position = new Position(-37.6806924, 144.8754449),
                    Icon = BitmapDescriptorFactory.FromStream(stream)
                });

                var tt = await _googleMapService.GetDirections("-37.6895924", "144.8754449", "-37.6806924", "144.8754449");
            } catch (Exception ex) {
                Debug.WriteLine($"ERROR: {ex.Message}");
                Debugger.Break();

                Crashes.TrackError(ex);

            }
        }

        private void GetItems() {
            FilterItemViewModels = _filterItemBuilder.BuildItems().ToObservableCollection();
        }

        private async Task<IEnumerable<object>> SearchInfoAsync(string value, object cancellsationToken = default(object)) {

            IEnumerable<object> carParks = null;

            try {
                await Task.Delay(1000);
                //var userResult = await _identityService.GetUsersAsync(paginationModel, (CancellationToken)cancellationToken);

                //if (userResult != null) {
                //    users = userResult.Body;
                //}
                return _carparkItemBuilder.BuildItems();
            } catch (Exception ex) {
                Debug.WriteLine($"ERROR: {ex.Message}");
                Debugger.Break();

                Crashes.TrackError(ex);

                carParks = new object[] { };
            }

            return carParks;
        }

        private async void ApplySearchResults(IEnumerable<object> foundResult) {
            try {
                if (foundResult.Any()) {
                    CarparksPopupViewModel.ShowPopupCommand.Execute(null);
                    await CarparksPopupViewModel.InitializeAsync(foundResult);
                }
            } catch (Exception ex) {
                Debug.WriteLine($"ERROR: {ex.Message}");
                Debugger.Break();
            }
        }

        private void SelectedFilter(FilterMessage filterMessage) {
            foreach (var itemViewModel in FilterItemViewModels) {
                if (itemViewModel.Title != filterMessage.Filter) {
                    itemViewModel.IsSelected = false;
                }
            }

            OnSearch().IgnoreAwait();
        }

        private async Task OnSearch() {
            ResetCancellationTokenSource(ref _searchCarParsCancellationTokenSource);
            CancellationTokenSource cancellationTokenSource = _searchCarParsCancellationTokenSource;

            var result = await SearchInfoAsync(TargetValue, cancellationTokenSource.Token).ConfigureAwait(false);

            if (result != null) {
                ApplySearchResults(result);
            }
        }

        private async void OpenHelpSystem(HelpSystemMessage helpSystemMessage) {
            HelpSystemPopupViewModel.ShowPopupCommand.Execute(null);
            await HelpSystemPopupViewModel.InitializeAsync(null);
        }

        private async void OpenNotSignUpInfo(NotSignUpMessage message) {
            NotSignUpPopupViewModel.ShowPopupCommand.Execute(null);
            await NotSignUpPopupViewModel.InitializeAsync(null);
        }

        private async void CheckPermission() {
            try {
                BasePlatformPermission locationPermission = null;
                if (Device.RuntimePlatform == Device.iOS) {
                    locationPermission = new Permissions.LocationWhenInUse();
                } else if (Device.RuntimePlatform == Device.Android) {
                    locationPermission = new Permissions.LocationWhenInUse();
                }

                PermissionStatus status = await _permissionService.CheckAndRequestPermissionAsync(locationPermission);
                if (status != PermissionStatus.Granted) {
                    throw new PermissionException("Please allow access in your device settings.");
                }

            } catch (Exception ex) {
                Debug.WriteLine($"ERROR: {ex.Message}");
                Debugger.Break();
                Crashes.TrackError(ex);
            }
        }
    }
}
