using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
[assembly: ExportFont("icomoon.ttf", Alias = "icommon")]
[assembly: ExportFont("BwModelicaCyrillicDEMO-Bold.ttf", Alias = "ModelicaBold")]
[assembly: ExportFont("BwModelicaCyrillicDEMO-ExtraBold.ttf", Alias = "ModelicaExtraBold")]
[assembly: ExportFont("BwModelicaCyrillicDEMO-Medium.ttf", Alias = "ModelicaMedium")]


namespace ThemeGallery {
    static class IconFont {
        public const string Circle = "\uea56";
        

    }
}