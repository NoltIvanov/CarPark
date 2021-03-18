using beyond.park.client.Builders.DataItems.TourItems;
using beyond.park.client.Extensions;
using beyond.park.client.Models.Registration;
using beyond.park.client.ViewModels.Base;
using beyond.park.client.ViewModels.Registration;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace beyond.park.client.ViewModels {
    public sealed class GuideViewModel : ContentPageBaseViewModel {

        private const int ZERO_INDEX = default(int);

        private readonly ITourItemBuilder _tourItemBuilder;

        ObservableCollection<TourItem> _screens;
        public ObservableCollection<TourItem> Screens {
            get => _screens;
            set => SetProperty(ref _screens, value);
        }

        int _position;
        public int Position {
            get => _position;
            set {
                SetProperty(ref _position, value);
                CheckButtonStatus(value);
                UpdateTourInfo(value);
            }
        }

        string _title;
        public string Title {
            get => _title;
            set => SetProperty(ref _title, value);
        }

        string _description;
        public string Description {
            get => _description;
            set => SetProperty(ref _description, value);
        }

        bool _isShownButton;
        public bool IsShownButton {
            get => _isShownButton;
            set => SetProperty(ref _isShownButton, value);
        }

        public ICommand RegisterCommand => new Command(async () => await OnRegister());

        public ICommand NextCommand => new Command(() => OnNext());

        /// <summary>
        ///     ctor().
        /// </summary>
        public GuideViewModel(ITourItemBuilder tourItemBuilder) {
            _tourItemBuilder = tourItemBuilder;  
        }

        public override void Dispose() {           
            Screens?.Clear();
        }

        public override Task InitializeAsync(object navigationData) {

            GetItems();
            UpdateTourInfo(ZERO_INDEX);

            return base.InitializeAsync(navigationData);
        }

        private void GetItems() {
            Screens = _tourItemBuilder?.BuildItems()?.ToObservableCollection();
        }

        private void OnNext() {
            if (_position == Screens?.Count - 1) {
                Position = default(int);
            } else {
                Position = _position + 1;
            }
        }

        private void CheckButtonStatus(int index) {
            if (index == Screens?.Count - 1) {
                IsShownButton = true;
            } else {
                IsShownButton = false;
            }
        }

        private void UpdateTourInfo(int index) {
            if (Screens != null && Screens.Any()) {
                var currentInfo = Screens.ElementAt(index);
                Title = currentInfo.Title;
                Description = currentInfo.Description;
            }
        }

        private async Task OnRegister() {
            await NavigationService.RemoveBackStackAsync();
            await NavigationService.FirstInitilizeAsync<LogInDetailsViewModel>();
        }
    }
}
