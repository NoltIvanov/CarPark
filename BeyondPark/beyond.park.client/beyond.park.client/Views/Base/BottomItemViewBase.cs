using beyond.park.client.ViewModels.Base;
using beyond.park.client.ViewModels.Base.Contracts;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using Xamarin.Forms;

namespace beyond.park.client.Views.Base {
    public abstract class BottomItemViewBase : ContentView {
        public BindableProperty IsSelectedProperty = BindableProperty.Create(
            nameof(IsSelected),
            typeof(bool),
            typeof(BottomItemViewBase),
            defaultValue: false,
            propertyChanged: (BindableObject bindable, object oldValue, object newValue) => (bindable as BottomItemViewBase).OnIsSelected(bindable));

        public bool IsSelected {
            get => (bool)GetValue(IsSelectedProperty);
            set => SetValue(IsSelectedProperty, value);
        }

        public View AppropriateItemContentView { get; private set; }

        public int TabIndex { get; set; }

        protected override void OnPropertyChanged([CallerMemberName] string propertyName = null) {
            base.OnPropertyChanged(propertyName);

            if (propertyName == BindingContextProperty.PropertyName) {
                if (BindingContext is IBottomBarTab bottomBarItemContext) {
                    ApplyRelativeView(bottomBarItemContext);
                }
            }
        }

        protected virtual void OnIsSelected(BindableObject bindable = default(BindableObject)) { }

        /// <summary>
        /// Hacky/crutch way to inform base class that XAML was initialized. Call it after InitializeComponent(); in derived class.
        /// </summary>
        protected void ComponentInitialized() {
            SetBinding(IsSelectedProperty, new Binding(nameof(IsSelected), mode: BindingMode.TwoWay));
            OnIsSelected();
        }

        private void ApplyRelativeView(IBottomBarTab bottomBarItemContext) {
            if (!(bottomBarItemContext is ViewLessTabViewModel)) {
                AppropriateItemContentView = (View)new DataTemplate(bottomBarItemContext.RelativeViewType).CreateContent();
                AppropriateItemContentView.BindingContext = bottomBarItemContext;
            }
        }
    }
}
