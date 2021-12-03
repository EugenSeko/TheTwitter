﻿using InterTwitter.Helpers;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;

namespace InterTwitter.Views
{
    public class BaseContentPage : ContentPage
    {
        public BaseContentPage()
        {
            On<iOS>().SetUseSafeArea(true);
        }
        #region ---Overrides---
        protected override void OnAppearing()
        {
            base.OnAppearing();
            if (BindingContext is IPageActionsHandler actionsHandler)
            {
                actionsHandler.OnAppearing();
            }
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            if (BindingContext is IPageActionsHandler actionsHandler)
            {
                actionsHandler.OnDisappearing();
            }
        }
        #endregion
    }
}
