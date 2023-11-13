using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using static ApiRequest.Request;

namespace kursovaya
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Profile : ContentPage
    {
        int page = 1;
        readonly int MaxPost = 5;
        int id;
        OtherProfile client;
        public Profile(in ApiRequest.Request.OtherProfile client)
        {
            InitializeComponent();

            this.client = client;
            id = client.id;
            
            Box.WidthRequest = ClientInfo.screenWidth / ClientInfo.density / 2;
            Box.HeightRequest = ClientInfo.screenHeight / ClientInfo.density / 4.5;

            Box.Margin = new Thickness(ClientInfo.screenWidth / ClientInfo.density / 4, 10, 0, 0);
            Gender.Margin = new Thickness(0, -ClientInfo.screenWidth / ClientInfo.density / 4.2 + 5, 0, 0);
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

            DisplayPosts();
        }
        private void ViewPost(object sender, EventArgs e)
        {
            BoxView clickedButton = (BoxView)sender;
            int classId = int.Parse(clickedButton.ClassId);

            Navigation.PushAsync(new OnePost(classId));
        }

        private void DisplayPosts()
        {
            var selectPost = from post in PageWithAllPosts.AnswerRequestPosts
                             where post.user.id == id
                             select post;
            
            int row = 5;

            int maxTextLengthName = 35;

            double newYPostName = 5;

            int NumPost = 1;

            double boxW = ClientInfo.screenWidth / 4;

            int index = 0;

            Grid _GridProfile = GridProfile;

            if (_GridProfile.Children.Count > 0) { _GridProfile.Children.Clear(); }
            if (_GridProfile.RowDefinitions.Count > 0) { _GridProfile.RowDefinitions.Clear(); }
            if (_GridProfile.ColumnDefinitions.Count > 0) { _GridProfile.ColumnDefinitions.Clear(); }

            _GridProfile.Margin = new Thickness(0, ClientInfo.screenHeight / ClientInfo.density / 4, 0, 0);

            _GridProfile.ColumnDefinitions.Add(new ColumnDefinition { Width = ClientInfo.screenWidth / 6 / ClientInfo.density }); // Левая пустая колонка
            _GridProfile.ColumnDefinitions.Add(new ColumnDefinition { Width = boxW }); // Центральная колонка
            _GridProfile.ColumnDefinitions.Add(new ColumnDefinition { Width = ClientInfo.screenWidth / 6 / ClientInfo.density }); // Правая пустая колонка

            
            Label name = new Label()
            {
                Text = $"{client.username}",
                TextColor = Xamarin.Forms.Color.White,
                FontSize = 25,
                Margin = new Thickness(-15, -10, 0, 0),
                HorizontalOptions = LayoutOptions.Center,
            };

            Label age = new Label()
            {
                Text = $"age:\n{client.age}",
                TextColor = Xamarin.Forms.Color.White,
                FontSize = 18,
                HorizontalTextAlignment = TextAlignment.Center,
                HorizontalOptions = LayoutOptions.Start,
            };

            Label gender = new Label()
            {
                Text = $"gender:\n{client.gender}",
                TextColor = Xamarin.Forms.Color.White,
                FontSize = 18,
                Margin = new Thickness(boxW / 1.3, 0, 0, 0),
                HorizontalTextAlignment = TextAlignment.Center,
            };

            Label country = new Label()
            {
                Text = $"country:\n{client.country}",
                TextColor = Xamarin.Forms.Color.White,
                FontSize = 18,
                HorizontalTextAlignment = TextAlignment.Center,
                HorizontalOptions = LayoutOptions.Center,
            };


            Label Dev = new Label()
            {
                TextColor = Xamarin.Forms.Color.Red,
                FontSize = 20,
                Margin = new Thickness(-10, -10, 0, 0),
                HorizontalOptions = LayoutOptions.Center,
            };

            Label Mode = new Label()
            {
                TextColor = Xamarin.Forms.Color.Red,
                FontSize = 20,
                Margin = new Thickness(-10, -15, 0, 0),
                HorizontalOptions = LayoutOptions.Center,
            };


            _GridProfile.RowDefinitions.Add(new RowDefinition { Height = ClientInfo.screenHeight / 6 / ClientInfo.density / 7 });
            _GridProfile.RowDefinitions.Add(new RowDefinition { Height = ClientInfo.screenHeight / 6 / ClientInfo.density / 7 });
            _GridProfile.RowDefinitions.Add(new RowDefinition { Height = ClientInfo.screenHeight / 6 / ClientInfo.density / 7 });
            _GridProfile.RowDefinitions.Add(new RowDefinition { Height = ClientInfo.screenHeight / 6 / ClientInfo.density / 3.5 });
            _GridProfile.RowDefinitions.Add(new RowDefinition { Height = ClientInfo.screenHeight / 6 / ClientInfo.density / 3.5 });


            Dev.Text = client.is_superuser ? "Developer" : "";
            Mode.Text = client.is_staff ? "Moderator - CCG" : "";

            _GridProfile.Children.Add(name,1,0);
            _GridProfile.Children.Add(Dev,1,1);
            _GridProfile.Children.Add(Mode, 1, 2);
            _GridProfile.Children.Add(age, 1, 3);
            _GridProfile.Children.Add(gender, 1, 3);
            _GridProfile.Children.Add(country, 1, 3);

            
            Label _post = new Label()
            {
                Text = "Посты: ",
                TextColor = Color.WhiteSmoke,
                FontSize = 20,
                HorizontalOptions = LayoutOptions.Center,
                Margin = new Thickness(0,15,0,0),
            };
            _GridProfile.Children.Add(_post, 1, 4);

            foreach (var answer in selectPost)
            {
                if (index != (page - 1) * 5)
                {
                    index++;
                    continue;
                }
                string NamePost = answer.name;
                if (NamePost.Length > maxTextLengthName)
                {
                    NamePost = NamePost.Substring(0, maxTextLengthName - 3) + "...";
                }

                Label PostName = new Label
                {
                    Text = NamePost,
                    TextColor = Xamarin.Forms.Color.FromHex("#626362"),
                    HorizontalOptions = LayoutOptions.Start,
                    TranslationY = newYPostName,
                    FontSize = 20,
                    InputTransparent = true
                };

                BoxView boxView = new BoxView
                {
                    Color = Xamarin.Forms.Color.FromHex("#1C1C1C"),
                    WidthRequest = boxW,
                    ClassId = answer.id.ToString(),
                    CornerRadius = 10,
                    Margin = new Thickness(-10, 0, 0, 0),
                };

                var tapGestureRecognizer = new TapGestureRecognizer();
                tapGestureRecognizer.Tapped += (s, e) =>
                {
                    ViewPost(s, e);
                };

                boxView.GestureRecognizers.Add(tapGestureRecognizer);

                _GridProfile.RowDefinitions.Add(new RowDefinition { Height = ClientInfo.screenHeight / 6 / ClientInfo.density / 2.2});

                _GridProfile.Children.Add(boxView, 1, row);
                _GridProfile.Children.Add(PostName, 1, row);

                row++;
                if (NumPost == MaxPost)
                    break;
                else
                    NumPost++;
            }
                      
            //-------------------------------------------------------------

            Label back = new Label
            {
                Text = "Назад",
                TextColor = Xamarin.Forms.Color.FromHex("#626362"),
                HorizontalOptions = LayoutOptions.Start,
                FontSize = 20,
            };

            back.GestureRecognizers.Add(new TapGestureRecognizer()
            {
                Command = new Command(() =>
                {
                    BackPage();
                })
            });

            //-------------------------------------------------------------

            Label next = new Label
            {
                Text = "Далее",
                TextColor = Xamarin.Forms.Color.FromHex("#626362"),
                HorizontalOptions = LayoutOptions.Start,
                FontSize = 20,
                Margin = new Thickness(ClientInfo.screenWidth / 2 / ClientInfo.density, 0, 0, 0)
            };

            next.GestureRecognizers.Add(new TapGestureRecognizer()
            {
                Command = new Command(() =>
                {
                    NextPage();
                })
            });

            //-------------------------------------------------------------

            Label nowPage = new Label
            {
                Text = page.ToString(),
                TextColor = Xamarin.Forms.Color.FromHex("#626362"),
                HorizontalOptions = LayoutOptions.Start,
                FontSize = 20,
                Margin = new Thickness(ClientInfo.screenWidth / 3 / ClientInfo.density,0,0,0)
            };

            //-------------------------------------------------------------
            

            int lastPage = (int)Math.Ceiling((double)selectPost.Count() / 5);
            _GridProfile.Children.Add(nowPage, 1, row);

            if (page == lastPage)
            {
                if (lastPage > 1)
                {
                    _GridProfile.Children.Add(back, 1, row);
                }
            }
            else if (page != 1)
            {
                _GridProfile.Children.Add(back,1,row);
                _GridProfile.Children.Add(next,1,row);
            }
            else
            {
                _GridProfile.Children.Add(next,1,row);
            }
        }

        private void BackPage()
        {
            page--;
            DisplayPosts();
        }

        private void NextPage()
        {
            page++;
            DisplayPosts();
        }

        protected override void OnAppearing()
        {
        }
    }
}
