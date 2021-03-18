using Android.Content;
using beyond.park.client.Droid.Services;
using beyond.park.client.Services.Platform.Contracts;
using Xamarin.Forms;

[assembly: Dependency(typeof(EmailService))]
namespace beyond.park.client.Droid.Services {
    public class EmailService : IEmailService {
        public void OpenInbox() {
            Intent intent = new Intent(Intent.ActionMain);
            intent.AddCategory(Intent.CategoryAppEmail);
            Android.App.Application.Context.StartActivity(intent);
        }
    }
}