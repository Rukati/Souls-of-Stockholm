using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace kursovaya
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    internal class LoadingPopup : PopupPage
    {
        public LoadingPopup()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {

            var activityIndicator = new ActivityIndicator
            {
                IsRunning = true,
                IsVisible = true,
                Color = Color.White
            };

            var stackLayout = new StackLayout
            {
                VerticalOptions = LayoutOptions.CenterAndExpand,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                Children = { activityIndicator }
            };

            Content = stackLayout;

            CloseWhenBackgroundIsClicked = false;

        }
    }
}
