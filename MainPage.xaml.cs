using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using Rg.Plugins.Popup.Services;
using System.Net.Http;
using System.Linq;
using static ApiRequest.Request;
using System.Collections.Generic;

namespace kursovaya
{
    public partial class MainPage : ContentPage
    {
        private async Task ChangeTextColorAndRestore(Entry entry, Color newColor, TimeSpan duration)
        {
            Color originalColor = entry.PlaceholderColor; // Сохраняем оригинальный цвет текста
            // Создаем анимацию изменения цвета текста
            var animation = new Animation(callback: (double value) =>
            {
                entry.PlaceholderColor = Color.FromRgba(
                    newColor.R + (originalColor.R - newColor.R) * value,
                    newColor.G + (originalColor.G - newColor.G) * value,
                    newColor.B + (originalColor.B - newColor.B) * value,
                    newColor.A + (originalColor.A - newColor.A) * value);
            });

            // Запускаем анимацию на основном потоке
            Device.BeginInvokeOnMainThread(() =>
            {
                animation.Commit(
                    owner: entry,
                    name: "TextColorAnimation",
                    length: (uint)duration.TotalMilliseconds,
                    finished: (animation2, finished2) =>
                    {
                        // Восстанавливаем оригинальный цвет текста после завершения анимации
                        entry.PlaceholderColor = originalColor;
                    });
            });

            // Ждем завершения анимации
            await Task.Delay((int)duration.TotalMilliseconds);
        }
        public MainPage()
        {
            InitializeComponent();
            ClientInfo player = new ClientInfo();
            double left = (ClientInfo.screenWidth / ClientInfo.density) / 6;
            double top = (ClientInfo.screenHeight / ClientInfo.density);

            UsernameFrame.Margin = new Thickness(left, top - 320, 0, 0);
            PasswordFrame.Margin = new Thickness(left, top - 240, 0, 0);
            Sign.Margin = new Thickness(left, top - 150, 0, 0);
            
            Label label = new Label()
            {
                Text = "Пропустить",
                FontSize = 17,
                Margin = new Thickness(left * 3 - 40,Sign.Margin.Top + 70,0,0),
                TextColor = Color.DarkGray,
            };

            var tapGesture = new TapGestureRecognizer();
            tapGesture.Tapped += SkipTapped; 

            label.GestureRecognizers.Add(tapGesture);
            StartAbsolute.Children.Add(label);
        }

        LoadingPopup popup = new LoadingPopup();
        private async void SkipTapped(object sender, EventArgs e)
        {
            await PopupNavigation.Instance.PushAsync(popup);

            ClientInfo.Profile = new Client
            {
                id = -1,
            };
            var newMainPage = new FlyoutPage1();
            Application.Current.MainPage = new NavigationPage(newMainPage)
            {
                BarBackgroundColor = Color.FromHex("#303030")
            };

            await PopupNavigation.Instance.PopAsync();

        }

        private async void ButtonClick(object sender, EventArgs e)
        {

            if (string.IsNullOrWhiteSpace(Username.Text) && string.IsNullOrWhiteSpace(Password.Text))
            {
                // Создайте задачи для каждого асинхронного метода
                var changeUsernameColorTask = ChangeTextColorAndRestore(Username, Color.Red, TimeSpan.FromSeconds(1));
                var changePasswordColorTask = ChangeTextColorAndRestore(Password, Color.Red, TimeSpan.FromSeconds(1));

                // Дождитесь завершения обоих задач
                await Task.WhenAll(changeUsernameColorTask, changePasswordColorTask);

            }
            else if (string.IsNullOrWhiteSpace(Username.Text))
                await ChangeTextColorAndRestore(Username, Color.Red, TimeSpan.FromSeconds(1));
            else if (string.IsNullOrWhiteSpace(Password.Text))
                await ChangeTextColorAndRestore(Password, Color.Red, TimeSpan.FromSeconds(1));
            else
            {

                await PopupNavigation.Instance.PushAsync(popup);

                int AuthenticationCode = await ApiRequest.Request.PostAuthentication(Username.Text, Password.Text);
                if (AuthenticationCode == 200)
                {
                    List<Client> profiles = await GetInfoProfile();
                    ClientInfo.Profile = profiles[0];
                    var newMainPage = new FlyoutPage1();

                    Application.Current.MainPage = new NavigationPage(newMainPage)
                    {
                        BarBackgroundColor = Color.FromHex("#303030")
                    };

                }
                else
                {
                    await DisplayAlert("Ошибка", "Проверьте правильность данных", "Закрыть");
                }
                await PopupNavigation.Instance.PopAsync();
            }
        }
    }
}
