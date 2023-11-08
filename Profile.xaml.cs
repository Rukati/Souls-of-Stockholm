using Rg.Plugins.Popup.Services;
using System;
using System.Threading;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace kursovaya
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Profile : ContentPage
    {
        LoadingPopup popup = new LoadingPopup();
        public Profile(ref ApiRequest.Request.OtherProfile client)
        {
            InitializeComponent();
            Box.WidthRequest = ClientInfo.screenWidth / ClientInfo.density / 2;
            Box.HeightRequest = ClientInfo.screenHeight / ClientInfo.density / 4;

            Box.Margin = new Thickness(ClientInfo.screenWidth / ClientInfo.density / 4 - 5, 60, 0, 0);

            switch (client.gender)
            {
                case "female":
                    Gender.Source = "girl.png";
                    break;
                case "male":
                    Gender.Source = "boy.png";
                    break;
                case "other":
                    Gender.Source = "other.png";
                    break;
                default:
                    break;
            }
        }

        protected override void OnAppearing()
        {
        }
    }
}
