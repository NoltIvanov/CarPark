using System;
using System.Windows.Input;
using Xamarin.Forms;

namespace beyond.park.client.Controls.Popups {
    public abstract class SinglePopupViewBase : ContentView {

        public event EventHandler Viewable = delegate { };

        public static readonly BindableProperty BackingColorProperty = BindableProperty.Create(
            nameof(BackingColor),
            typeof(Color),
            typeof(SinglePopupViewBase),
            defaultValue: Color.Black);

        public static readonly BindableProperty IsViewableProperty = BindableProperty.Create(
            nameof(IsViewable),
            typeof(bool),
            typeof(SinglePopupViewBase),
            defaultValue: default(bool),
            propertyChanged: (BindableObject bindable, object oldValue, object newValue) => (bindable as SinglePopupViewBase)?.OnIsViewable());

        public static readonly BindableProperty CloseCommandProperty = BindableProperty.Create(
            nameof(CloseCommand),
            typeof(ICommand),
            typeof(SinglePopupViewBase),
            defaultValue: null);    

        public Color BackingColor {
            get => (Color)GetValue(BackingColorProperty);
            set => SetValue(BackingColorProperty, value);
        }
        
        public bool IsViewable {
            get => (bool)GetValue(SinglePopupViewBase.IsViewableProperty);
            set => SetValue(SinglePopupViewBase.IsViewableProperty, value);
        }

        public ICommand CloseCommand {
            get => (ICommand)GetValue(CloseCommandProperty);
            set => SetValue(CloseCommandProperty, value);
        }

        private void OnIsViewable() {
            Viewable.Invoke(this, new EventArgs());
        }
    }
}
