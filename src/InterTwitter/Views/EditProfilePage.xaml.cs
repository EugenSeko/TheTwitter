using InterTwitter.ViewModels;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace InterTwitter.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EditProfilePage : BaseContentPage
    {
        public EditProfilePage()
        {
            InitializeComponent();
            backgroundImage.WidthRequest = Prism.PrismApplicationBase.Current.MainPage.Width;
            backgroundImage.HeightRequest = Prism.PrismApplicationBase.Current.MainPage.Height * 0.19;
        }

        protected override bool OnBackButtonPressed()
        {
            (BindingContext as EditProfilePageViewModel)?.NavigationCommandAsync.Execute(null);
            return true;
        }

        public async void OnLeftArrowTapHandler(object sender, EventArgs e)
        {
            leftArrowImage.Source = (ImageSource)Prism.PrismApplicationBase.Current.Resources["ic_left_gray"];
            await Task.Delay(500);
            leftArrowImage.Source = (ImageSource)Prism.PrismApplicationBase.Current.Resources["ic_left_blue"];
        }

        public async void OnCheckTapHandler(object sender, EventArgs e)
        {
            checkImage.Source = (ImageSource)Prism.PrismApplicationBase.Current.Resources["ic_check_gray"];
            await Task.Delay(500);
            checkImage.Source = (ImageSource)Prism.PrismApplicationBase.Current.Resources["ic_check_blue"];
        }
    }
}