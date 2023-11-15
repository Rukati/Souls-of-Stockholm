using ApiRequest;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace kursovaya
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FlyoutPage1 : FlyoutPage
    {
        public FlyoutPage1()
        {
            InitializeComponent();

            NavigationPage navigationPage = new NavigationPage(new PageWithAllPosts()); 
            navigationPage.BarBackgroundColor = Xamarin.Forms.Color.FromHex("#303030");
            Detail = navigationPage;

            FlyoutPage.ListView.ItemSelected += ListView_ItemSelected;
        }

        private async void ListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            FlyoutPage.ListView.SelectedItem = null;
            IsPresented = false;

            if (e.SelectedItem is FlyoutPage1FlyoutMenuItem menuItem)
            {
                if (menuItem.Id == 1)
                {
                    await Navigation.PushAsync(new CreatePostPage(), true);
                }
                else if (menuItem.Id == 0)
                {
                    LoadingPopup popup = new LoadingPopup();
                    await PopupNavigation.Instance.PushAsync(popup);

                    if (menuItem.Title == "Профиль")
                    {
                        if (ClientInfo.Profile.id != -1)
                        {
                            ApiRequest.Request.OtherProfile profile = await Request.GetInfoOtherProfile(ClientInfo.Profile.id);
                            await Navigation.PushAsync(new Profile(in profile), true);
                        }
                        else
                            await DisplayAlert("Ошибка", "Пожалуйста, авторизуйтесь!", "OK");
                    }
                    await PopupNavigation.Instance.PopAsync();
                }
                else if (menuItem.Id == 2)
                {
                    if (menuItem.Title == "Войти")
                    {
                        await Navigation.PushModalAsync(new MainPage());
                    }
                    else
                    {
                        ClientInfo.Profile = new Request.Client()
                        {
                            id = -1,
                        };
                        await SecureStorage.SetAsync("IsLoggedIn", "false");

                        Application.Current.MainPage = new FlyoutPage1();
                    }

                }
            }
        }
    }
}
