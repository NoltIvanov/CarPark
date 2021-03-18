using Autofac;
using beyond.park.client.Builders.DataItems.CarparkItems;
using beyond.park.client.Builders.DataItems.FilterItems;
using beyond.park.client.Builders.DataItems.HelpItems;
using beyond.park.client.Builders.DataItems.TourItems;
using beyond.park.client.Factories.Validation;
using beyond.park.client.Services.Dialog;
using beyond.park.client.Services.GoogleMap;
using beyond.park.client.Services.Identity;
using beyond.park.client.Services.Navigation;
using beyond.park.client.Services.OpenUrl;
using beyond.park.client.Services.Permission;
using beyond.park.client.Services.RequestProvider;
using beyond.park.client.ViewModels.ActionBars;
using beyond.park.client.ViewModels.Popups;
using beyond.park.client.ViewModels.Registration;
using System;
using System.Diagnostics;
using System.Globalization;
using System.Reflection;
using Xamarin.Forms;

namespace beyond.park.client.ViewModels.Base {
    public static class DependencyLocator {

        private static IContainer _container;

        public static readonly BindableProperty AutoWireViewModelProperty =
          BindableProperty.CreateAttached("AutoWireViewModel", typeof(bool), typeof(DependencyLocator), default(bool), propertyChanged: OnAutoWireViewModelChanged);

        public static bool GetAutoWireViewModel(BindableObject bindable) {
            return (bool)bindable.GetValue(AutoWireViewModelProperty);
        }

        public static void SetAutoWireViewModel(BindableObject bindable, bool value) {
            bindable.SetValue(AutoWireViewModelProperty, value);
        }

        public static void RegisterDependencies() {
            var builder = new ContainerBuilder();

            // View models.    
            builder.RegisterType<MainViewModel>();
            builder.RegisterType<GuideViewModel>();
            builder.RegisterType<SignInViewModel>();
            builder.RegisterType<WelcomeViewModel>();
            builder.RegisterType<SideMenuViewModel>();
            builder.RegisterType<LogInDetailsViewModel>();
            builder.RegisterType<CarparksPopupViewModel>();
            builder.RegisterType<VehicleDetailsViewModel>();
            builder.RegisterType<PaymentDetailsViewModel>();
            builder.RegisterType<AccountCreatedViewModel>();
            builder.RegisterType<NotSignUpPopupViewModel>();
            builder.RegisterType<HelpSystemPopupViewModel>();
            builder.RegisterType<CommonActionBarViewModel>();
            builder.RegisterType<WelcomeBackPopupViewModel>();
            builder.RegisterType<AccountCreationPopupViewModel>();                        

            // Services.         
            builder.RegisterType<DialogService>().As<IDialogService>();
            builder.RegisterType<OpenUrlService>().As<IOpenUrlService>().SingleInstance();         
            builder.RegisterType<RequestProvider>().As<IRequestProvider>().SingleInstance();
            builder.RegisterType<IdentityService>().As<IIdentityService>();           
            builder.RegisterType<GoogleMapService>().As<IGoogleMapService>();
            builder.RegisterType<NavigationService>().As<INavigationService>().SingleInstance();
            builder.RegisterType<PermissionService>().As<IPermissionService>();

            // Factories.            
            builder.RegisterType<ValidationObjectFactory>().As<IValidationObjectFactory>();

            // Data items
            builder.RegisterType<TourItemBuilder>().As<ITourItemBuilder>();
            builder.RegisterType<HelpItemBuilder>().As<IHelpItemBuilder>();
            builder.RegisterType<FilterItemBuilder>().As<IFilterItemBuilder>();
            builder.RegisterType<CarparkItemBuilder>().As<ICarparkItemBuilder>();

            if (_container != null) {
                _container.Dispose();
            }
            _container = builder.Build();
        }

        public static T Resolve<T>() => _container.Resolve<T>();

        private static void OnAutoWireViewModelChanged(BindableObject bindable, object oldValue, object newValue) {
            if (!(bindable is Element view)) return;

            var viewType = view.GetType();
            var viewName = viewType.FullName.Replace(".Views.", ".ViewModels.");
            var viewAssemblyName = viewType.GetTypeInfo().Assembly.FullName;
            var viewModelName = string.Format(CultureInfo.InvariantCulture, "{0}Model, {1}", viewName, viewAssemblyName);

            var viewModelType = Type.GetType(viewModelName);
            if (viewModelType == null) {
                Debug.WriteLine("------------------------ERROR: Can't find viewModel type ---------------------");
                return;
            }

            var viewModel = _container.Resolve(viewModelType);
            view.BindingContext = viewModel;
        }
    }
}
