using InterTwitter.Helpers;
using InterTwitter.Extensions;
using InterTwitter.Services;
using Prism.Navigation;
using Prism.Services.Dialogs;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using InterTwitter.Views;
using InterTwitter.Resources.Strings;

namespace InterTwitter.ViewModels
{
    public class BlacklistPageViewModel : BaseViewModel
    {
        private readonly IUserService _userService;

        private bool _isBlacklistPage;
        private bool _isMutelistPage;

        public BlacklistPageViewModel(
            INavigationService navigationService,
            IUserService userService)
            : base(navigationService)
        {
            _userService = userService;
        }

        #region -- Public Properties --

        private ObservableCollection<UserViewModel> _usersList;
        public ObservableCollection<UserViewModel> UsersList
        {
            get => _usersList;
            set => SetProperty(ref _usersList, value);
        }

        private string _title;
        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }

        public ICommand NavigateBackCommandAsync => SingleExecutionCommand.FromFunc(() => NavigationService.GoBackAsync());

        #endregion

        #region -- Overrides --

        public override async Task InitializeAsync(INavigationParameters parameters)
        {
            if (_isBlacklistPage = parameters.ContainsKey(Constants.Navigation.BLACKLIST))
            {
                Title = Strings.Blacklist;
                await InitBlacklistAsync();
            }
            else if (_isMutelistPage = parameters.ContainsKey(Constants.Navigation.MUTELIST))
            {
                Title = Resources.Strings.Strings.Mute;
                await InitMutelistAsync();
            }
        }

        #endregion

        #region -- Private Helpers --

        private async Task InitBlacklistAsync()
        {
            UsersList = new ObservableCollection<UserViewModel>();
            var blockedUsersResponse = await _userService.GetAllBlockedUsersAsync();
            if (blockedUsersResponse.IsSuccess)
            {
                foreach (var userModel in blockedUsersResponse.Result)
                {
                    var userViewModel = userModel.ToUserViewModel();
                    userViewModel.RemoveCommand = SingleExecutionCommand.FromFunc(OnRemoveCommand);
                    UsersList.Add(userViewModel);
                }
            }
        }

        private async Task InitMutelistAsync()
        {
            UsersList = new ObservableCollection<UserViewModel>();
            var mutedUsersResponse = await _userService.GetAllMutedUsersAsync();
            if (mutedUsersResponse.IsSuccess)
            {
                foreach (var userModel in mutedUsersResponse.Result)
                {
                    var userViewModel = userModel.ToUserViewModel();
                    userViewModel.RemoveCommand = SingleExecutionCommand.FromFunc(OnRemoveCommand);
                    UsersList.Add(userViewModel);
                }
            }
        }

        private async Task OnRemoveCommand(object parameter)
        {
            if (parameter is UserViewModel userViewModel)
            {
                _removingUser = parameter as UserViewModel;

                string partOfMessage = string.Empty;
                if (_isBlacklistPage)
                {
                    partOfMessage = Strings.FromTheBlacklist;
                }
                else if (_isMutelistPage)
                {
                    partOfMessage = Strings.FromTheMute;
                }

                var param = new DialogParameters();
                param.Add(Constants.DialogParameterKeys.TITLE, $"{Strings.Remove} {userViewModel.Name} {partOfMessage}");
                param.Add(Constants.DialogParameterKeys.OK_BUTTON_TEXT, Strings.Ok);
                param.Add(Constants.DialogParameterKeys.CANCEL_BUTTON_TEXT, Strings.Cancel);

                await Rg.Plugins.Popup.Services.PopupNavigation.Instance.PushAsync(new AlertView(param, CloseDialogCallback));
            }
        }

        private UserViewModel _removingUser;
        private async void CloseDialogCallback(IDialogParameters dialogResult)
        {
            bool result = (bool)dialogResult?[Constants.DialogParameterKeys.ACCEPT];
            if (result)
            {
                if (_isBlacklistPage)
                {
                    await _userService.RemoveFromBlacklistAsync(_removingUser.Id);
                    UsersList.Remove(_removingUser);
                }
                else if (_isMutelistPage)
                {
                    await _userService.RemoveFromMutelistAsync(_removingUser.Id);
                    UsersList.Remove(_removingUser);
                }
            }

            await Rg.Plugins.Popup.Services.PopupNavigation.Instance.PopAsync();
        }

        #endregion

    }
}
