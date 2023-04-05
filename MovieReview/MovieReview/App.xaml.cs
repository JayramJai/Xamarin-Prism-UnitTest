using System.Diagnostics.CodeAnalysis;
using Acr.UserDialogs;
using Flurl.Http.Configuration;
using MovieReview.Services;
using MovieReview.Services.Interfaces;
using MovieReview.ViewModel;
using MovieReview.Views;
using Plugin.Connectivity;
using Prism;
using Prism.Ioc;
using Prism.Plugin.Popups;
using Xamarin.Forms;

namespace MovieReview
{
    [ExcludeFromCodeCoverage]
    public partial class App 
    {
        public App(IPlatformInitializer initializer): base(initializer) { }

        protected async override void OnInitialized()
        {
            InitializeComponent();
            await NavigationService.NavigateAsync("NavigationPage/Homepage");
        }
        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterInstance(CrossConnectivity.Current);
            containerRegistry.RegisterInstance(UserDialogs.Instance);
            containerRegistry.RegisterPopupNavigationService();

            RegisterSingleton(containerRegistry);
            RegisterForNavigation(containerRegistry);
        }

        private static void RegisterSingleton(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterSingleton<IContentService, ContentService>();
            containerRegistry.RegisterSingleton<IFlurlClientFactory, PerBaseUrlFlurlClientFactory>();
            containerRegistry.RegisterSingleton<ILocalStorageService, LocalStorageService>();
            containerRegistry.RegisterSingleton<IDialogService, DialogService>();
        }
        private static void RegisterForNavigation(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<NavigationPage>();
            containerRegistry.RegisterForNavigation<Homepage, HomepageViewModel>();
            containerRegistry.RegisterForNavigation<DetailPage, DetailPageViewModel>();
        }
    }
}
