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
    public partial class FlyoutViewFlyout : ContentPage
    {
        public ListView ListView;

        public FlyoutViewFlyout()
        {
            InitializeComponent();

            BindingContext = new FlyoutViewFlyoutViewModel();
            ListView = MenuItemsListView;
        }

        private class FlyoutViewFlyoutViewModel : INotifyPropertyChanged
        {
            public ObservableCollection<FlyoutViewFlyoutMenuItem> MenuItems { get; set; }

            public FlyoutViewFlyoutViewModel()
            {
                MenuItems = new ObservableCollection<FlyoutViewFlyoutMenuItem>(new[]
                {
                    new FlyoutViewFlyoutMenuItem { Id = 0, Title = "Page 1" },
                    new FlyoutViewFlyoutMenuItem { Id = 1, Title = "Page 2" },
                    new FlyoutViewFlyoutMenuItem { Id = 2, Title = "Page 3" },
                    new FlyoutViewFlyoutMenuItem { Id = 3, Title = "Page 4" },
                    new FlyoutViewFlyoutMenuItem { Id = 4, Title = "Page 5" },
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