using InterTwitter.Enums;
using InterTwitter.Extensions;
using InterTwitter.Helpers;
using InterTwitter.Models;
using InterTwitter.Models.TweetViewModel;
using InterTwitter.Services;
using Prism.Navigation;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.CommunityToolkit.Helpers;
using Xamarin.Forms;

namespace InterTwitter.ViewModels
{
    public class SearchPageViewModel : BaseTabViewModel
    {
        private readonly ITweetService _tweetService;
        private readonly IHashtagService _hashtagService;

        public SearchPageViewModel(
            INavigationService navigationService,
            ITweetService tweetService,
            IHashtagService hashtagManager)
            : base(navigationService)
        {
            _tweetService = tweetService;
            _hashtagService = hashtagManager;

            FoundTweets = new ObservableCollection<BaseTweetViewModel>();
            Hashtags = new ObservableCollection<HashtagModel>();

            IconPath = Prism.PrismApplicationBase.Current.Resources["ic_search_gray"] as ImageSource;
            AvatarIcon = "pic_profile_small";
        }

        #region -- Public properties --

        private string _avatarIcon;
        public string AvatarIcon
        {
            get => _avatarIcon;
            set => SetProperty(ref _avatarIcon, value);
        }

        private IEnumerable<string> _searchWords;
        public IEnumerable<string> SearchWords
        {
            get => _searchWords;
            set => SetProperty(ref _searchWords, value);
        }

        private string _queryString;
        public string QueryString
        {
            get => _queryString;
            set => SetProperty(ref _queryString, value);
        }

        private string _noResultsMessage;
        public string NoResultsMessage
        {
            get => _noResultsMessage;
            set => SetProperty(ref _noResultsMessage, value);
        }

        private HashtagModel _selectedHashtag;
        public HashtagModel SelectedHashtag
        {
            get => _selectedHashtag;
            set => SetProperty(ref _selectedHashtag, value);
        }

        private ObservableCollection<HashtagModel> _hashtags;
        public ObservableCollection<HashtagModel> Hashtags
        {
            get => _hashtags;
            set => SetProperty(ref _hashtags, value);
        }

        private ObservableCollection<BaseTweetViewModel> _foundTweets;
        public ObservableCollection<BaseTweetViewModel> FoundTweets
        {
            get => _foundTweets;
            set => SetProperty(ref _foundTweets, value);
        }

        private ESearchStatus _tweetsSearchStatus;
        public ESearchStatus TweetsSearchStatus
        {
            get => _tweetsSearchStatus;
            set => SetProperty(ref _tweetsSearchStatus, value);
        }

        private ESearchResult _tweetSearchResult;
        public ESearchResult TweetSearchResult
        {
            get => _tweetSearchResult;
            set => SetProperty(ref _tweetSearchResult, value);
        }

        private ICommand _openFlyoutCommand;
        public ICommand OpenFlyoutCommandAsync => _openFlyoutCommand ??= SingleExecutionCommand.FromFunc(OnOpenFlyoutCommandAsync);

        private ICommand _startTweetsSearchTapCommand;
        public ICommand StartTweetsSearchTapCommand => _startTweetsSearchTapCommand ??= SingleExecutionCommand.FromFunc(OnStartTweetsSearchTapCommandAsync);

        private ICommand _backToHashtagsTapCommand;
        public ICommand BackToHashtagsTapCommand => _backToHashtagsTapCommand ??= SingleExecutionCommand.FromFunc(OnBackToHashtagsTapCommandAsync);

        private ICommand _hashtagTapCommand;
        public ICommand HashtagTapCommand => _hashtagTapCommand ??= SingleExecutionCommand.FromFunc(OnHashtagTapCommandAsync);

        #endregion

        #region -- Overrides --

        public override void OnAppearing()
        {
            IconPath = Prism.PrismApplicationBase.Current.Resources["ic_search_blue"] as ImageSource;
        }

        public override void OnDisappearing()
        {
            IconPath = Prism.PrismApplicationBase.Current.Resources["ic_search_gray"] as ImageSource;
        }

        public override async void OnNavigatedTo(INavigationParameters parameters)
        {
            await LoadHashtagsAsync();

            base.OnNavigatedTo(parameters);
        }

