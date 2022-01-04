using DLToolkit.Forms.Controls;
using InterTwitter.Resources.Strings;
using InterTwitter.Services;
using InterTwitter.Services.Video;
using InterTwitter.ViewModels;
using InterTwitter.Views;
using Prism;
using Prism.Ioc;
using Prism.Plugin.Popups;
using Prism.Unity;
using Xamarin.CommunityToolkit.Helpers;
using Xamarin.Forms;

namespace InterTwitter
{
    public partial class App : PrismApplication
    {
        public App(IPlatformInitializer initializer = null)
            : base(initializer)
        {
        }

        #region -- Overrides --

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterPopupNavigationService();
            containerRegistry.RegisterPopupDialogService();
            containerRegistry.RegisterDialog<AlertView, AlertViewModel>();

            //Services
            containerRegistry.RegisterSingleton<IMockService, MockService>();
            containerRegistry.RegisterSingleton<ISettingsManager, SettingsManager>();
            containerRegistry.RegisterSingleton<IAuthorizationService, AuthorizationService>();
            containerRegistry.RegisterSingleton<ITweetService, TweetService>();
            containerRegistry.RegisterSingleton<IHashtagService, HashtagService>();
            containerRegistry.RegisterSingleton<IRegistrationService, RegistrationService>();
            containerRegistry.RegisterSingleton<IUserService, UserService>();
            containerRegistry.RegisterSingleton<IBookmarkService, BookmarkService>();
            containerRegistry.RegisterSingleton<ILikeService, LikeService>();
            containerRegistry.RegisterSingleton<INotificationService, NotificationService>();
            containerRegistry.RegisterSingleton<IVideoService, VideoService>();

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
            containerRegistry.RegisterForNavigation<CreatePage, CreatePageViewModel>();
            containerRegistry.RegisterForNavigation<LogInPage, LogInPageViewModel>();
            containerRegistry.RegisterForNavigation<PasswordPage, PasswordPageViewModel>();
            containerRegistry.RegisterForNavigation<EditProfilePage, EditProfilePageViewModel>();
            containerRegistry.RegisterForNavigation<BlacklistPage, BlacklistPageViewModel>();
            containerRegistry.RegisterForNavigation<CreateTweetPage, CreateTweetPageViewModel>();
        }

        protected override async void OnInitialized()
        {
            InitializeComponent();
            App.Current.UserAppTheme = OSAppTheme.Light;

            FlowListView.Init();
            Sharpnado.Shades.Initializer.Initialize(loggerEnable: false);
            LocalizationResourceManager.Current.Init(Strings.ResourceManager);

            await NavigationService.NavigateAsync($"/{nameof(LogInPage)}");
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
            this.PopupPluginOnSleep();
        }

        protected override void OnResume()
        {
            this.PopupPluginOnResume();
        }

        #endregion
    }
}
