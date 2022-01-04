using InterTwitter.Enums;
using InterTwitter.Helpers;
using InterTwitter.Models;
using InterTwitter.Resources.Strings;
using InterTwitter.Services;
using InterTwitter.Services.Video;
using InterTwitter.Views;
using Prism.Navigation;
using Prism.Services;
using Prism.Services.Dialogs;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.CommunityToolkit.Helpers;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace InterTwitter.ViewModels
{
    public class CreateTweetPageViewModel : BaseViewModel
    {
        private IPageDialogService _pageDialogService;

        private IPermissionService _permissionService;

        private IVideoService _videoService;

        private ITweetService _tweetService;

        private ISettingsManager _settingsManager;

        private IRegistrationService _registrationService;

        private int _userId;

        public CreateTweetPageViewModel(
            INavigationService navigationService,
            IPermissionService permissionService,
            IPageDialogService pageDialogService,
            IVideoService videoService,
            ITweetService tweetService,
            ISettingsManager settingsManager,
            IRegistrationService registrationService)
            : base(navigationService)
        {
            _permissionService = permissionService;
            _pageDialogService = pageDialogService;
            _videoService = videoService;
            _tweetService = tweetService;
            _settingsManager = settingsManager;
            _registrationService = registrationService;

            _attachedMediaFiles = new ObservableCollection<MiniCardModel>();
        }

        #region -- Public properties --

        private string _text;
        public string Text
        {
            get => _text;
            set => SetProperty(ref _text, value);
        }

        private string _circleProgressBarText;
        public string CircleProgressBarText
        {
            get => _circleProgressBarText;
            set => SetProperty(ref _circleProgressBarText, value);
        }

        private Color _circleProgressBarTextColor;
        public Color CircleProgressBarTextColor
        {
            get => _circleProgressBarTextColor;
            set => SetProperty(ref _circleProgressBarTextColor, value);
        }

        private float _circleProgressBarFontScale;
        public float CircleProgressBarFontScale
        {
            get => _circleProgressBarFontScale;
            set => SetProperty(ref _circleProgressBarFontScale, value);
        }

        private SKTypefaceStyle _circleProgressBarFontAttributes;
        public SKTypefaceStyle CircleProgressBarFontAttributes
        {
            get => _circleProgressBarFontAttributes;
            set => SetProperty(ref _circleProgressBarFontAttributes, value);
        }

        private int _circleProgressBarvalue;
        public int CircleProgressBarValue
        {
            get => _circleProgressBarvalue;
            set => SetProperty(ref _circleProgressBarvalue, value);
        }

        private Color _circleProgressBarProgressLineColor = Color.Blue;
        public Color CircleProgressBarProgressLineColor
        {
            get => _circleProgressBarProgressLineColor;
            set => SetProperty(ref _circleProgressBarProgressLineColor, value);
        }

        private bool _isButtonUploadPhotosEnabled = true;
        public bool IsButtonUploadPhotosEnabled
        {
            get => _isButtonUploadPhotosEnabled;
            set => SetProperty(ref _isButtonUploadPhotosEnabled, value);
        }

        private bool _isButtonUploadGifEnabled = true;
        public bool IsButtonUploadGifEnabled
        {
            get => _isButtonUploadGifEnabled;
            set => SetProperty(ref _isButtonUploadGifEnabled, value);
        }

        private bool _isButtonUploadVideoEnabled = true;
        public bool IsButtonUploadVideoEnabled
        {
            get => _isButtonUploadVideoEnabled;
            set => SetProperty(ref _isButtonUploadVideoEnabled, value);
        }

        private bool _isButtonPostEnabled;
        public bool IsButtonPostEnabled
        {
            get => _isButtonPostEnabled;
            set => SetProperty(ref _isButtonPostEnabled, value);
        }

        private bool _isActivityIndicatorRunning;
        public bool IsActivityIndicatorRunning
        {
            get => _isActivityIndicatorRunning;
            set => SetProperty(ref _isActivityIndicatorRunning, value);
        }

        private EAttachedMediaType _attachedMediaType = EAttachedMediaType.None;
        public EAttachedMediaType AttachedMediaType
        {
            get => _attachedMediaType;
            set => SetProperty(ref _attachedMediaType, value);
        }

        private ObservableCollection<MiniCardModel> _attachedMediaFiles;
        public ObservableCollection<MiniCardModel> AttachedMediaFiles
        {
            get => _attachedMediaFiles;
            set => SetProperty(ref _attachedMediaFiles, value);
        }

        private string _avatarPath = "pic_profile_small.png";
        public string AvatarPath
        {
            get => _avatarPath;
            set => SetProperty(ref _avatarPath, value);
        }

        private ICommand _goBackCommand;
        public ICommand GoBackCommand => _goBackCommand = SingleExecutionCommand.FromFunc(OnGoBackCommandAsync);

        private ICommand _postTweetCommand;
        public ICommand PostTweetCommand => _postTweetCommand = SingleExecutionCommand.FromFunc(OnPostTweetCommandAsync);

        private ICommand _deleteAttachedPhotoCommand;
        public ICommand DeleteAttachedPhotoCommand => _deleteAttachedPhotoCommand = SingleExecutionCommand.FromFunc<MiniCardModel>(OnDeleteAttachedPhotoCommandAsync);

        private ICommand _deleteAttachedGifCommand;
        public ICommand DeleteAttachedGifCommand => _deleteAttachedGifCommand = SingleExecutionCommand.FromFunc(OnDeleteAttachedGifCommandAsync);

        private ICommand _deleteAttachedVideoCommand;
        public ICommand DeleteAttachedVideoCommand => _deleteAttachedVideoCommand = SingleExecutionCommand.FromFunc(OnDeleteAttachedVideoCommandAsync);

        private ICommand _addPhotoCommand;
        public ICommand AddPhotoCommand => _addPhotoCommand = SingleExecutionCommand.FromFunc(OnAddPhotoCommandAsync);

        private ICommand _addGifCommand;
        public ICommand AddGifCommand => _addGifCommand = SingleExecutionCommand.FromFunc(OnAddGifCommandAsync);

        private ICommand _addVideoCommand;
        public ICommand AddVideoCommand => _addVideoCommand = SingleExecutionCommand.FromFunc(OnAddVideoCommandAsync);

        #endregion

        #region -- IInitializeAsync implementation --

        public async override Task InitializeAsync(INavigationParameters parameters)
        {
            _userId = _settingsManager.UserId;
            var userModel = await _registrationService.GetByIdAsync(_userId);

            if (userModel.IsSuccess)
            {
                AvatarPath = userModel.Result.AvatarPath;
            }
        }

        #endregion

        #region -- Overrides --

        protected override void OnPropertyChanged(PropertyChangedEventArgs args)
        {
            base.OnPropertyChanged(args);

            switch (args.PropertyName)
            {
                case nameof(Text):
                    IsButtonPostEnabled = CheckPossibilityPostTweet();
                    Counter();
                    break;
            }
        }

        #endregion

        #region -- Private methods --

        private async Task OnGoBackCommandAsync()
        {
            if ((!string.IsNullOrEmpty(Text) && Text.Length > 0) || AttachedMediaFiles.Count > 0)
            {
                var confirm = await _pageDialogService.DisplayAlertAsync(Strings.Confirm, Strings.ConfirmComeBack, Strings.Ok, Strings.Cancel);

                if (confirm)
                {
                    await NavigationService.GoBackAsync();
                }
            }
            else
            {
                await NavigationService.GoBackAsync();
            }
        }

        private async Task OnPostTweetCommandAsync()
        {
            var list = new List<string>();

            foreach (var card in _attachedMediaFiles)
            {
                list.Add(card.FilePath);
            }

            var newTweet = new TweetModel()
            {
                UserId = _userId,
                Text = Text,
                Media = _attachedMediaType,
                MediaPaths = list,
                CreationTime = DateTime.Now,
            };

            await _tweetService.AddTweetAsync(newTweet);
            await NavigationService.GoBackAsync();
        }

        private async Task OnDeleteAttachedPhotoCommandAsync(MiniCardModel item)
        {
            AttachedMediaFiles.Remove(item);

            IsButtonUploadPhotosEnabled = AttachedMediaFiles.Count < Constants.Limits.MAX_COUNT_ATTACHED_PHOTOS;

            if (AttachedMediaFiles.Count == 0)
            {
                ClearAttachedMedia();
            }
            else
            {
                IsButtonPostEnabled = CheckPossibilityPostTweet();
            }
        }

        private async Task OnDeleteAttachedGifCommandAsync()
        {
            ClearAttachedMedia();
        }

        private async Task OnDeleteAttachedVideoCommandAsync()
        {
            ClearAttachedMedia();
        }

        private async Task OnAddVideoCommandAsync()
        {
            var canUseStorage = await _permissionService.RequestAsync<Permissions.StorageRead>() == Xamarin.Essentials.PermissionStatus.Granted
                            && await _permissionService.RequestAsync<Permissions.StorageWrite>() == Xamarin.Essentials.PermissionStatus.Granted;

            if (canUseStorage)
            {
                if (await TryPickVideoAsync() is FileResult openFile)
                {
                    IsActivityIndicatorRunning = true;

                    var result = await _videoService.ProcessingVideoForPostAsync(openFile.FullPath);

                    if (result.IsSuccess)
                    {
                        AttachedMediaFiles.Add(new MiniCardModel()
                        {
                            FilePath = result.Result.FrameFilePath,
                        });

                        AttachedMediaFiles.Add(new MiniCardModel()
                        {
                            FilePath = result.Result.VideoFilePath,
                        });

                        IsButtonUploadPhotosEnabled = false;
                        IsButtonUploadGifEnabled = false;
                        IsButtonUploadVideoEnabled = false;

                        AttachedMediaType = EAttachedMediaType.Video;

                        IsActivityIndicatorRunning = false;

                        IsButtonPostEnabled = CheckPossibilityPostTweet();
                    }
                    else
                    {
                        string textMessage = string.Empty;

                        switch (result.Result.Message)
                        {
                            case EVideoProcessingResult.Error:
                                textMessage = Strings.AlertErrorTrimmingVideo;
                                break;
                            case EVideoProcessingResult.LimitSizeVideo:
                                textMessage = Strings.AlertLimitSizeVideo;
                                break;
                            case EVideoProcessingResult.FileNotExist:
                                textMessage = Strings.AlertFileNotExist;
                                break;
                        }

                        IsActivityIndicatorRunning = false;

                        var parametrs = new DialogParameters { { Constants.Navigation.MESSAGE, textMessage } };
                        await Rg.Plugins.Popup.Services.PopupNavigation.Instance.PushAsync(new AlertView(parametrs, CloseDialogCallback));
                    }
                }
            }
            else
            {
                var parametrs = new DialogParameters { { Constants.Navigation.MESSAGE, Strings.AlertNeedAccessPhotosGallery } };
                await Rg.Plugins.Popup.Services.PopupNavigation.Instance.PushAsync(new AlertView(parametrs, CloseDialogCallback));
            }
        }

        private async Task OnAddPhotoCommandAsync()
        {
            await OnAddPhotoOrGifAsync(EAttachedMediaType.Photos);
        }

        private async Task OnAddGifCommandAsync()
        {
            await OnAddPhotoOrGifAsync(EAttachedMediaType.Gif);
        }

        private async Task<FileResult> TryPickPhotoAsync()
        {
            FileResult result;

            try
            {
                result = await MediaPicker.PickPhotoAsync();
            }
            catch (Exception ex)
            {
                result = null;
            }

            return result;
        }

        private async Task<FileResult> TryPickVideoAsync()
        {
            FileResult result;

            try
            {
                result = await MediaPicker.PickVideoAsync();
            }
            catch (Exception ex)
            {
                result = null;
            }

            return result;
        }

        private async Task OnAddPhotoOrGifAsync(EAttachedMediaType attachedMediaType)
        {
            var canUseStorage = await _permissionService.RequestAsync<Permissions.StorageRead>() == Xamarin.Essentials.PermissionStatus.Granted;

            if (canUseStorage)
            {
                if (await MediaPicker.PickPhotoAsync() is FileResult openFile)
                {
                    var conditionFileType = attachedMediaType == EAttachedMediaType.Photos
                        ? openFile.ContentType != "image/gif"
                        : openFile.ContentType == "image/gif";

                    var alertFileType = attachedMediaType == EAttachedMediaType.Photos
                        ? Strings.AlertOnlyPicture
                        : Strings.AlertOnlyGif;

                    var alertFileSize = attachedMediaType == EAttachedMediaType.Photos
                        ? Strings.AlertLimitSizePhoto
                        : Strings.AlertLimitSizeGif;

                    var maxSize = attachedMediaType == EAttachedMediaType.Photos
                        ? Constants.Limits.MAX_COUNT_ATTACHED_PHOTOS
                        : Constants.Limits.MAX_COUNT_ATTACHED_GIF;

                    if (conditionFileType)
                    {
                        FileInfo fileInf = new FileInfo(openFile.FullPath);

                        if (fileInf.Exists)
                        {
                            if (fileInf.Length <= Constants.Limits.MAX_SIZE_ATTACHED_PHOTO)
                            {
                                AttachedMediaFiles.Add(new MiniCardModel()
                                {
                                    FilePath = openFile.FullPath,
                                    ActionCommand = DeleteAttachedPhotoCommand,
                                });

                                IsButtonUploadPhotosEnabled = AttachedMediaFiles.Count < maxSize;
                                IsButtonUploadGifEnabled = false;
                                IsButtonUploadVideoEnabled = false;

                                AttachedMediaType = attachedMediaType;

                                IsButtonPostEnabled = CheckPossibilityPostTweet();
                            }
                            else
                            {
                                var parametrs = new DialogParameters { { Constants.Navigation.MESSAGE, alertFileSize } };
                                await Rg.Plugins.Popup.Services.PopupNavigation.Instance.PushAsync(new AlertView(parametrs, CloseDialogCallback));
                            }
                        }
                        else
                        {
                            var parametrs = new DialogParameters { { Constants.Navigation.MESSAGE, Strings.AlertFileNotExist } };
                            await Rg.Plugins.Popup.Services.PopupNavigation.Instance.PushAsync(new AlertView(parametrs, CloseDialogCallback));
                        }
                    }
                    else
                    {
                        var parametrs = new DialogParameters { { Constants.Navigation.MESSAGE, alertFileType } };
                        await Rg.Plugins.Popup.Services.PopupNavigation.Instance.PushAsync(new AlertView(parametrs, CloseDialogCallback));
                    }
                }
            }
            else
            {
                var parametrs = new DialogParameters { { Constants.Navigation.MESSAGE, Strings.AlertNeedAccessPhotosGallery } };
                await Rg.Plugins.Popup.Services.PopupNavigation.Instance.PushAsync(new AlertView(parametrs, CloseDialogCallback));
            }
        }

        private void ClearAttachedMedia()
        {
            AttachedMediaFiles.Clear();

            IsButtonUploadPhotosEnabled = true;
            IsButtonUploadGifEnabled = true;
            IsButtonUploadVideoEnabled = true;

            AttachedMediaType = EAttachedMediaType.None;

            IsButtonPostEnabled = CheckPossibilityPostTweet();
        }

        private bool CheckPossibilityPostTweet() => !string.IsNullOrEmpty(Text)
            ? (AttachedMediaFiles.Count > 0 || Text.Length > 0) && Text.Length <= Constants.Limits.MAX_LENGTH_TEXT
            : AttachedMediaFiles.Count > 0;

        private void Counter()
        {
            var value = Text.Length;

            if (value > Constants.Limits.MAX_LENGTH_TEXT)
            {
                CircleProgressBarTextColor = Color.Red;
                CircleProgressBarFontScale = 0.7f;
                CircleProgressBarProgressLineColor = Color.Red;

                CircleProgressBarText = (Constants.Limits.MAX_LENGTH_TEXT - value).ToString();
            }
            else
            {
                CircleProgressBarText = " ";
                CircleProgressBarProgressLineColor = Color.Blue;
            }

            if (value == Constants.Limits.MAX_LENGTH_TEXT + 50)
            {
                CircleProgressBarText = ":D";
                CircleProgressBarFontScale = 1;
                CircleProgressBarFontAttributes = SKTypefaceStyle.Bold;
            }

            CircleProgressBarValue = value;
        }

        private async void CloseDialogCallback(IDialogParameters dialogResult)
        {
            await Rg.Plugins.Popup.Services.PopupNavigation.Instance.PopAsync();
        }

        #endregion
    }
}