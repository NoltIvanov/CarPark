using System.Threading.Tasks;

namespace beyond.park.client.ViewModels.Base {
    public abstract class ActionBarBaseViewModel : NestedViewModel {
        string _userName;
        public string UserName {
            get => _userName;
            set => SetProperty(ref _userName, value);
        }

        string _roleDescription;
        public string RoleDescription {
            get => _roleDescription;
            set => SetProperty(ref _roleDescription, value);
        }

        string _companyName;
        public string CompanyName {
            get => _companyName;
            set => SetProperty(ref _companyName, value);
        }

        public override Task InitializeAsync(object navigationData) {
            SetProfileInfo();

            return base.InitializeAsync(navigationData);
        }

        private void SetProfileInfo() {
            UserName = BaseSingleton<GlobalSetting>.Instance.UserProfile?.UserName?.ToUpper();
            RoleDescription = BaseSingleton<GlobalSetting>.Instance.UserProfile?.RoleDescription?.ToUpper();            
        }
    }
}
