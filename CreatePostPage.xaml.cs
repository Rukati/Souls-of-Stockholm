using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace kursovaya
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CreatePostPage : ContentPage
    {
        public CreatePostPage()
        {
            InitializeComponent();
        }

        private async void OnInputComplete(object sender, EventArgs e)
        {
            string variable1 = Variable1Entry.Text;
            string variable2 = Variable2Entry.Text;

            int code = await ApiRequest.Request.PostCreatePost(variable1, variable2, ClientInfo.Profile.id);
            await Navigation.PopAsync();
            switch (code) {
                case 201:
                    await DisplayAlert("Успех", "Пост создан!", "ОК");
                    break;
                case 401:
                    await DisplayAlert("Провал","Пожалуйста, авторизуйтесь!","ОК");
                    break;
                default:
                    await DisplayAlert("Провал", "Что-то пошло не так...", "ОК");
                    break;
            }
            
        }

    }
}