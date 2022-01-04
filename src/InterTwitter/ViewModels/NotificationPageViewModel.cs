using InterTwitter.Enums;
using InterTwitter.Extensions;
using InterTwitter.Helpers;
using InterTwitter.Models;
using InterTwitter.Models.NotificationViewModel;
using InterTwitter.Services;
using InterTwitter.Views;
using Prism.Navigation;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace InterTwitter.ViewModels
{
    public class NotificationPageViewModel : BaseTabViewModel
    {
        private readonly INotificationService _notificationService;

        private readonly ISettingsManager _settingsManager;

        private readonly IRegistrationService _registrationService;

        private readonly IUserService _userService;

        private UserModel _currentUser;
        private int _userId;

        public NotificationPageViewModel(
            INotificationService notificationService,
            INavigationService navigationService,
            IUserService userService,
            ISettingsManager settingsManager,
            IRegistrationService registrationService)
            : base(navigationService)
        {
            IconPath = Prism.PrismApplicationBase.Current.Resources["ic_notifications_gray"] as ImageSource;
            _notificationService = notificationService;
            _settingsManager = settingsManager;
            _registrationService = registrationService;
            _userService = userService;
        }

        #region -- Public Properties --

        private bool _IsNotFound;

        public bool IsNotFound
        {
            get => _IsNotFound;
            set => SetProperty(ref _IsNotFound, value);
        }

        private ObservableCollection<BaseNotificationViewModel> _tweets;

        public ObservableCollection<BaseNotificationViewModel> Tweets
        {
            get => _tweets;
            set => SetProperty(ref _tweets, value);
        }

        private ICommand _openFlyoutCommandAsync;

        public ICommand OpenFlyoutCommandAsync => _openFlyoutCommandAsync ?? (_openFlyoutCommandAsync = SingleExecutionCommand.FromFunc(OnOpenFlyoutCommandAsync));

        #endregion

        #region -- Overrides --

        protected override void OnPropertyChanged(PropertyChangedEventArgs args)
        {
            base.OnPropertyChanged(args);

            if (args.PropertyName == nameof(Tweets))
            {
                IsNotFound = Tweets == null || Tweets.Count == 0;
            }
        }

        public override void OnAppearing()
        {
            IconPath = Prism.PrismApplicationBase.Current.Resources["ic_notifications_blue"] as ImageSource;
        }

        public override void OnDisappearing()
        {
            IconPath = Prism.PrismApplicationBase.Current.Resources["ic_notifications_gray"] as ImageSource;
        }

        #endregion

        #region -- Private Helpers --

        private Task OnOpenFlyoutCommandAsync()
        {
            MessagingCenter.Send(this, Constants.Messages.OPEN_SIDEBAR, true);
            MessagingCenter.Send(this, Constants.Messages.TAB_CHANGE, typeof(NotificationsPage));

            return Task.CompletedTask;
        }

        public override async void OnNavigatedTo(INavigationParameters parameters)
        {
            _userId = _settingsManager.UserId;
            var result = await _registrationService.GetByIdAsync(_userId);
            var mutedUsers = await _userService.GetAllMutedUsersAsync();
            var blockedUsers = await _userService.GetAllBlockedUsersAsync();

            if (result.IsSuccess)
            {
                _currentUser = result.Result;

                var resultNotification = await _notificationService.GetNotificationsAsync(_userId);
                if (mutedUsers.IsSuccess && mutedUsers.Result != null && blockedUsers.IsSuccess && blockedUsers.Result != null)
                {
                    Tweets = new ObservableCollection<BaseNotificationViewModel>(resultNotification.Result
                            .Select(x => x.Media == EAttachedMediaType.Photos || x.Media == EAttachedMediaType.Gif ? x.ToImagesNotificationViewModel() : x.ToBaseNotificationViewModel())
                            .Where(t => mutedUsers.Result.All(u => u.Id != t.UserId))
                            .Where(t => blockedUsers.Result.All(u => u.Id != t.UserId))
                            .OrderByDescending(x => x.CreationTime));
                }
                else if (mutedUsers.IsSuccess && mutedUsers.Result != null)
                {
                    Tweets = new ObservableCollection<BaseNotificationViewModel>(resultNotification.Result
                           .Select(x => x.Media == EAttachedMediaType.Photos || x.Media == EAttachedMediaType.Gif ? x.ToImagesNotificationViewModel() : x.ToBaseNotificationViewModel())
                           .Where(t => mutedUsers.Result.All(u => u.Id != t.UserId))
                           .OrderByDescending(x => x.CreationTime));
                }
                else if (blockedUsers.IsSuccess && blockedUsers.Result != null)
                {
                    Tweets = new ObservableCollection<BaseNotificationViewModel>(resultNotification.Result
                           .Select(x => x.Media == EAttachedMediaType.Photos || x.Media == EAttachedMediaType.Gif ? x.ToImagesNotificationViewModel() : x.ToBaseNotificationViewModel())
                           .Where(t => blockedUsers.Result.All(u => u.Id != t.UserId))
                           .OrderByDescending(x => x.CreationTime));
                }
                else if (resultNotification.IsSuccess)
                {
                    Tweets = new ObservableCollection<BaseNotificationViewModel>(resultNotification.Result
                            .Select(x => x.Media == EAttachedMediaType.Photos || x.Media == EAttachedMediaType.Gif ? x.ToImagesNotificationViewModel() : x.ToBaseNotificationViewModel())
                                                .OrderByDescending(x => x.CreationTime));
                }
            }

            #endregion

        }
    }
}