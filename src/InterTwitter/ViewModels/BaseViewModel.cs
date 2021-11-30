using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Text;

namespace InterTwitter.ViewModels
{
    public class BaseViewModel : BindableBase
    {
        public BaseViewModel(INavigationService navigationService)
        {
            NavigationService = navigationService;
        }

        #region -- Public properies --

        protected INavigationService NavigationService { get; private set; }

        #endregion
    }
}
