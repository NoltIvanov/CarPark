
using Xamarin.Forms;

namespace beyond.park.client.Controls {
    public partial class Indicator : ContentView {

        public static readonly BindableProperty PadCanvasColorProperty = BindableProperty.Create(
            nameof(PadCanvasColor),
            typeof(Color),
            typeof(Indicator),
            defaultValue: default(Color),
            propertyChanged: (BindableObject bindable, object oldValue, object newValue) => {
                if (bindable is Indicator declarer) {
                    declarer._padCanvas_BoxView.BackgroundColor = (Color)newValue;
                }
            });

        public static readonly BindableProperty IndicatorColorProperty = BindableProperty.Create(
            nameof(IndicatorColor),
            typeof(Color),
            typeof(Indicator),
            defaultValue: default(Color),
            propertyChanged: (BindableObject bindable, object oldValue, object newValue) => {
                if (bindable is Indicator declarer) {
                    declarer._spinnerIndicator_ActivityIndicator.Color = (Color)newValue;
                }
            });

        public Indicator() {
            InitializeComponent();
        }

        public Color PadCanvasColor {
            get => (Color)GetValue(Indicator.PadCanvasColorProperty);
            set => SetValue(Indicator.PadCanvasColorProperty, value);
        }

        public Color IndicatorColor {
            get => (Color)GetValue(Indicator.IndicatorColorProperty);
            set => SetValue(Indicator.IndicatorColorProperty, value);
        }
    }
}