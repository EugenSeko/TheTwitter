using InterTwitter.Views.Flyout;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Text;

namespace InterTwitter.ViewModels.Flyout
{
    public class MenuItemViewModel : BindableBase
    {
        public MenuItemViewModel()
        {
            TargetType = typeof(FlyoutViewFlyoutMenuItem);
        }

        public int Id { get; set; }
        public string Title { get; set; }

        public Type TargetType { get; set; }
    }
}
