using beyond.park.client.Models.EventMessages;
using beyond.park.client.ViewModels.Base;
using System.Windows.Input;
using Xamarin.Forms;

namespace beyond.park.client.ViewModels.Items {
    public sealed class FilterItemViewModel : ViewModelBase {
        bool _isSelected;
        public bool IsSelected {
            get => _isSelected;
            set => SetProperty(ref _isSelected, value);
        }

        string _title;
        public string Title {
            get => _title;
            set => SetProperty(ref _title, value);
        }

        public ICommand SelectedFilterCommand => new Command(() => OnSelectedFilter());

        private void OnSelectedFilter() {
            IsSelected = true;
            PubSub.Hub.Default.Publish(new FilterMessage { Filter = Title });
        }
    }
}
