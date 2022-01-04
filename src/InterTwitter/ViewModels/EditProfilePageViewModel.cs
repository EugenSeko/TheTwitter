using InterTwitter.Helpers;
using InterTwitter.Models;
using InterTwitter.Resources.Strings;
using InterTwitter.Services;
using InterTwitter.Views;
using Prism.Navigation;
using Prism.Services.Dialogs;
using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace InterTwitter.ViewModels
{
    public class EditProfilePageViewModel : BaseViewModel
    {
        private readonly IPermissionService _permissionsService;
        private readonly ISettingsManager _settingsManager;
        private readonly IUserService _userService;
        private UserModel _user;

        public EditProfilePageViewModel(
            INavigationService navigationService,
            IPermissionService permissionsService,
            ISettingsManager settingsManager,
            IUserService userService)
            : base(navigationService)
        {
            _permissionsService = permissionsService;
            _settingsManager = settingsManager;
            _userService = userService;
        }

        #region --- Public Properties ---

        private string _userMail;
        public string UserMail
        {
            get => _userMail;
            set => SetProperty(ref _userMail, value);
        }

        private string _userName;
        public string UserName
        {
            get => _userName;
            set => SetProperty(ref _userName, value);
        }

        private string _oldPassword;
        public string OldPassword
        {
            get => _oldPassword;
            set => SetProperty(ref _oldPassword, value);
        }

        private string _newPassword;
        public string NewPassword
        {
            get => _newPassword;
            set => SetProperty(ref _newPassword, value);
        }

        private string _userBackgroundImage;
        public string UserBackgroundImage
        {
            get => _userBackgroundImage;
            set => SetProperty(ref _userBackgroundImage, value);
        }

        private string _userImagePath;
        public string UserImagePath
        {
            get => _userImagePath;
            set => SetProperty(ref _userImagePath, value);
        }

        private ICommand _navigationCommandAsync;
        public ICommand NavigationCommandAsync => _navigationCommandAsync ??= SingleExecutionCommand.FromFunc(OnNavigationCommandAsync);

        private ICommand _checkCommandAsync;
        public ICommand CheckCommandAsync => _checkCommandAsync ??= SingleExecutionCommand.FromFunc(() => OnCheckCommandAsync());

        private ICommand _pickBackgroundImageAsync;
        public ICommand PickBackgroundImageAsync => _pickBackgroundImageAsync ??= SingleExecutionCommand.FromFunc(OnPickBackgroundImageAsync);

        private ICommand _pickUserImageAsync;
        public ICommand PickUserImageAsync => _pickUserImageAsync ??= SingleExecutionCommand.FromFunc(OnPickUserImageAsync);

        #endregion

        #region -- Overrides --

        public override Task InitializeAsync(INavigationParameters parameters)
        {
            var userResponse = _userService.GetUserAsync(_settingsManager.UserId).Result;
            if (userResponse.IsSuccess)
            {
                _user = userResponse.Result;
                UserBackgroundImage = _user.BackgroundUserImagePath;
                UserImagePath = _user.AvatarPath;
                UserMail = _user.Email;
                UserName = _user.Name;
                // OldPassword = user.Password;
            }

            return Task.CompletedTask;
        }

        #endregion

        #region --- Private Helpers ---

        private async Task OnPickBackgroundImageAsync()
        {
            try
            {
                var status = await _permissionsService.RequestAsync<Permissions.StorageRead>();

                if (status == PermissionStatus.Granted)
                {
                    var file = await MediaPicker.PickPhotoAsync();

                    if (file == null)
                    {
                        return;
                    }

                    UserBackgroundImage = file.FullPath;
                }
            }
            catch (Exception)
            {
            }
        }

        private async Task OnPickUserImageAsync()
        {
            try
            {
                var status = await _permissionsService.RequestAsync<Permissions.StorageRead>();

                if (status == PermissionStatus.Granted)
                {
                    var file = await MediaPicker.PickPhotoAsync();

                    if (file == null)
                    {
                        return;
                    }

                    UserImagePath = file.FullPath;
                }
            }
            catch (Exception)
            {
            }
        }

        private bool IsProfileChanged => _user.Name != UserName || _user.Email != UserMail ||
            _user.AvatarPath != UserImagePath || _user.BackgroundUserImagePath != UserBackgroundImage ||
            !string.IsNullOrEmpty(OldPassword) || !string.IsNullOrEmpty(NewPassword);

        private async Task OnCheckCommandAsync()
        {
            if (!IsProfileChanged)
            {
                return;
            }

            var param = new DialogParameters();
            param.Add(Constants.DialogParameterKeys.TITLE, $"{Strings.SaveChanges}?");
            param.Add(Constants.DialogParameterKeys.OK_BUTTON_TEXT, Strings.Ok);
            param.Add(Constants.DialogParameterKeys.CANCEL_BUTTON_TEXT, Strings.Cancel);

            await Rg.Plugins.Popup.Services.PopupNavigation.Instance.PushAsync(new AlertView(param, CloseDialogCallback));
        }

        private async void CloseDialogCallback(IDialogParameters dialogResult)
        {
            bool isAllValid = true;
            var param = new DialogParameters();
            param.Add(Constants.DialogParameterKeys.OK_BUTTON_TEXT, Strings.Ok);

            bool result = (bool)dialogResult?[Constants.DialogParameterKeys.ACCEPT];
            if (result)
            {
                if (!string.IsNullOrEmpty(OldPassword) || !string.IsNullOrEmpty(NewPassword))
                {
                    if (string.IsNullOrEmpty(OldPassword))
                    {
                        param.Add(Constants.DialogParameterKeys.TITLE, Strings.OldPassEmpty);
                        isAllValid = false;
                    }
                    else if (OldPassword != _user.Password)
                    {
                        param.Add(Constants.DialogParameterKeys.TITLE, Strings.OldPassWrong);
                        isAllValid = false;
                    }

                    if (!string.IsNullOrEmpty(NewPassword) && Regex.IsMatch(NewPassword, Constants.RegexPatterns.PASSWORD_REGEX))
                    {
                        _user.Password = NewPassword;
                    }
                    else
                    {
                        param.Add(Constants.DialogParameterKeys.TITLE, Strings.NewPasswordIsNotValid);
                        isAllValid = false;
                    }
                }

                if (!string.IsNullOrEmpty(UserName) && Regex.IsMatch(UserName, Constants.RegexPatterns.USERNAME_REGEX))
                {
                    _user.Name = UserName;
                }
                else
                {
                    param.Add(Constants.DialogParameterKeys.TITLE, Strings.NameIsNotValid);
                    isAllValid = false;
                }

                if (!string.IsNullOrEmpty(UserMail) && Regex.IsMatch(UserMail, Constants.RegexPatterns.EMAIL_REGEX))
                {
                    _user.Email = UserMail;
                }
                else
                {
                    param.Add(Constants.DialogParameterKeys.TITLE, Strings.EmailIsNotValid);
                    isAllValid = false;
                }

                if (isAllValid)
                {
                    await Rg.Plugins.Popup.Services.PopupNavigation.Instance.PopAsync();
                    _user.AvatarPath = UserImagePath;
                    _user.BackgroundUserImagePath = UserBackgroundImage;

                    await _userService.UpdateUserAsync(_user);

                    MessagingCenter.Send(this, Constants.Messages.USER_PROFILE_CHANGED);
                    await NavigationService.GoBackAsync();
                }
                else if (param.ContainsKey(Constants.DialogParameterKeys.TITLE) && param[Constants.DialogParameterKeys.TITLE] != null)
                {
                    await Rg.Plugins.Popup.Services.PopupNavigation.Instance.PopAsync();
                    await Rg.Plugins.Popup.Services.PopupNavigation.Instance.PushAsync(new AlertView(param, Callback));
                    return;
                }
            }
            else if (!_isIndirectCall)
            {
                await Rg.Plugins.Popup.Services.PopupNavigation.Instance.PopAsync();
            }
            else
            {
                await Rg.Plugins.Popup.Services.PopupNavigation.Instance.PopAsync();
                await NavigationService.GoBackAsync();
            }
        }

        private async void Callback(IDialogParameters dialogResult)
        {
            await Rg.Plugins.Popup.Services.PopupNavigation.Instance.PopAsync();
        }

        private bool _isIndirectCall;
        private async Task OnNavigationCommandAsync()
        {
            if (IsProfileChanged)
            {
                _isIndirectCall = true;
                await OnCheckCommandAsync();
            }
            else
            {
                await NavigationService.GoBackAsync();
            }
        }

        #endregion

    }
}
