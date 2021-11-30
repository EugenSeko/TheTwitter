using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Text;

namespace InterTwitter.ViewModels.Flyout
{
    public class FlyoutPageViewModel : BaseViewModel
    {
        public FlyoutPageViewModel(INavigationService navigationService)
                                               : base(navigationService)
        {
        }
    }
}
