using CoreGraphics;
using SlideOverKit.iOS;
using System;
using UIKit;
using Xamarin.Forms.Platform.iOS;

namespace beyond.park.client.iOS.Renderers {
    public abstract class ContentPageRenderer : PageRenderer, ISlideOverKitPageRendereriOS {
		public Action<bool> ViewDidAppearEvent { get; set; }

		public Action<VisualElementChangedEventArgs> OnElementChangedEvent { get; set; }

		public Action ViewDidLayoutSubviewsEvent { get; set; }

		public Action<bool> ViewDidDisappearEvent { get; set; }

		public Action<CGSize, IUIViewControllerTransitionCoordinator> ViewWillTransitionToSizeEvent { get; set; }

		public ContentPageRenderer() {
			new SlideOverKitiOSHandler().Init(this);
		}

		protected override void OnElementChanged(VisualElementChangedEventArgs e) {
			base.OnElementChanged(e);

			if (OnElementChangedEvent != null)
				OnElementChangedEvent(e);
		}

		public override void ViewDidLayoutSubviews() {
			base.ViewDidLayoutSubviews();
			if (ViewDidLayoutSubviewsEvent != null)
				ViewDidLayoutSubviewsEvent();

		}

		public override void ViewDidAppear(bool animated) {
			base.ViewDidAppear(animated);
			if (ViewDidAppearEvent != null)
				ViewDidAppearEvent(animated);

		}

		public override void ViewDidDisappear(bool animated) {
			base.ViewDidDisappear(animated);
			if (ViewDidDisappearEvent != null)
				ViewDidDisappearEvent(animated);
		}

		public override void ViewWillTransitionToSize(CGSize toSize, IUIViewControllerTransitionCoordinator coordinator) {
			base.ViewWillTransitionToSize(toSize, coordinator);
			if (ViewWillTransitionToSizeEvent != null)
				ViewWillTransitionToSizeEvent(toSize, coordinator);
		}
	}
}