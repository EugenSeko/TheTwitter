﻿using InterTwitter.Helpers;
using Prism.Navigation;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace InterTwitter.ViewModels
{
    public class HomePageViewModel : BaseTabViewModel
    {
        public HomePageViewModel(INavigationService navigationService)
            : base(navigationService)
        {
            IconPath = Prism.PrismApplicationBase.Current.Resources["ic_home_gray"] as ImageSource;
        }

        #region --- Public Properties ---

        public ICommand OpenFlyoutCommandAsync => SingleExecutionCommand.FromFunc(OnOpenFlyoutCommandAsync);

        #endregion

        #region --- Overrides ---

        public override void OnAppearing()
        {
            IconPath = Prism.PrismApplicationBase.Current.Resources["ic_home_blue"] as ImageSource;
        }

        public override void OnDisappearing()
        {
            IconPath = Prism.PrismApplicationBase.Current.Resources["ic_home_gray"] as ImageSource;
        }

        #endregion

        #region --- Private Helpers ---

        private Task OnOpenFlyoutCommandAsync()
        {
            MessagingCenter.Send(this, Constants.Messages.OPEN_SIDEBAR, true);
            return Task.CompletedTask;
        }

        #endregion

    }
}
