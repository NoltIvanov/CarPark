using beyond.park.client.Common;
using beyond.park.client.Helpers;
using beyond.park.client.ViewModels.Base;
using System.Runtime.Serialization;
using Xamarin.Essentials;

namespace beyond.park.client.Models.Identities {
    [DataContract]
    public class UserProfile : ExtendedBindableObject {

        [DataMember]
        public string AccesToken { get; set; } = string.Empty;

        [DataMember]
        public string RefreshToken { get; set; } = string.Empty;

        [DataMember]
        public bool IsAuth { get; set; }

        [DataMember]
        public string Email { get; set; } = string.Empty;

        [DataMember]
        public string NetUid { get; set; }

        [DataMember]
        public long Id { get; set; }

        [DataMember]
        public string PhoneNumber { get; set; }

        [DataMember]
        public string UserName { get; set; }

        [DataMember]
        public string AvatarUrl { get; set; }     

        [DataMember]
        public string RoleDescription { get; set; }      

        internal void ClearUserProfile() {            
            RoleDescription = string.Empty;
            RefreshToken = string.Empty;
            AccesToken = string.Empty;
            AvatarUrl = string.Empty;
            UserName = string.Empty;
            NetUid = string.Empty;
            Email = string.Empty;
            Id = default(long);
            IsAuth = false;          
        }

        internal void SaveChanges() {
            Preferences.Set(ApplicationConsts.USER_PROFILE, JsonSerializerHelper.SerializeObject<UserProfile>(this));
        }
    }
}
