using beyond.park.client.iOS.Renderers;
using beyond.park.client.Views;
using Xamarin.Forms;

[assembly: ExportRenderer(typeof(MainView), typeof(MainViewPageRenderer))]
namespace beyond.park.client.iOS.Renderers {
    public sealed class MainViewPageRenderer : ContentPageRenderer { }
}