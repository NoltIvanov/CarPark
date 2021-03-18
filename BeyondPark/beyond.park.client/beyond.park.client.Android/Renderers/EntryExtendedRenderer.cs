using Android.Content;
using Android.Views;
using beyond.park.client.Controls;
using beyond.park.client.Droid.Renderers;
using Microsoft.AppCenter.Crashes;
using System;
using System.ComponentModel;
using System.Diagnostics;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(EntryExtended), typeof(EntryExtendedRenderer))]
namespace beyond.park.client.Droid.Renderers {
    public class EntryExtendedRenderer : EntryRenderer {

		const GravityFlags DefaultGravity = GravityFlags.CenterVertical;

		BorderRenderer _renderer;

		public EntryExtendedRenderer(Context context) : base(context) { }

		protected override void OnElementChanged(ElementChangedEventArgs<Entry> e) {
			base.OnElementChanged(e);
			if (e.OldElement != null || this.Element == null) {
				return;
			}

			Control.Gravity = DefaultGravity;

			EntryExtended entryEx = Element as EntryExtended;

			UpdateBackground(entryEx);
			UpdatePadding(entryEx);
			UpdateTextAlighnment(entryEx);
		}

		protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e) {
			base.OnElementPropertyChanged(sender, e);
			try {
				if (Element == null) {
					return;
				}

				EntryExtended entryEx = Element as EntryExtended;

				if (e.PropertyName == EntryExtended.BorderWidthProperty.PropertyName ||
					e.PropertyName == EntryExtended.BorderColorProperty.PropertyName ||
					e.PropertyName == EntryExtended.BorderRadiusProperty.PropertyName ||
					e.PropertyName == EntryExtended.BackgroundColorProperty.PropertyName) {
					UpdateBackground(entryEx);
				} else if (e.PropertyName == EntryExtended.LeftPaddingProperty.PropertyName ||
					  e.PropertyName == EntryExtended.RightPaddingProperty.PropertyName) {
					UpdatePadding(entryEx);
				} else if (e.PropertyName == Entry.HorizontalTextAlignmentProperty.PropertyName) {
					UpdateTextAlighnment(entryEx);
				}
			} catch (Exception ex) {
				Crashes.TrackError(ex);
				Debugger.Break();
			}
		}

		protected override void Dispose(bool disposing) {
			base.Dispose(disposing);
			try {
				if (disposing) {
					if (_renderer != null) {
						_renderer.Dispose();
						_renderer = null;
					}
				}
			} catch (Exception ex) {
				Crashes.TrackError(ex);
				Debugger.Break();
			}
		}

		void UpdateBackground(EntryExtended entryEx) {
			try {
				_renderer = new BorderRenderer();

				Control.Background = _renderer.GetBorderBackground(entryEx.BorderColor, entryEx.BackgroundColor, entryEx.BorderWidth, entryEx.BorderRadius);
			} catch (Exception ex) {
				Crashes.TrackError(ex);
				Debugger.Break();
			}
			if (_renderer != null) {
				_renderer.Dispose();
				_renderer = null;
			}
		}

		void UpdatePadding(EntryExtended entryEx) {
			try {
				Control.SetPadding((int)Context.ToPixels(entryEx.LeftPadding), 0,
					(int)Context.ToPixels(entryEx.RightPadding), 0);
			} catch (Exception ex) {
				Crashes.TrackError(ex);
				Debugger.Break();
				System.Diagnostics.Debug.WriteLine($"ERROR: {ex.Message}");
			}
		}

		void UpdateTextAlighnment(EntryExtended entryEx) {
			try {
				GravityFlags gravity = DefaultGravity;

				switch (entryEx.HorizontalTextAlignment) {
					case Xamarin.Forms.TextAlignment.Start:
						gravity |= GravityFlags.Start;
						break;
					case Xamarin.Forms.TextAlignment.Center:
						gravity |= GravityFlags.CenterHorizontal;
						break;
					case Xamarin.Forms.TextAlignment.End:
						gravity |= GravityFlags.End;
						break;
				}

				Control.Gravity = gravity;
			} catch (Exception ex) {
				Crashes.TrackError(ex);
				Debugger.Break();
			}
		}
	}
}