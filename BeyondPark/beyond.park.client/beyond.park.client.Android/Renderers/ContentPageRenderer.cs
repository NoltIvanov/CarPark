using Android.Content;
using SlideOverKit.Droid;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

namespace beyond.park.client.Droid.Renderers {
    public abstract class ContentPageRenderer : PageRenderer, ISlideOverKitPageRendererDroid {
        public Action<ElementChangedEventArgs<Page>> OnElementChangedEvent { get; set; }

        public Action<bool, int, int, int, int> OnLayoutEvent { get; set; }

        public Action<int, int, int, int> OnSizeChangedEvent { get; set; }

        public ContentPageRenderer(Context context) : base(context) {
            new SlideOverKitDroidHandler().Init(this, context);
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Page> e) {
            base.OnElementChanged(e);
            if (OnElementChangedEvent != null)
                OnElementChangedEvent(e);
        }

        protected override void OnLayout(bool changed, int l, int t, int r, int b) {
            base.OnLayout(changed, l, t, r, b);
            if (OnLayoutEvent != null)
                OnLayoutEvent(changed, l, t, r, b);
        }

        protected override void OnSizeChanged(int w, int h, int oldw, int oldh) {
            base.OnSizeChanged(w, h, oldw, oldh);
            if (OnSizeChangedEvent != null)
                OnSizeChangedEvent(w, h, oldw, oldh);
        }
    }
}