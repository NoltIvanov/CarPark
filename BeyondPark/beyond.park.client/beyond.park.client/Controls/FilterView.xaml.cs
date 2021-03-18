using System;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.PancakeView;

namespace beyond.park.client.Controls {
    public partial class FilterView : PancakeView {

        private TapGestureRecognizer _tapGestureRecognizer;

        public static readonly BindableProperty TitleProperty = BindableProperty.Create(
                nameof(TitleProperty),
                typeof(string),
                typeof(FilterView),
                string.Empty,
                BindingMode.OneWay,
                propertyChanged: TitleChanging);
     
        public static readonly BindableProperty IsSelectedProperty = BindableProperty.Create(
                nameof(IsSelected),
                typeof(bool),
                typeof(FilterView),
                default(bool),
                BindingMode.TwoWay,
                propertyChanged: SelectedChanging);

        public static readonly BindableProperty SelectCommandProperty = BindableProperty.Create(
               propertyName: nameof(SelectCommand),
               returnType: typeof(ICommand),
               declaringType: typeof(FilterView),
               defaultValue: default(ICommand));

        public bool IsSelected {
            get => (bool)GetValue(IsSelectedProperty);
            set => SetValue(IsSelectedProperty, value);
        }

        public string Title {
            get => (string)GetValue(TitleProperty);
            set => SetValue(TitleProperty, value);
        }

        public ICommand SelectCommand {
            get => (ICommand)GetValue(SelectCommandProperty);
            set => SetValue(SelectCommandProperty, value);
        }

        public FilterView() {
            InitializeComponent();

            _tapGestureRecognizer = new TapGestureRecognizer { NumberOfTapsRequired = 1 };            
            _tapGestureRecognizer.Tapped += Tapped_Tapped;
            GestureRecognizers.Add(_tapGestureRecognizer);
        }

        ~FilterView() {
            _tapGestureRecognizer.Tapped -= Tapped_Tapped;
        }

        private void Tapped_Tapped(object sender, EventArgs e) {
            Select();
            SelectCommand?.Execute(null);
        }

        private static void SelectedChanging(BindableObject bindable, object oldValue, object newValue) {
            //if (bindable is FilterView filterView) {
            //    if (newValue is bool selected && !selected) {
            //        filterView.DeSelect();
            //    } 
            //}

            if (bindable is FilterView filterView && newValue is bool selected) {
                if (selected) {
                    filterView.Select();
                } else {
                    filterView.DeSelect();
                }               
            }
        }

        private static void TitleChanging(BindableObject bindable, object oldValue, object newValue) {
            if (bindable is FilterView filterView && newValue is string value) {
                filterView.title.Text = value;                
            }
        }

        private void Select() {
            title.TextColor = (Color)App.Current.Resources["WhiteColor"];
            BackgroundColor = (Color)App.Current.Resources["BlueColor"];
        }

        private void DeSelect() {
            title.TextColor = (Color)App.Current.Resources["GrayColor"];
            BackgroundColor = (Color)App.Current.Resources["WhiteColor"];
        }
    }
}