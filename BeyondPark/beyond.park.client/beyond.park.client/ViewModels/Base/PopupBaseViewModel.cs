using beyond.park.client.Controls.Popups;
using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace beyond.park.client.ViewModels.Base {
    public abstract class PopupBaseViewModel : NestedViewModel, IPopupContext {

        private string _title;
        public string Title {
            get => _title;
            set => SetProperty(ref _title, value);
        }

        bool _isPopupVisible;
        public bool IsPopupVisible {
            get => _isPopupVisible;
            set {
                SetProperty(ref _isPopupVisible, value);

                OnIsPopupVisible();
            }
        }

        public ICommand ShowPopupCommand => new Command((object param) => {
            OnShowPopupCommand(param);
            UpdatePopupScopeVisibility(true);
            IsPopupVisible = true;
        });

        public ICommand ClosePopupCommand => new Command((object param) => {
            OnClosePopupCommand(param);
            UpdatePopupScopeVisibility(false);
        });

        public abstract Type RelativeViewType { get; }

        public override Task InitializeAsync(object navigationData) {
            if (navigationData is ContentPageBaseViewModel pageBaseViewModel) {
                if (!pageBaseViewModel.Popups.Contains(this)) {
                    pageBaseViewModel.Popups.Add(this);
                }
            }

            return base.InitializeAsync(navigationData);
        }

        protected virtual void OnIsPopupVisible() { }

        protected virtual void OnShowPopupCommand(object param) { }

        protected virtual void OnClosePopupCommand(object param) { }
    }
}
