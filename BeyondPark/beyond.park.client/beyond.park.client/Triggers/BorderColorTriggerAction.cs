using beyond.park.client.Controls;
using Xamarin.Forms;

namespace beyond.park.client.Triggers {
    public class BorderColorTriggerAction : TriggerAction<EntryExtended> {
        protected override void Invoke(EntryExtended entry) {
            if (!string.IsNullOrEmpty(entry.Text)) {
                entry.BorderColor = (Color)App.Current.Resources["OrangeColor"];
            } else {
                entry.BorderColor = (Color)App.Current.Resources["GrayColor"];
            }
        }
    }
}
