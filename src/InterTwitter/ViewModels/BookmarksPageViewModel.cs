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
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace InterTwitter.ViewModels
{
    public class BookmarksPageViewModel : BaseTabViewModel
    {
        private readonly ITweetService _tweetService;
        private readonly ISettingsManager _settingsManager;
        private readonly IRegistrationService _registrationService;
        private readonly IBookmarkService _bookmarkService;
        private readonly ILikeService _likeService;

        private UserModel _currentUser;
        private int _userId;

        public BookmarksPageViewModel(
            INavigationService navigationService,
            ITweetService tweetService,
            ILikeService likeService,
            IBookmarkService bookmarkService,
            ISettingsManager settingsManager,
            IRegistrationService registrationService)
            : base(navigationService)
        {
            _tweetService = tweetService;
            _likeService = likeService;
            _bookmarkService = bookmarkService;
            _settingsManager = settingsManager;
            _registrationService = registrationService;
            IconPath = Prism.PrismApplicationBase.Current.Resources["ic_bookmarks_gray"] as ImageSource;
        }

        #region -- Public properties --

        private bool _IsNotFound;
        public bool IsNotFound
        {
            get => _IsNotFound;
            set => SetProperty(ref _IsNotFound, value);
        }

        private bool _IsVisibleButton = false;
        public bool IsVisibleButton
        {
            get => _IsVisibleButton;
            set => SetProperty(ref _IsVisibleButton, value);
        }

        private string _imageButtonSource = "ic_hidden_menu_gray";
        public string ImageButtonSource
        {
            get => _imageButtonSource;
            set => SetProperty(ref _imageButtonSource, value);
        }

        private ObservableCollection<BaseTweetViewModel> _tweets;

        public ObservableCollection<BaseTweetViewModel> Tweets
        {
            get => _tweets;
            set => SetProperty(ref _tweets, value);
        }

        private ICommand _VisibleButtonCommand;

        public ICommand VisibleButtonCommand => _VisibleButtonCommand ??= SingleExecutionCommand.FromFunc(OnVisibleButtonCommandAsync);

        private ICommand _UnvisibleButtonCommand;

        public ICommand UnvisibleButtonCommand => _UnvisibleButtonCommand ??= SingleExecutionCommand.FromFunc(OnUnvisibleButtonCommandAsync);

        private ICommand _DeleteAllBookmarks;

        public ICommand DeleteAllBookmarks => _DeleteAllBookmarks ??= SingleExecutionCommand.FromFunc(OnDeleteAllBookmarksCommandAsync);

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
                if (IsNotFound)
                {
                    ImageButtonSource = string.Empty;
                }
                else
                {
                    ImageButtonSource = "ic_hidden_menu_gray";
                }
            }
        }

        public override void OnAppearing()
        {
            IconPath = Prism.PrismApplicationBase.Current.Resources["ic_bookmarks_blue"] as ImageSource;
        }

        public override void OnDisappearing()
        {
            IconPath = Prism.PrismApplicationBase.Current.Resources["ic_bookmarks_gray"] as ImageSource;

            MessagingCenter.Unsubscribe<MessageEvent>(this, MessageEvent.DeleteBookmark);
            MessagingCenter.Unsubscribe<MessageEvent>(this, MessageEvent.AddLike);
            MessagingCenter.Unsubscribe<MessageEvent>(this, MessageEvent.DeleteLike);
        }

        public override async void OnNavigatedTo(INavigationParameters parameters)
        {
            _userId = _settingsManager.UserId;
            var result = await _registrationService.GetByIdAsync(_userId);

            if (result.IsSuccess)
            {
                _currentUser = result.Result;
                var resultTweet = await _tweetService.GetAllTweetsAsync();
                var resultBookmark = await _bookmarkService.GetBookmarksAsync(_userId);
                var getTweetResult = resultTweet.Result.ToList();
                var getBookmarks = resultBookmark.Result;

                if (resultTweet.IsSuccess && resultBookmark.IsSuccess)
                {
                    var tweetViewModels = new List<BaseTweetViewModel>(getTweetResult.Where(x => getBookmarks.Any(y => y.TweetId == x.Id))
                        .Select(x => x.Media == EAttachedMediaType.Photos || x.Media == EAttachedMediaType.Gif ? x.ToImagesTweetViewModel() : x.ToBaseTweetViewModel())
                        .OrderByDescending(x => x.CreationTime));

                    foreach (var tweet in tweetViewModels)
                    {
                        var tweetAuthor = await _tweetService.GetAuthorAsync(tweet.UserId);

                        if (tweetAuthor.IsSuccess)
                        {
                            tweet.UserAvatar = tweetAuthor.Result.AvatarPath;
                            tweet.UserBackgroundImage = tweetAuthor.Result.BackgroundUserImagePath;
                            tweet.UserName = tweetAuthor.Result.Name;
                            tweet.IsTweetLiked = (await _likeService.AnyAsync(tweet.TweetId, _userId)).IsSuccess;
                            var resultLike = await _likeService.CountAsync(tweet.TweetId);
                            if (resultLike.IsSuccess)
                            {
                                tweet.LikesNumber = resultLike.Result;
                            }
                        }

                        tweet.IsBookmarked = true;
                    }

                    Tweets = new ObservableCollection<BaseTweetViewModel>(tweetViewModels);

                    MessagingCenter.Subscribe<MessageEvent>(this, MessageEvent.DeleteBookmark, (me) => DeleteBookmarkAsync(me));
                    MessagingCenter.Subscribe<MessageEvent>(this, MessageEvent.AddLike, (me) => AddLikeAsync(me));
                    MessagingCenter.Subscribe<MessageEvent>(this, MessageEvent.DeleteLike, (me) => DeleteLikeAsync(me));
                }
            }
        }

        #endregion

        #region -- Private helpers --

        private async void DeleteBookmarkAsync(MessageEvent me)
        {
            var result = await _bookmarkService.DeleteBoormarkAsync(me.UnTweetId, _userId);
            if (result.IsSuccess)
            {
                var tweet = Tweets.FirstOrDefault(x => x.TweetId == me.UnTweetId);
                Tweets.Remove(tweet);
            }
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

        private Task OnOpenFlyoutCommandAsync()
        {
            MessagingCenter.Send(this, Constants.Messages.OPEN_SIDEBAR, true);
            MessagingCenter.Send(this, Constants.Messages.TAB_CHANGE, typeof(BookmarksPage));
            return Task.CompletedTask;
        }

        private Task OnVisibleButtonCommandAsync()
        {
            if (!IsNotFound)
            {
                IsVisibleButton = true;
            }

            return Task.CompletedTask;
        }

        private Task OnUnvisibleButtonCommandAsync()
        {
            IsVisibleButton = false;

            return Task.CompletedTask;
        }

        private async Task OnDeleteAllBookmarksCommandAsync()
        {
            var result = await _bookmarkService.DeleteAllBookmarksAsync(_userId);
            if (result.IsSuccess)
            {
                Tweets = new ();
            }

            IsVisibleButton = false;
        }

        #endregion
    }
}
