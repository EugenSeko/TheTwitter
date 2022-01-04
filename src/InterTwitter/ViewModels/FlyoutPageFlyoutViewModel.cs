using InterTwitter.Helpers;
using InterTwitter.Models;
using InterTwitter.Services;
using InterTwitter.Views;
using Prism.Navigation;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace InterTwitter.ViewModels
{
    public class FlyoutPageFlyoutViewModel : BaseTabViewModel
    {
        private readonly ISettingsManager _settingsManager;
        private readonly IUserService _userService;
        private readonly IAuthorizationService _authorizationService;

        private UserModel _user;

        public FlyoutPageFlyoutViewModel(
            INavigationService navigationService,
            ISettingsManager settingsManager,
            IAuthorizationService authorizationService,
            IUserService userService)
            : base(navigationService)
        {
            _settingsManager = settingsManager;
            _userService = userService;
            _authorizationService = authorizationService;

            MenuItems = new ObservableCollection<MenuItemViewModel>(new[]
                {
                    new MenuItemViewModel
                    {
                        Id = 0, Title = "Home",
                        TargetType = typeof(HomePage),
                        ImageSource = Prism.PrismApplicationBase.Current.Resources["ic_home_gray"] as ImageSource,
                        TapCommand = new Command(OnItemTapCommand),
                    },

                    new MenuItemViewModel
                    {
                        Id = 1,
                        Title = "Search",
                        TargetType = typeof(SearchPage),
                        ImageSource = Prism.PrismApplicationBase.Current.Resources["ic_search_gray"] as ImageSource,
                        TapCommand = new Command(OnItemTapCommand),
                    },
                    new MenuItemViewModel
                    {
                        Id = 2,
                        Title = "Notification",
                        TargetType = typeof(NotificationsPage),
                        ImageSource = Prism.PrismApplicationBase.Current.Resources["ic_notifications_gray"] as ImageSource,
                        TapCommand = new Command(OnItemTapCommand),
                    },
                    new MenuItemViewModel
                    {
                        Id = 3,
                        Title = "Bookmarks",
                        TargetType = typeof(BookmarksPage),
                        ImageSource = Prism.PrismApplicationBase.Current.Resources["ic_bookmarks_gray"] as ImageSource,
                        TapCommand = new Command(OnItemTapCommand),
                    },
                });

            Subscribe();
        }

        #region -- Public Properties --

        private ObservableCollection<MenuItemViewModel> _menuItems;
        public ObservableCollection<MenuItemViewModel> MenuItems
        {
            get => _menuItems;
            set => SetProperty(ref _menuItems, value);
        }

        private string _profileName;
        public string ProfileName
        {
            get => _profileName;
            set => SetProperty(ref _profileName, value);
        }

        private string _profileEmail;
        public string ProfileEmail
        {
            get => _profileEmail;
            set => SetProperty(ref _profileEmail, value);
        }

        private string _userImagePath;
        public string UserImagePath
        {
            get => _userImagePath;
            set => SetProperty(ref _userImagePath, value);
        }

        private ICommand _logoutCommandAsync;
        public ICommand LogoutCommandAsync => _logoutCommandAsync ??= SingleExecutionCommand.FromFunc(OnLogoutCommandAsync);

        private ICommand _navigateEditProfileCommandAsync;
        public ICommand NavigateEditProfileCommandAsync => _navigateEditProfileCommandAsync ??= SingleExecutionCommand.FromFunc(OnNavigateEditProfileCommandAsync);

        private ICommand _navigateProfileCommandAsync;
        public ICommand NavigateProfileCommandAsync => _navigateProfileCommandAsync ??= SingleExecutionCommand.FromFunc(OnNavigateProfileCommandAsync);

        #endregion

        #region -- Overrides --

        public override Task InitializeAsync(INavigationParameters parameters)
        {
            _user = _userService.GetUserAsync(_settingsManager.UserId).Result.Result;
            ProfileName = _user.Name;
            ProfileEmail = _user.Email;
            UserImagePath = _user.AvatarPath;
            return base.InitializeAsync(parameters);
        }

        #endregion

        #region -- Private Helpers --

        private void Subscribe()
        {
            MessagingCenter.Subscribe<SearchPageViewModel, Type>(this, Constants.Messages.TAB_CHANGE, ChangeVisualState);
            MessagingCenter.Subscribe<BookmarksPageViewModel, Type>(this, Constants.Messages.TAB_CHANGE, ChangeVisualState);
            MessagingCenter.Subscribe<NotificationPageViewModel, Type>(this, Constants.Messages.TAB_CHANGE, ChangeVisualState);
            MessagingCenter.Subscribe<FlyoutPageDetailViewModel, Type>(this, Constants.Messages.TAB_CHANGE, ChangeVisualState);
            MessagingCenter.Subscribe<EditProfilePageViewModel>(this, Constants.Messages.USER_PROFILE_CHANGED, UpdateAsync);
        }

        private void ChangeVisualState(object sender, Type selectedTabType)
        {
            if (MenuItems != null)
            {
                ImageSource[] selectedItemImageSource = new ImageSource[8];

                selectedItemImageSource[0] = Prism.PrismApplicationBase.Current.Resources["ic_home_blue"] as ImageSource;
                selectedItemImageSource[1] = Prism.PrismApplicationBase.Current.Resources["ic_search_blue"] as ImageSource;
                selectedItemImageSource[2] = Prism.PrismApplicationBase.Current.Resources["ic_notifications_blue"] as ImageSource;
                selectedItemImageSource[3] = Prism.PrismApplicationBase.Current.Resources["ic_bookmarks_blue"] as ImageSource;

                selectedItemImageSource[4] = Prism.PrismApplicationBase.Current.Resources["ic_home_gray"] as ImageSource;
                selectedItemImageSource[5] = Prism.PrismApplicationBase.Current.Resources["ic_search_gray"] as ImageSource;
                selectedItemImageSource[6] = Prism.PrismApplicationBase.Current.Resources["ic_notifications_gray"] as ImageSource;
                selectedItemImageSource[7] = Prism.PrismApplicationBase.Current.Resources["ic_bookmarks_gray"] as ImageSource;

                foreach (var item in MenuItems)
                {
                    if (item.TargetType == selectedTabType)
                    {
                        item.TextColor = (Color)Prism.PrismApplicationBase.Current.Resources["appcolor_i1"];
                        item.ImageSource = selectedItemImageSource[item.Id];
                    }
                    else
                    {
                        item.TextColor = (Color)Prism.PrismApplicationBase.Current.Resources["appcolor_i3"];
                        item.ImageSource = selectedItemImageSource[item.Id + 4];
                    }
                }
            }
        }

        private async void OnItemTapCommand(object param)
        {
            var menuItem = param as MenuItemViewModel;
            MessagingCenter.Send(this, Constants.Messages.OPEN_SIDEBAR, false);
            MessagingCenter.Send(this, Constants.Messages.TAB_SELECTED, menuItem.Id);
            await NavigationService.NavigateAsync(nameof(menuItem.TargetType));
        }

        private async void UpdateAsync(object sender)
        {
            var userResponse = await _userService.GetUserAsync(_settingsManager.UserId);
            if (userResponse.IsSuccess)
            {
                var user = userResponse.Result;
                ProfileName = user.Name;
                ProfileEmail = user.Email;
                UserImagePath = user.AvatarPath;
            }
        }

        private async Task OnLogoutCommandAsync()
        {
            _settingsManager.UserId = 0;

            await NavigationService.NavigateAsync($"/{nameof(LogInPage)}");
        }

        private async Task OnNavigateProfileCommandAsync()
        {
            var param = new NavigationParameters();
            param.Add(Constants.Navigation.CURRENT_USER, _user);
            await NavigationService.NavigateAsync(nameof(ProfilePage), param);
            MessagingCenter.Send(this, Constants.Messages.OPEN_SIDEBAR, false);
        }

        private async Task OnNavigateEditProfileCommandAsync()
        {
            await NavigationService.NavigateAsync(nameof(EditProfilePage));
            MessagingCenter.Send(this, Constants.Messages.OPEN_SIDEBAR, false);
        }

        #endregion

    }
}
