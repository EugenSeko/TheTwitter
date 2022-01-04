using InterTwitter.Enums;
using InterTwitter.Helpers;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace InterTwitter.Models.TweetViewModel
{
    public class BaseTweetViewModel : BindableBase
    {
        public BaseTweetViewModel()
        {
            Mode = EStateMode.Truncated;
        }

        #region -- Public properties --

        private EStateMode _mode;
        public EStateMode Mode
        {
            get => _mode;
            set => SetProperty(ref _mode, value);
        }

        private int _tweetId;
        public int TweetId
        {
            get => _tweetId;
            set => SetProperty(ref _tweetId, value);
        }

        private int _userId;
        public int UserId
        {
            get => _userId;
            set => SetProperty(ref _userId, value);
        }

        private string _userName;
        public string UserName
        {
            get => _userName;
            set => SetProperty(ref _userName, value);
        }

        private string _userAvatar;
        public string UserAvatar
        {
            get => _userAvatar;
            set => SetProperty(ref _userAvatar, value);
        }

        private string _userBackgroundImage;
        public string UserBackgroundImage
        {
            get => _userBackgroundImage;
            set => SetProperty(ref _userBackgroundImage, value);
        }

        private string _text;
        public string Text
        {
            get => _text;
            set
            {
                SetProperty(ref _text, value);
                RaisePropertyChanged(nameof(IsTextVisible));
            }
        }

        private IEnumerable<string> _keysToHighlight;
        public IEnumerable<string> KeysToHighlight
        {
            get => _keysToHighlight;
            set => SetProperty(ref _keysToHighlight, value);
        }

        public bool IsTextVisible => !string.IsNullOrEmpty(Text);

        private IEnumerable<string> _mediaPaths;
        public IEnumerable<string> MediaPaths
        {
            get => _mediaPaths;
            set => SetProperty(ref _mediaPaths, value);
        }

        private EAttachedMediaType _mediaType;
        public EAttachedMediaType Media
        {
            get => _mediaType;
            set => SetProperty(ref _mediaType, value);
        }

        private int _likesNumber;
        public int LikesNumber
        {
            get => _likesNumber;
            set => SetProperty(ref _likesNumber, value);
        }

        private bool _IsTweetLiked;
        public bool IsTweetLiked
        {
            get => _IsTweetLiked;
            set => SetProperty(ref _IsTweetLiked, value);
        }

        private bool _isBookmarked;
        public bool IsBookmarked
        {
            get => _isBookmarked;
            set => SetProperty(ref _isBookmarked, value, nameof(IsBookmarked));
        }

        private ICommand _likeTweetCommand;
        public ICommand LikeTweetCommand => _likeTweetCommand ?? (_likeTweetCommand = SingleExecutionCommand.FromFunc<ImagesTweetViewModel>(OnLikeAsync));

        private ICommand _openTweetCommand;
        public ICommand OpenTweetCommand => _openTweetCommand ?? (_openTweetCommand = SingleExecutionCommand.FromFunc<ImagesTweetViewModel>(OnOpenTweetAsync));

        private ICommand _markTweetCommand;
        public ICommand MarkTweetCommand => _markTweetCommand ?? (_markTweetCommand = SingleExecutionCommand.FromFunc<BaseTweetViewModel>(OnMarkAsync));

        //private ICommand _moveToProfileCommand;
        //public ICommand MoveToProfileCommand => _moveToProfileCommand ?? (_moveToProfileCommand = SingleExecutionCommand.FromFunc<BaseTweetViewModel>(OnGoToProfileAsync));
        private ICommand _moveToProfileCommand;
        public ICommand MoveToProfileCommand
        {
            get => _moveToProfileCommand;
            set => SetProperty(ref _moveToProfileCommand, value);
        }

        private DateTime _CreationTime;
        public DateTime CreationTime
        {
            get => _CreationTime;
            set => SetProperty(ref _CreationTime, value);
        }
        #endregion
        #region -- Private helpers --
        private Task OnLikeAsync(BaseTweetViewModel tweet)
        {
            IsTweetLiked = !IsTweetLiked;
            if (IsTweetLiked)
            {
                MessagingCenter.Send<MessageEvent>(new MessageEvent(TweetId), MessageEvent.AddLike);
            }
            else
            {
                MessagingCenter.Send<MessageEvent>(new MessageEvent(TweetId), MessageEvent.DeleteLike);
            }

            return Task.CompletedTask;
        }

        private Task OnOpenTweetAsync(BaseTweetViewModel arg)
        {
            return Task.CompletedTask;
        }

        private Task OnMarkAsync(BaseTweetViewModel arg)
        {
            IsBookmarked = !IsBookmarked;
            if (IsBookmarked)
            {
                MessagingCenter.Send<MessageEvent>(new MessageEvent(TweetId), MessageEvent.AddBookmark);
            }
            else
            {
                MessagingCenter.Send<MessageEvent>(new MessageEvent(TweetId), MessageEvent.DeleteBookmark);
            }

            return Task.CompletedTask;
        }

        private Task OnGoToProfileAsync(BaseTweetViewModel arg)
        {
            return Task.CompletedTask;
        }

        #endregion

    }
}
