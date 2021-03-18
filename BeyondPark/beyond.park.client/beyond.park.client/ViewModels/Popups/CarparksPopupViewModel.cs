using beyond.park.client.Extensions;
using beyond.park.client.Models.Rest.Carpark;
using beyond.park.client.ViewModels.Base;
using beyond.park.client.ViewModels.Items;
using beyond.park.client.Views.Popups;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;

namespace beyond.park.client.ViewModels.Popups {
    public sealed class CarparksPopupViewModel : PopupBaseViewModel {

        public override Type RelativeViewType => typeof(CarparksPopupView);

        ObservableCollection<CarparkItemViewModel> _carparkItemViewModels;
        public ObservableCollection<CarparkItemViewModel> CarparkItemViewModels {
            get => _carparkItemViewModels;
            set => SetProperty(ref _carparkItemViewModels, value);
        }

        /// <summary>
        ///     ctor().
        /// </summary>
        public CarparksPopupViewModel() {

        }

        public override Task InitializeAsync(object navigationData) {

            if (navigationData is List<CarparkBody> items) {
                CarparkItemViewModels = MapData(items);
            }

            return base.InitializeAsync(navigationData);
        }
     
        public override void Dispose() {
            base.Dispose();

            CarparkItemViewModels?.Clear();
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
    }
}
