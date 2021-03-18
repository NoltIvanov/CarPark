using beyond.park.client.Common;
using beyond.park.client.Helpers;
using beyond.park.client.Models.Identities;
using beyond.park.client.Models.Rest.EndPoints;
using Xamarin.Essentials;

namespace beyond.park.client {
    public sealed class GlobalSetting {
        public UserProfile UserProfile { get; private set; }

        public RestEndpoints RestEndpoints { get; private set; } = new RestEndpoints();

        //public AppLanguage LanguageInterface { get; set; } = new English();

        /// <summary>
        ///     ctor().
        /// </summary>
        public GlobalSetting() {
            string jsonUserProfile = Preferences.Get(ApplicationConsts.USER_PROFILE, string.Empty);
            UserProfile = string.IsNullOrEmpty(jsonUserProfile) ? new UserProfile() : JsonSerializerHelper.Deserialize<UserProfile>(jsonUserProfile);
        }
    }
}
