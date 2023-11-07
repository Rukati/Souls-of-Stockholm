using ApiRequest;
using Newtonsoft.Json;
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
        private List<Posts> AnswerRequestPosts = new List<Posts>();
        Grid layout;
        public PageWithAllPosts()
        {
            InitializeComponent();
            layout = GridText;
        }
        protected override async void OnAppearing()
        {
            AnswerRequestPosts = await GetAllPosts();
            
            if (layout.Children.Count > 0)
            {
                layout.RowDefinitions.Clear();
                layout.ColumnDefinitions.Clear();
                layout.Children.Clear();
            }

            Label titleLabel = new Label
            {
                Text = "🜲🜲🜲🜲🜲🜲",
                TextColor = Xamarin.Forms.Color.Wheat,
                FontSize = 20,
                Opacity = 0.5,
                TextDecorations = TextDecorations.Strikethrough,
                HorizontalOptions = LayoutOptions.Center
            };
            NavigationPage.SetTitleView(this, titleLabel);

            ShowPosts(AnswerRequestPosts);

        }
        private void ViewPost(object sender, EventArgs e)
        {
            BoxView clickedButton = (BoxView)sender;
            int classId = int.Parse(clickedButton.ClassId);

            Navigation.PushAsync(new OnePost(classId));
        }
        private void ShowPosts(List<Posts> answerList)
        {
            layout.RowDefinitions.Add(new RowDefinition { Height = 125 });
            Label SOS = new Label
            {
                Text = "SOS",
                TextColor = Xamarin.Forms.Color.FromHex("#1C1C1C"),
                FontSize = 120,
                HorizontalOptions = LayoutOptions.Center,
                Margin = new Thickness(0,-5,0,0),
            };

            layout.RowDefinitions.Add(new RowDefinition { Height = 25});
            Label Welcome = new Label
            {
                Text = "Добро пожаловать на Souls of Stockholm",
                TextColor = Xamarin.Forms.Color.FromHex("#1C1C1C"),
                FontSize = 15,
                HorizontalOptions = LayoutOptions.Center,
                FontAttributes = FontAttributes.Bold
            };

            layout.RowDefinitions.Add(new RowDefinition { Height = 20 });
            Label Hello = new Label
            {
                Text = "Здесь не салам, здесь здравствуйте!",
                TextColor = Xamarin.Forms.Color.FromHex("#1C1C1C"),
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

            foreach (var answer in answerList)
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


                layout.RowDefinitions.Add(new RowDefinition { Height = ClientInfo.screenHeight / answerList.Count / ClientInfo.density + 10});

                layout.Children.Add(boxView, 1, row);
                layout.Children.Add(PostAuthor, 1, row);
                layout.Children.Add(PostName, 1, row);

                row++;
            }
            /*          
                        //-------------------------------------------------------------

                        Label back = new Label
                        {
                            Text = "Назад",
                            TextColor = Xamarin.Forms.Color.FromHex("#626362"),
                            HorizontalOptions = LayoutOptions.Start,
                            TranslationX = ClientInfo.screenWidth / ClientInfo.density / 6,
                            TranslationY = ClientInfo.screenHeight / ClientInfo.density - 65,
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
                            TranslationX = ClientInfo.screenWidth / ClientInfo.density / 3 * 2,
                            TranslationY = ClientInfo.screenHeight / ClientInfo.density - 65,
                            FontSize = 20,
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
                            TranslationX = ClientInfo.screenWidth / ClientInfo.density / 2 - 10,
                            TranslationY = ClientInfo.screenHeight / ClientInfo.density - 65,
                            FontSize = 20,
                        };

                        //-------------------------------------------------------------

                        int lastPage = (int)Math.Ceiling((double)answerList.Count / 5);
                        if (page == lastPage)
                        {
                            layout.Children.Add(nowPage);
                            layout.Children.Add(back);
                        }
                        else if (page != 1)
                        {
                            layout.Children.Add(back);
                            layout.Children.Add(nowPage);
                            layout.Children.Add(next);
                        }
                        else
                        {
                            layout.Children.Add(next);
                            layout.Children.Add(nowPage);
                        }
            */
        }

    }
}
