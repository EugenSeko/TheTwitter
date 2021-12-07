﻿using DLToolkit.Forms.Controls;
using InterTwitter.Services;
using InterTwitter.ViewModels;
using InterTwitter.Views;
using Prism.Ioc;
using Prism.Unity;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace InterTwitter
{
    public partial class App : PrismApplication
    {
        public static T Resolve<T>() => Current.Container.Resolve<T>();

        public App()
        {
        }

        #region -- Overrides --

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            //Services
            containerRegistry.RegisterInstance<IMockService>(Container.Resolve<MockService>());
            containerRegistry.RegisterInstance<ITweetService>(Container.Resolve<TweetService>());
            // Navigation
            containerRegistry.RegisterForNavigation<NavigationPage>();
            containerRegistry.RegisterForNavigation<MainPage>();
            containerRegistry.RegisterForNavigation<HomePage, HomePageViewModel>();
        }

        protected override async void OnInitialized()
        {
            InitializeComponent();
            FlowListView.Init();
            await NavigationService.NavigateAsync($"/{nameof(NavigationPage)}/{nameof(HomePage)}");
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }

        #endregion

    }
}
