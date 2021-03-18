using beyond.park.client.Controls;
using beyond.park.client.iOS.Renderers;
using CoreGraphics;
using System;
using System.ComponentModel;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(EntryExtended), typeof(EntryExtendedRenderer))]
namespace beyond.park.client.iOS.Renderers {
    public class EntryExtendedRenderer : EntryRenderer {
        EntryExtended _element;

        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e) {
            base.OnElementChanged(e);

            if (Control != null) {
                DisableNativeBorder();
            }

            if (Element != null) {
                UpdatePadding();

                if (e.NewElement != null) {
                    _element = e.NewElement as EntryExtended;
                    SetupLayer((int)_element.BorderWidth, _element.BorderRadius);
                }
            }

            if (e.OldElement != null) {
                // Unsubscribe from event handlers and cleanup any resources
            }

            if (e.NewElement != null) {
                // Configure the control and subscribe to event handlers
            }
        }

        void SetupLayer(int borderWidth, nfloat borderRadius) {
            Layer.CornerRadius = borderRadius;

            if (Element.BackgroundColor != Color.Default) {
                Layer.BackgroundColor = Element.BackgroundColor.ToUIColor().CGColor;
            } else {
                Layer.BackgroundColor = UIColor.White.CGColor;
            }

            if (_element.BorderColor != Color.Default) {
                Layer.BorderColor = _element.BorderColor.ToCGColor();
                Layer.BorderWidth = borderWidth;
            }

            Layer.RasterizationScale = UIScreen.MainScreen.Scale;
            Layer.ShouldRasterize = true;
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e) {
            base.OnElementPropertyChanged(sender, e);

            if (e.PropertyName == EntryExtended.LeftPaddingProperty.PropertyName) {
                UpdatePadding();
            } else if (e.PropertyName == EntryExtended.BorderColorProperty.PropertyName) {
                SetupLayer((int)_element.BorderWidth, _element.BorderRadius);
            }
        }

        void UpdatePadding() {
            UIView paddingView = new UIView(new CGRect(0, 0, ((EntryExtended)Element).LeftPadding, 0));
            Control.LeftView = paddingView;
            Control.LeftViewMode = UITextFieldViewMode.Always;
        }

        void DisableNativeBorder() {
            Control.BorderStyle = UIKit.UITextBorderStyle.None;
        }
    }
}