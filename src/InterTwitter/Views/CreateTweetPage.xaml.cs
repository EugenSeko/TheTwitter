using InterTwitter.ViewModels;
using Xamarin.Forms;

namespace InterTwitter.Views
{
    public partial class CreateTweetPage : BaseContentPage
    {
        public CreateTweetPage()
        {
            InitializeComponent();
        }

        #region -- Overrides --

        protected override bool OnBackButtonPressed()
        {
            if (BindingContext is CreateTweetPageViewModel viewModel)
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    viewModel.GoBackCommand.Execute(null);
                });
            }

            return true;
        }

        #endregion
    }
}