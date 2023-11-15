using ApiRequest;
using Newtonsoft.Json;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Security.Permissions;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using static ApiRequest.Request;

namespace kursovaya
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PageWithAllPosts : ContentPage
    {
        static public List<Posts> AnswerRequestPosts;
        readonly Grid layout;
        LoadingPopup popup = new LoadingPopup();
        public PageWithAllPosts()
        {
            InitializeComponent();
            layout = GridText;
        }
        protected override void OnAppearing()
        {
   
            if (layout.Children.Count > 0)
            {
                layout.RowDefinitions.Clear();
                layout.ColumnDefinitions.Clear();
                layout.Children.Clear();
            }

            ShowPosts();

        }
        private async void ViewPost(object sender, EventArgs e)
        {
            BoxView clickedButton = (BoxView)sender;
            int classId = int.Parse(clickedButton.ClassId);

            await PopupNavigation.Instance.PushAsync(popup);
            await Navigation.PushAsync(new OnePost(classId), true);
            await PopupNavigation.Instance.PopAsync();
        }
        private async void ShowPosts()
        {
            await PopupNavigation.Instance.PushAsync(popup);

            AnswerRequestPosts = await GetAllPosts();

            layout.RowDefinitions.Add(new RowDefinition { Height = 125 });
            Label SOS = new Label
            {
                Text = "SOS",
                TextColor = Xamarin.Forms.Color.FromHex("#8E8E8E"),
                FontSize = 120,
                HorizontalOptions = LayoutOptions.Center,
                Margin = new Thickness(0,-10,0,0),
            };

            layout.RowDefinitions.Add(new RowDefinition { Height = 25});
            Label Welcome = new Label
            {
                Text = "Добро пожаловать на Souls of Stockholm",
                TextColor = Xamarin.Forms.Color.FromHex("#8E8E8E"),
                FontSize = 15,
                HorizontalOptions = LayoutOptions.Center,
                FontAttributes = FontAttributes.Bold
            };

            layout.RowDefinitions.Add(new RowDefinition { Height = 20 });
            Label Hello = new Label
            {
                Text = "Здесь не салам, здесь здравствуйте!",
                TextColor = Xamarin.Forms.Color.FromHex("#8E8E8E"),
                FontSize = 10,
                HorizontalOptions = LayoutOptions.Center,
                FontAttributes = FontAttributes.Bold,
                Margin = new Thickness(0, -10, 0, 0),
            };

            layout.Children.Add(SOS,0,0);
            layout.Children.Add(Welcome, 0, 1);
            layout.Children.Add(Hello, 0, 2);
            Grid.SetColumnSpan(SOS, 3);
            Grid.SetColumnSpan(Hello, 3);
            Grid.SetColumnSpan(Welcome, 3);

            int row = 3;

            int maxTextLengthComment = 55;
            int maxTextLengthName = 22;

            double newYPostName = 5;
            double newYPostContent = 35;

            foreach (var answer in AnswerRequestPosts)
            {
                string NamePost = answer.name;
                if (NamePost.Length > maxTextLengthName)
                {
                    NamePost = NamePost.Substring(0, maxTextLengthName - 3) + "...";
                }

                string PostAuthorName = answer.user.username;
                if (PostAuthorName.Length > maxTextLengthComment)
                {
                    PostAuthorName = PostAuthorName.Substring(0, maxTextLengthComment - 3) + "...";
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
                //newYPostName += 75;

                Label PostAuthor = new Label
                {
                    Text = PostAuthorName,
                    TextColor = Xamarin.Forms.Color.FromHex("#626362"),
                    HorizontalOptions = LayoutOptions.Start,
                    TranslationY = newYPostContent,
                    FontSize = 10,
                    InputTransparent = true,
                };

                BoxView boxView = new BoxView
                {
                    Color = Xamarin.Forms.Color.FromHex("#1C1C1C"),
                    WidthRequest = ClientInfo.screenWidth / 4,
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

                layout.ColumnDefinitions.Add(new ColumnDefinition { Width = ClientInfo.screenWidth / 6 / ClientInfo.density }); // Левая пустая колонка
                layout.ColumnDefinitions.Add(new ColumnDefinition { Width = boxView.WidthRequest }); // Центральная колонка
                layout.ColumnDefinitions.Add(new ColumnDefinition { Width = ClientInfo.screenWidth / 6 / ClientInfo.density }); // Правая пустая колонка


                layout.RowDefinitions.Add(new RowDefinition { Height = ClientInfo.screenHeight / ClientInfo.density / 14 });

                layout.Children.Add(boxView, 1, row);
                layout.Children.Add(PostAuthor, 1, row);
                layout.Children.Add(PostName, 1, row);

                row++;
            }

            await PopupNavigation.Instance.PopAsync();
        }

    }
}
