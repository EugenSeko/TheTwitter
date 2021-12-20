﻿using DLToolkit.Forms.Controls;
using InterTwitter.Droid.Services.PermissionsService;
using InterTwitter.Models;
using InterTwitter.Resources;
using InterTwitter.Services;
using InterTwitter.Services.BookmarkService;
using InterTwitter.Services.PermissionsService;
using InterTwitter.Services.Settings;
using InterTwitter.Services.UserService;
using InterTwitter.ViewModels;
using InterTwitter.ViewModels.Flyout;
using InterTwitter.Views;
using Prism;
using Prism.Ioc;
using Prism.Navigation;
using Prism.Unity;
using System.Globalization;
using Xamarin.CommunityToolkit.Helpers;
using Xamarin.Forms;

namespace InterTwitter
{
    public partial class App : PrismApplication
    {
        public static T Resolve<T>() => Current.Container.Resolve<T>();

        public App(IPlatformInitializer initializer = null)
            : base(initializer)
        {
        }

        #region -- Overrides --

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterDialog<AlertView, AlertViewModel>();

            //Services
            containerRegistry.RegisterInstance<ISettingsManager>(Container.Resolve<SettingsManager>());
            containerRegistry.RegisterInstance<IMockService>(Container.Resolve<MockService>());
            containerRegistry.RegisterInstance<ITweetService>(Container.Resolve<TweetService>());
            containerRegistry.RegisterInstance<IBookmarkService>(Container.Resolve<BookmarkService>());
            containerRegistry.RegisterInstance<IPermissionsService>(Container.Resolve<PermissionsService>());
            containerRegistry.RegisterInstance<IUserService>(Container.Resolve<UserService>());

            // Navigation
            containerRegistry.RegisterForNavigation<NavigationPage>();
            containerRegistry.RegisterForNavigation<FlyOutPage, FlyOutPageViewModel>();
            containerRegistry.RegisterForNavigation<FlyoutPageDetail, FlyoutPageDetailViewModel>();
            containerRegistry.RegisterForNavigation<FlyoutPageFlyout, FlyoutPageFlyoutViewModel>();
            containerRegistry.RegisterForNavigation<HomePage, HomePageViewModel>();
            containerRegistry.RegisterForNavigation<SearchPage, SearchPageViewModel>();
            containerRegistry.RegisterForNavigation<BookmarksPage, BookmarksPageViewModel>();
            containerRegistry.RegisterForNavigation<NotificationsPage, NotificationPageViewModel>();
            containerRegistry.RegisterForNavigation<ProfilePage, ProfilePageViewModel>();
            containerRegistry.RegisterForNavigation<EditProfilePage, EditProfilePageViewModel>();
            containerRegistry.RegisterForNavigation<BlacklistPage, BlacklistPageViewModel>();

            //Dialogs
            containerRegistry.RegisterDialog<AlertView, AlertViewModel>();
            containerRegistry.RegisterDialog<Alert2View, AlertViewModel>();
        }

        protected override async void OnInitialized()
        {
            LocalizationResourceManager.Current.PropertyChanged += (sender, e) => Resource.Culture = LocalizationResourceManager.Current.CurrentCulture;
            LocalizationResourceManager.Current.Init(Resource.ResourceManager);
            LocalizationResourceManager.Current.CurrentCulture = new CultureInfo("en");

            InitializeComponent();

            Sharpnado.Shades.Initializer.Initialize(loggerEnable: false);
            FlowListView.Init();
            await NavigationService.NavigateAsync($"/{nameof(StartPage)}");
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }

        #endregion

    }
}
