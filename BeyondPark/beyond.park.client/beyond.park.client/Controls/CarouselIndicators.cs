using FFImageLoading.Svg.Forms;
using System;
using System.Collections;
using System.Linq;
using Xamarin.Forms;

namespace beyond.park.client.Controls {
    public class CarouselIndicators : Grid {

        private ImageSource _unselectedImageSource = null;

        private ImageSource _selectedImageSource = null;

        private readonly StackLayout _indicators = new StackLayout() { Orientation = StackOrientation.Horizontal, HorizontalOptions = LayoutOptions.CenterAndExpand };

        /// <summary>
        ///     ctor().
        /// </summary>
        public CarouselIndicators() {
            HorizontalOptions = LayoutOptions.CenterAndExpand;
            RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });
            Children.Add(_indicators);
        }

        public static readonly BindableProperty PositionProperty = BindableProperty.Create(
                nameof(Position),
                typeof(int),
                typeof(CarouselIndicators),
                default(int),
                BindingMode.TwoWay,
                propertyChanging: PositionChanging);

        public static readonly BindableProperty ItemsSourceProperty = BindableProperty.Create(
            nameof(ItemsSource),
            typeof(IEnumerable),
            typeof(CarouselIndicators),
            Enumerable.Empty<object>(),
            BindingMode.OneWay,
            propertyChanged: ItemsChanged);

        public static readonly BindableProperty SelectedIndicatorProperty = BindableProperty.Create(
            nameof(SelectedIndicator),
            typeof(string),
            typeof(CarouselIndicators),
            string.Empty,
            BindingMode.OneWay);

        public static readonly BindableProperty UnselectedIndicatorProperty = BindableProperty.Create(
            nameof(UnselectedIndicator),
            typeof(string),
            typeof(CarouselIndicators),
            string.Empty,
            BindingMode.OneWay);

        public static readonly BindableProperty IndicatorWidthProperty = BindableProperty.Create(
            nameof(IndicatorWidth),
            typeof(double),
            typeof(CarouselIndicators),
            default(double),
            BindingMode.OneWay);

        public static readonly BindableProperty IndicatorHeightProperty = BindableProperty.Create(
            nameof(IndicatorHeight),
            typeof(double),
            typeof(CarouselIndicators),
            default(double),
            BindingMode.OneWay);

        public string SelectedIndicator {
            get => (string)GetValue(SelectedIndicatorProperty);
            set => SetValue(SelectedIndicatorProperty, value);           
        }

        public string UnselectedIndicator {
            get => (string)GetValue(UnselectedIndicatorProperty); 
            set => SetValue(UnselectedIndicatorProperty, value); 
        }

        public double IndicatorWidth {
            get => (double)GetValue(IndicatorWidthProperty); 
            set => SetValue(IndicatorWidthProperty, value); 
        }

        public double IndicatorHeight {
            get => (double)GetValue(IndicatorHeightProperty); 
            set => SetValue(IndicatorHeightProperty, value); 
        }

        public int Position {
            get => (int)GetValue(PositionProperty); 
            set => SetValue(PositionProperty, value); 
        }

        public IEnumerable ItemsSource {
            get => (IEnumerable)GetValue(ItemsSourceProperty); 
            set => SetValue(ItemsSourceProperty, (object)value); 
        }

        private void Clear() {
            _indicators.Children.Clear();
        }

        private void Init(int position) {

            if (_unselectedImageSource == null)                
                _unselectedImageSource = UnselectedIndicator;

            if (_selectedImageSource == null)               
                _selectedImageSource = SelectedIndicator;

            if (_indicators.Children.Count > 0) {
                for (int i = 0; i < _indicators.Children.Count; i++) {
                    if (((SvgCachedImage)_indicators.Children[i]).ClassId == nameof(State.Selected) && i != position)
                        _indicators.Children[i] = BuildImage(State.Unselected, i);
                    else if (((SvgCachedImage)_indicators.Children[i]).ClassId == nameof(State.Unselected) && i == position)
                        _indicators.Children[i] = BuildImage(State.Selected, i);
                }
            } else {
                var enumerator = ItemsSource?.GetEnumerator();
                if (enumerator == null) return;
                int count = 0;
                while (enumerator.MoveNext()) {
                    SvgCachedImage image = null;
                    if (position == count)
                        image = BuildImage(State.Selected, count);
                    else
                        image = BuildImage(State.Unselected, count);

                    _indicators.Children.Add(image);

                    count++;
                }
            }
        }

        private SvgCachedImage BuildImage(State state, int position) {
            var image = new SvgCachedImage() {
                WidthRequest = IndicatorWidth,
                HeightRequest = IndicatorHeight,
                ClassId = state.ToString()
            };

            switch (state) {
                case State.Selected:
                    image.Source = _selectedImageSource;                    
                    break;
                case State.Unselected:
                    image.Source = _unselectedImageSource;                    
                    break;
                default:
                    throw new Exception("Invalid state selected");
            }

            image.GestureRecognizers.Add(new TapGestureRecognizer() { Command = new Command(() => { Position = position; }) });

            return image;
        }

        private static void PositionChanging(object bindable, object oldValue, object newValue) {
            var carouselIndicators = bindable as CarouselIndicators;

            carouselIndicators.Init(Convert.ToInt32(newValue));
        }

        private static void ItemsChanged(object bindable, object oldValue, object newValue) {
            var carouselIndicators = bindable as CarouselIndicators;

            carouselIndicators.Clear();
            carouselIndicators.Init(0);
        }

        public enum State {
            Selected,
            Unselected
        }
    }
}
