using beyond.park.client.iOS.Services;
using beyond.park.client.Services.Platform.Contracts;
using Foundation;
using UIKit;
using Xamarin.Forms;

[assembly: Dependency(typeof(EmailService))]
namespace beyond.park.client.iOS.Services {
    public sealed class EmailService : IEmailService {
        public void OpenInbox() {
            NSUrl mailUrl = new NSUrl("message://");
            if (UIApplication.SharedApplication.CanOpenUrl(mailUrl)) {
                UIApplication.SharedApplication.OpenUrl(mailUrl);
            }
        }
    }
}