        public override void OnNavigatedFrom(INavigationParameters parameters)
        {
            ResetSearchState();

            base.OnNavigatedFrom(parameters);
        }

        protected override void OnPropertyChanged(PropertyChangedEventArgs args)
        {
            base.OnPropertyChanged(args);

            switch (args.PropertyName)
            {
                case nameof(TweetsSearchStatus):
                    if (TweetsSearchStatus == ESearchStatus.NotActive)
                    {
                        ResetSearchState();
                    }

                    break;
            }
        }

        #endregion

        #region -- Public helpers --

        public void ResetSearchState()
        {
            QueryString = string.Empty;
            NoResultsMessage = string.Empty;
            TweetsSearchStatus = ESearchStatus.NotActive;
            FoundTweets.Clear();
        }

        #endregion

        #region -- Private helpers --

        private async Task LoadHashtagsAsync()
        {
            var getPopularHashtagsResult = await _hashtagService.GetPopularHashtags(Constants.Values.NUMBER_OF_POPULAR_HASHTAGS);

            if (getPopularHashtagsResult.IsSuccess)
            {
                Hashtags = new ObservableCollection<HashtagModel>(getPopularHashtagsResult.Result);
            }
        }

        private async Task InitTweetsForDisplayingAsync(IEnumerable<TweetModel> tweets)
        {
            var tweetViewModels = new List<BaseTweetViewModel>(
                tweets.Select(x => x.Media == EAttachedMediaType.Photos || x.Media == EAttachedMediaType.Gif
                    ? x.ToImagesTweetViewModel()
                    : x.ToBaseTweetViewModel()));

            foreach (var tweet in tweetViewModels)
            {
                var tweetAuthor = await _tweetService.GetAuthorAsync(tweet.UserId);

                if (tweetAuthor.IsSuccess)
                {
                    tweet.UserAvatar = tweetAuthor.Result.AvatarPath;
                    tweet.UserBackgroundImage = tweetAuthor.Result.BackgroundUserImagePath;
                    tweet.UserName = tweetAuthor.Result.Name;
                    tweet.KeysToHighlight = SearchWords;
                }
            }

            SearchWords = null;
            FoundTweets = new ObservableCollection<BaseTweetViewModel>(tweetViewModels);
        }

        private Task OnOpenFlyoutCommandAsync()
        {
            MessagingCenter.Send(this, Constants.Messages.OPEN_SIDEBAR, true);

            return Task.CompletedTask;
        }

        private Task OnStartTweetsSearchTapCommandAsync()
        {
            FindTweets(QueryString);

            return Task.CompletedTask;
        }

        private Task OnHashtagTapCommandAsync()
        {
            QueryString = SelectedHashtag.Text;

            FindTweets(QueryString);

            return Task.CompletedTask;
        }

        private Task OnBackToHashtagsTapCommandAsync()
        {
            TweetsSearchStatus = ESearchStatus.NotActive;

            ResetSearchState();

            return Task.CompletedTask;
        }

        private async void FindTweets(string queryString)
        {
            TweetsSearchStatus = ESearchStatus.Active;

            if (!string.IsNullOrWhiteSpace(queryString))
            {
                SearchWords = Constants.Methods.GetUniqueWords(queryString)
                    .Where(x => x.Length > 1);
            }

            if (SearchWords == null || SearchWords.Count() == 0)
            {
                TweetSearchResult = ESearchResult.NoResults;
                NoResultsMessage = LocalizationResourceManager.Current["InaccurateRequest"];
            }
            else
            {
                var getAllTweetsByKeywordsAsyncResult = await _tweetService.FindTweetsByKeywordsAsync(SearchWords);

                if (getAllTweetsByKeywordsAsyncResult.IsSuccess)
                {
                    TweetSearchResult = ESearchResult.Success;

                    await InitTweetsForDisplayingAsync(getAllTweetsByKeywordsAsyncResult.Result);
                }
                else
                {
                    TweetSearchResult = ESearchResult.NoResults;
                    NoResultsMessage = $"{LocalizationResourceManager.Current["NoResultsFor"]}\n\"{queryString}\"";
                }
            }
        }

        #endregion
    }
}
