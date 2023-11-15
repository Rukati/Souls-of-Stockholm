using Xamarin.Forms;
using Xamarin.Essentials;
using static ApiRequest.Request;
using System.Collections.Generic;

namespace kursovaya
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
        }

        protected override async void OnStart()
        {
            base.OnStart();

            ClientInfo client = new ClientInfo();

            // Определите, авторизован ли пользователь
            string isLoggedIn = await SecureStorage.GetAsync("IsLoggedIn");

            if (!string.IsNullOrEmpty(isLoggedIn) && isLoggedIn.ToLower() == "true")
            {
                List<Client> profiles = await GetInfoProfile();
                ClientInfo.Profile = profiles[0];
                // Пользователь авторизован, переходим на основную страницу
                MainPage = new NavigationPage(new FlyoutPage1())
                {
                    BarBackgroundColor = Color.FromHex("#303030")
                };
            }
            else
            {
                // Пользователь не авторизован, переходим на страницу входа
                MainPage = new MainPage();
            }
        }
    }
}
