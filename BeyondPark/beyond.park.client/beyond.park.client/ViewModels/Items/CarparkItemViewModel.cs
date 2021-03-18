using beyond.park.client.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace beyond.park.client.ViewModels.Items {
    public sealed class CarparkItemViewModel : ViewModelBase {

        string _name;
        public string Name {
            get => _name;
            set => SetProperty(ref _name, value);
        }

        string _address;
        public string Address {
            get => _address;
            set => SetProperty(ref _address, value);
        }

        int _distance;
        public int Distance {
            get => _distance;
            set => SetProperty(ref _distance, value);
        }

        decimal _fee;
        public decimal Fee {
            get => _fee;
            set => SetProperty(ref _fee, value);
        }

        public ICommand RouteCommand => new Command(() => OnRoute());

        /// <summary>
        ///     ctor().
        /// </summary>
        public CarparkItemViewModel() {

        }

        private void OnRoute() {
            
        }
    }
}
