using Android.Content;
using beyond.park.client.Droid.Renderers;
using beyond.park.client.Views;
using Xamarin.Forms;

[assembly: ExportRenderer(typeof(MainView), typeof(MainViewPageRenderer))]
namespace beyond.park.client.Droid.Renderers {
    public sealed class MainViewPageRenderer : ContentPageRenderer {
        public MainViewPageRenderer(Context context) : base(context) { }
    }
}