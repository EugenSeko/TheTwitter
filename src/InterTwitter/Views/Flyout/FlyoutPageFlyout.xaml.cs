using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace InterTwitter.Views.Flyout
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FlyoutPageFlyout : ContentPage
    {
        public ListView ListView;

        public FlyoutPageFlyout()
        {
            InitializeComponent();

            BindingContext = new FlyoutPageFlyoutViewModel();
            ListView = MenuItemsListView;
        }

        private class FlyoutPageFlyoutViewModel : INotifyPropertyChanged
        {
            public ObservableCollection<FlyoutPageFlyoutMenuItem> MenuItems { get; set; }

            public FlyoutPageFlyoutViewModel()
            {
                MenuItems = new ObservableCollection<FlyoutPageFlyoutMenuItem>(new[]
                {
                    new FlyoutPageFlyoutMenuItem { Id = 0, Title = "Page 1" },
                    new FlyoutPageFlyoutMenuItem { Id = 1, Title = "Page 2" },
                    new FlyoutPageFlyoutMenuItem { Id = 2, Title = "Page 3" },
                    new FlyoutPageFlyoutMenuItem { Id = 3, Title = "Page 4" },
                    new FlyoutPageFlyoutMenuItem { Id = 4, Title = "Page 5" },
                });
            }

            #region INotifyPropertyChanged Implementation
            public event PropertyChangedEventHandler PropertyChanged;
            private void OnPropertyChanged([CallerMemberName] string propertyName = "")
            {
                if (PropertyChanged == null)
                {
                    return;
                }

                PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
            #endregion
        }
    }
}