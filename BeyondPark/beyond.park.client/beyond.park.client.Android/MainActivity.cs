using Acr.UserDialogs;
using AiForms.Effects.Droid;
using Android;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using FFImageLoading.Forms.Platform;
using Plugin.PayCards;
using Xamarin;

namespace beyond.park.client.Droid {
    [Activity(Label = "Beyond Park", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity {

        private const int REQUEST_LOCATION_ID = 222;

        private readonly string[] _locationPermissions =
        {
            Manifest.Permission.AccessCoarseLocation,
            Manifest.Permission.AccessFineLocation               
        };

        internal static MainActivity Instance { get; private set; }

        protected override void OnCreate(Bundle savedInstanceState) {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(savedInstanceState);

            Instance = this;

            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);

            FormsGoogleMaps.Init(this, savedInstanceState); // Google map.
            FormsGoogleMapsBindings.Init();
            PayCardsRecognizerService.Initialize(this);
            CachedImageRenderer.Init(false);
            UserDialogs.Init(this);
            Effects.Init();         

            LoadApplication(new App());
        }

        protected override void OnStart() {
            base.OnStart();

            if ((int)Build.VERSION.SdkInt >= 23) {
                if (CheckSelfPermission(Manifest.Permission.AccessFineLocation) != Permission.Granted) {
                    RequestPermissions(_locationPermissions, REQUEST_LOCATION_ID);
                } else {
                    // Permissions already granted - display a message.
                }
            }
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults) {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data) {
            base.OnActivityResult(requestCode, resultCode, data);

            PayCardsRecognizerService.OnActivityResult(requestCode, resultCode, data);
        }
    }
}