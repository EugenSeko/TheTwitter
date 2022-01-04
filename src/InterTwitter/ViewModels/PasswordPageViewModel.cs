using InterTwitter.Helpers;
using InterTwitter.Models;
using InterTwitter.Resources.Strings;
using InterTwitter.Services;
using InterTwitter.ViewModels.Validators;
using InterTwitter.Views;
using Prism.Navigation;
using Prism.Services.Dialogs;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Input;

namespace InterTwitter.ViewModels
{
    public class PasswordPageViewModel : BaseViewModel
    {
        private readonly IRegistrationService _registrationService;

        private readonly IDialogService _dialogService;

        private readonly ISettingsManager _settingsManager;

        private readonly IKeyboardHelper _keyboardHelper;

        private UserModel _user;
        private double _maxHeight;
        private bool _isSaveFocusedPassword;
        private bool _isSaveFocusedConfirmPassword;

        public PasswordPageViewModel (
            INavigationService navigationService,
            IDialogService dialogService,
            IRegistrationService registrationService,
            ISettingsManager settingsManager,
            IKeyboardHelper keyboardHelper)
            : base(navigationService)
        {
            _dialogService = dialogService;
            _registrationService = registrationService;
            _settingsManager = settingsManager;
            _keyboardHelper = keyboardHelper;
        }

        #region -- Public properties --

        private string _password = string.Empty;

        public string Password
        {
            get => _password;
            set => SetProperty(ref _password, value);
        }

        private bool _isVisiblePassword = false;

        public bool IsVisiblePassword
        {
            get => _isVisiblePassword;
            set => SetProperty(ref _isVisiblePassword, value);
        }

        private bool _isVisibleConfirmPassword = false;

        public bool IsVisibleConfirmPassword
        {
            get => _isVisibleConfirmPassword;
            set => SetProperty(ref _isVisibleConfirmPassword, value);
        }

        private bool _isWrongPassword = false;

        public bool IsWrongPassword
        {
            get => _isWrongPassword;
            set => SetProperty(ref _isWrongPassword, value);
        }

        private bool _isWrongConfirmPassword = false;

        public bool IsWrongConfirmPassword
        {
            get => _isWrongConfirmPassword;
            set => SetProperty(ref _isWrongConfirmPassword, value);
        }

        private string _confirmPassword = string.Empty;

        public string ConfirmPassword
        {
            get => _confirmPassword;
            set => SetProperty(ref _confirmPassword, value);
        }

        private bool _isFocusedPassword = false;

        public bool IsFocusedPassword
        {
            get => _isFocusedPassword;
            set => SetProperty(ref _isFocusedPassword, value);
        }

        private bool _isFocusedConfirmPassword = false;

        public bool IsFocusedConfirmPassword
        {
            get => _isFocusedConfirmPassword;
            set => SetProperty(ref _isFocusedConfirmPassword, value);
        }

        private string _buttonText = Strings.Next;

        public string ButtonText
        {
            get => _buttonText;
            set => SetProperty(ref _buttonText, value);
        }

        private bool _isVisibleButton = false;

        public bool IsVisibleButton
        {
            get => _isVisibleButton;
            set => SetProperty(ref _isVisibleButton, value);
        }

        private double _currentHeight;

        public double CurrentHeight
        {
            get => _currentHeight;
            set => SetProperty(ref _currentHeight, value);
        }

        private bool _isEntryPasswordFocused;

        public bool IsEntryPasswordFocused
        {
            get => _isEntryPasswordFocused;
            set => SetProperty(ref _isEntryPasswordFocused, value);
        }

        private bool _isEntryConfirmPasswordFocused;

        public bool IsEntryConfirmPasswordFocused
        {
            get => _isEntryConfirmPasswordFocused;
            set => SetProperty(ref _isEntryConfirmPasswordFocused, value);
        }

        private ICommand _CreateCommand;

        public ICommand CreateCommand => _CreateCommand ??= SingleExecutionCommand.FromFunc(OnCreateCommandAsync);

        private ICommand _TwitterCommand;

