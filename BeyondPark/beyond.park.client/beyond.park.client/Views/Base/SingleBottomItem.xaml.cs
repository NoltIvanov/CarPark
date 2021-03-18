using beyond.park.client.ViewModels.Base.Contracts;
using FFImageLoading.Forms;
using FFImageLoading.Transformations;
using Xamarin.Forms;

namespace beyond.park.client.Views.Base {

    public partial class SingleBottomItem : BottomItemViewBase {
        private static readonly ColorSpaceTransformation DEFAULT_ICON_COLOR = new ColorSpaceTransformation(Color.Black.ColorToMatrix());

        private static readonly ColorSpaceTransformation SELECTED_ICON_COLOR_TRANSFORMATION =
            new ColorSpaceTransformation(((Color)App.Current.Resources["GreenColor"]).ColorToMatrix());

        public SingleBottomItem() {
            InitializeComponent();
            ComponentInitialized();
        }

        public Color _themeColor;
        public Color ThemeColor {
            get => _themeColor;
            set {
                _themeColor = value;
            }
        }

        protected override void OnIsSelected(BindableObject bindable = default(BindableObject)) {
            base.OnIsSelected(bindable);

            if (IsSelected) {
                if (bindable != null) {
                    if (bindable is BottomItemViewBase singleBottomItem) {
                        if (singleBottomItem.BindingContext is IBottomBarTab bottomBarTab) {
                            if (bottomBarTab.HasBackgroundItem) {
                                _icon_CachedImage.Transformations.Add(DEFAULT_ICON_COLOR);
                                _icon_CachedImage.ReloadImage();
                            } else {
                                _icon_CachedImage.Transformations.Add(SELECTED_ICON_COLOR_TRANSFORMATION);
                                _icon_CachedImage.ReloadImage();
                            }
                        }
                    }
                }
            } else {
                _icon_CachedImage.Transformations.Clear();
                _icon_CachedImage.ReloadImage();
            }
        }
    }
}