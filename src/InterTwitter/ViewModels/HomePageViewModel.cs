using InterTwitter.Enums;
using InterTwitter.Extensions;
using InterTwitter.Helpers;
using InterTwitter.Models;
using InterTwitter.Models.TweetViewModel;
using InterTwitter.Services;
using InterTwitter.Views;
using Prism.Navigation;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace InterTwitter.ViewModels
{
    public class HomePageViewModel : BaseTabViewModel
    {
        private readonly ISettingsManager _settingsManager;
        private readonly ITweetService _tweetService;
        private readonly IBookmarkService _bookmarkService;
        private readonly ILikeService _likeService;
        private readonly IUserService _userService;
        private readonly IRegistrationService _registrationService;

        private UserModel _currentUser;
        private int _userId;

        public HomePageViewModel(
            INavigationService navigationService,
            ISettingsManager settingsManager,
            IBookmarkService bookmarkService,
            ILikeService likeService,
            ITweetService tweetService,
            IUserService userService,
            IRegistrationService registrationService)
            : base(navigationService)
        {
            _settingsManager = settingsManager;
            _bookmarkService = bookmarkService;
            _likeService = likeService;
            _tweetService = tweetService;
            _userService = userService;
            _registrationService = registrationService;

            IconPath = Prism.PrismApplicationBase.Current.Resources["ic_home_gray"] as ImageSource;
        }

        #region -- Public properties --
        private ObservableCollection<BaseTweetViewModel> _tweets;
        public ObservableCollection<BaseTweetViewModel> Tweets
        {
            get => _tweets;
            set => SetProperty(ref _tweets, value);
        }

        private ICommand _openFlyoutCommandAsync;
        public ICommand OpenFlyoutCommandAsync => _openFlyoutCommandAsync ?? (_openFlyoutCommandAsync = SingleExecutionCommand.FromFunc(OnOpenFlyoutCommandAsync));

        private ICommand _addTweetCommandAsync;
        public ICommand AddTweetCommandAsync => _addTweetCommandAsync ?? (_addTweetCommandAsync = SingleExecutionCommand.FromFunc(OnOpenAddTweetPageAsync));

        #endregion

        #region -- Overrides --

        public override async void OnAppearing()
        {
            IconPath = App.Current.Resources["ic_home_blue"] as ImageSource;
            await InitAsync();
        }

        public override void OnDisappearing()
        {
            IconPath = Prism.PrismApplicationBase.Current.Resources["ic_home_gray"] as ImageSource;

            MessagingCenter.Unsubscribe<MessageEvent>(this, MessageEvent.AddBookmark);
            MessagingCenter.Unsubscribe<MessageEvent>(this, MessageEvent.DeleteBookmark);
            MessagingCenter.Unsubscribe<MessageEvent>(this, MessageEvent.AddLike);
            MessagingCenter.Unsubscribe<MessageEvent>(this, MessageEvent.DeleteLike);
        }

        #endregion
        #region -- Private helpers --
        private async Task InitAsync()
        {
            _userId = _settingsManager.UserId;
            var result = await _registrationService.GetByIdAsync(_userId);

            if (result.IsSuccess)
            {
                _currentUser = result.Result;
                var getTweetResult = await _tweetService.GetAllTweetsAsync();
                var blocked = _userService.GetAllBlockedUsersAsync().Result;

                if (getTweetResult.IsSuccess)
                {
                    List<BaseTweetViewModel> tweetViewModels;
                    if (blocked.Result != null)
                    {
                         tweetViewModels = new List<BaseTweetViewModel>(getTweetResult.Result
                        .Select(x => x.Media == EAttachedMediaType.Photos || x.Media == EAttachedMediaType.Gif ? x.ToImagesTweetViewModel() : x.ToBaseTweetViewModel())
                        .Where(t => blocked.Result.All(u => u.Id != t.UserId)));
                    }
                    else
                    {
                        tweetViewModels = new List<BaseTweetViewModel>(getTweetResult.Result
                       .Select(x => x.Media == EAttachedMediaType.Photos || x.Media == EAttachedMediaType.Gif ? x.ToImagesTweetViewModel() : x.ToBaseTweetViewModel()));
                    }

                    foreach (var tweet in tweetViewModels)
                    {
                        var user = await _userService.GetUserAsync(tweet.UserId);
                        var tweetAuthor = await _tweetService.GetAuthorAsync(tweet.UserId);

                        if (tweetAuthor.IsSuccess)
                        {
                            tweet.UserAvatar = tweetAuthor.Result.AvatarPath;
                            tweet.UserBackgroundImage = tweetAuthor.Result.BackgroundUserImagePath;
                            tweet.UserName = tweetAuthor.Result.Name;

                            tweet.IsBookmarked = (await _bookmarkService.AnyAsync(tweet.TweetId, _userId)).IsSuccess;
                            tweet.IsTweetLiked = (await _likeService.AnyAsync(tweet.TweetId, _userId)).IsSuccess;

                            var resultLike = await _likeService.CountAsync(tweet.TweetId);
                            if (resultLike.IsSuccess)
                            {
                                tweet.LikesNumber = resultLike.Result;
                            }

                            if (tweetAuthor.Result.Id == _currentUser.Id)
                            {
                                tweet.MoveToProfileCommand = new Command(() =>
                                NavigationService.NavigateAsync(nameof(ProfilePage), new NavigationParameters
                                { { Constants.Navigation.CURRENT_USER, user.Result } }));
                            }
                            else
                            {
                                tweet.MoveToProfileCommand = new Command(() =>
                                NavigationService.NavigateAsync(nameof(ProfilePage), new NavigationParameters
                                { { Constants.Navigation.USER, user.Result } }));
                            }
                        }
                    }

                    Tweets = new ObservableCollection<BaseTweetViewModel>(tweetViewModels);

                    MessagingCenter.Subscribe<MessageEvent>(this, MessageEvent.AddBookmark, (me) => AddBookmarkAsync(me));
                    MessagingCenter.Subscribe<MessageEvent>(this, MessageEvent.DeleteBookmark, (me) => DeleteBookmarkAsync(me));
                    MessagingCenter.Subscribe<MessageEvent>(this, MessageEvent.AddLike, (me) => AddLikeAsync(me));
                    MessagingCenter.Subscribe<MessageEvent>(this, MessageEvent.DeleteLike, (me) => DeleteLikeAsync(me));
                }
            }
        }

        private async void AddBookmarkAsync(MessageEvent me)
        {
            var result = await _bookmarkService.AddBookmarkAsync(me.UnTweetId, _userId);
        }

        private async void DeleteBookmarkAsync(MessageEvent me)
        {
            var result = await _bookmarkService.DeleteBoormarkAsync(me.UnTweetId, _userId);
        }

        private async void AddLikeAsync(MessageEvent me)
        {
            var resultAdd = await _likeService.AddLikeAsync(me.UnTweetId, _userId);
            var result = await _likeService.CountAsync(me.UnTweetId);
            if (result.IsSuccess)
            {
                var tweet = Tweets.FirstOrDefault(x => x.TweetId == me.UnTweetId);
                if (tweet != null)
                {
                    tweet.LikesNumber = result.Result;
                }
            }
        }

        private async void DeleteLikeAsync(MessageEvent me)
        {
            var resultAdd = await _likeService.DeleteLikeAsync(me.UnTweetId, _userId);
            var result = await _likeService.CountAsync(me.UnTweetId);
            if (result.IsSuccess)
            {
                var tweet = Tweets.FirstOrDefault(x => x.TweetId == me.UnTweetId);
                tweet.LikesNumber = result.Result;
            }
        }

        private Task OnOpenAddTweetPageAsync()
        {
            return NavigationService.NavigateAsync(nameof(CreateTweetPage), useModalNavigation: true);
        }

        private Task OnOpenFlyoutCommandAsync()
        {
            MessagingCenter.Send(this, Constants.Messages.OPEN_SIDEBAR, true);
            MessagingCenter.Send(this, Constants.Messages.TAB_CHANGE, typeof(HomePage));
            return Task.CompletedTask;
        }

        #endregion
    }
}