        public ICommand TwitterCommand => _TwitterCommand ??= SingleExecutionCommand.FromFunc(OnTwitterCommandAsync);

        #endregion

        #region -- Overrides --

        protected override async void OnPropertyChanged(PropertyChangedEventArgs args)
        {
            base.OnPropertyChanged(args);

            if (args.PropertyName == nameof(IsFocusedPassword))
            {
                ButtonText = Strings.Next;
            }

            if (args.PropertyName == nameof(IsFocusedConfirmPassword))
            {
                ButtonText = Strings.Confirm;
            }

            if (args.PropertyName == nameof(CurrentHeight))
            {
                if (_maxHeight < CurrentHeight)
                {
                    _maxHeight = CurrentHeight;
                }

                if (_maxHeight > CurrentHeight && (IsFocusedPassword || IsFocusedConfirmPassword))
                {
                    await Task.Delay(300);
                    IsVisibleButton = true;
                }
                else
                {
                    IsVisibleButton = false;
                }
            }

            if (args.PropertyName == nameof(IsFocusedPassword))
            {
                if (IsFocusedPassword)
                {
                    _isSaveFocusedPassword = true;
                    _isSaveFocusedConfirmPassword = false;
                    _maxHeight = CurrentHeight;
                }
            }

            if (args.PropertyName == nameof(IsFocusedConfirmPassword))
            {
                if (IsFocusedConfirmPassword)
                {
                    _isSaveFocusedPassword = false;
                    _isSaveFocusedConfirmPassword = true;
                    _maxHeight = CurrentHeight;
                }
            }

            if (args.PropertyName == nameof(Password))
            {
                IsWrongPassword = false;
            }

            if (args.PropertyName == nameof(ConfirmPassword))
            {
                IsWrongConfirmPassword = false;
            }
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            if (parameters.TryGetValue(Constants.Navigation.USER, out UserModel user))
            {
                _user = user;
            }
        }

        #endregion

        #region -- Private helpers --

        private async Task OnCreateCommandAsync()
        {
            _keyboardHelper.HideKeyboard();

            var parametrs = new NavigationParameters { { Constants.Navigation.USER, _user } };
            await NavigationService.GoBackAsync(parametrs);
        }

        private async Task OnTwitterCommandAsync()
        {
            var validator = ValidatorsExtension.PasswordPageValidator.Validate(this);
            if (validator.IsValid)
            {
                _user.Password = Password;
                _user.AvatarPath = "pic_profile_big";
                _user.BackgroundUserImagePath = "pic_profile_big";

                _keyboardHelper.HideKeyboard();

                var result = await _registrationService.AddAsync(_user);
                if (result.IsSuccess)
                {
                    _settingsManager.UserId = _user.Id;
                    var parametrs = new NavigationParameters { { Constants.Navigation.USER, _user } };
                    await NavigationService.NavigateAsync($"/{nameof(FlyOutPage)}", parametrs);
                }
                else
                {
                    var parametrs = new DialogParameters { { Constants.Navigation.MESSAGE, Strings.AlertDatabase } };
                    await Rg.Plugins.Popup.Services.PopupNavigation.Instance.PushAsync(new AlertView(parametrs, CloseDialogCallback));
                }
            }
            else
            {
                foreach (var error in validator.Errors)
                {
                    if (error.PropertyName == nameof(Password))
                    {
                        IsWrongPassword = true;
                    }

                    if (error.PropertyName == nameof(ConfirmPassword) && ButtonText != Strings.Next)
                    {
                        IsWrongConfirmPassword = true;
                    }
                }

                if (!IsWrongPassword)
                {
                    ButtonText = Strings.Confirm;
                    IsEntryConfirmPasswordFocused = true;
                }
                else
                {
                    ButtonText = Strings.Next;
                    IsEntryPasswordFocused = true;
                }
            }
        }

        private async void CloseDialogCallback(IDialogParameters dialogResult)
        {
            await Rg.Plugins.Popup.Services.PopupNavigation.Instance.PopAsync();
        }

        #endregion
    }
}
