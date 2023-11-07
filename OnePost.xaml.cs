using System.Collections.Generic;
using Newtonsoft.Json;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Rg.Plugins.Popup.Services;
using System.Linq;
using System;
using Rg.Plugins.Popup.Extensions;
using ApiRequest;
using static ApiRequest.Request;

namespace kursovaya
{

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class OnePost : ContentPage
    {
        int row = 7;
        private int PostID;
        LoadingPopup popup = new LoadingPopup();
        public OnePost(int id)
        {
            InitializeComponent();
            this.PostID = id;
        }
        async protected override void OnAppearing()
        {

            await PopupNavigation.Instance.PushAsync(popup);

            Label titleLabel = new Label
            {
                Text = "Souls of Stockholm",
                TextColor = Color.Wheat,
                FontSize = 20,
                Opacity = 0.5,
                TextDecorations = TextDecorations.Strikethrough,
                HorizontalOptions = LayoutOptions.End
            };
            NavigationPage.SetTitleView(this, titleLabel);

            try
            {
                Request.Root answer = await Request.GetInfoOnePost(PostID);

                if (answer.comments.Count() > 0)
                {
                    if (answer.comments.Count() * 3 + 7 == row) return;

                    var sortedComment = from i in answer.comments
                                        orderby i.id descending
                                        select i;

                    var color = Color.FromHex("#626362");

                    foreach (var comment in sortedComment)
                    {
                        Label authorComment = new Label
                        {
                            Text = comment.user.username,
                            TextColor = Color.FromHex("#0079FD"),
                            HorizontalOptions = LayoutOptions.Start,
                            VerticalOptions = LayoutOptions.Start,
                            FontSize = 16,
                            TextDecorations = TextDecorations.Underline,

                        };

                        authorComment.GestureRecognizers.Add(new TapGestureRecognizer()
                        {
                            Command = new Command(() =>
                            {
                                ViewProfile(comment.user.id);
                            })
                        });

                        Label commentariy = new Label
                        {
                            Text = comment.content,
                            TextColor = color,
                            HorizontalOptions = LayoutOptions.Start,
                            VerticalOptions = LayoutOptions.Start,
                            FontSize = 20,
                            FontAttributes = FontAttributes.Italic,
                        };

                        
                        OnePostLayout.RowDefinitions.Add(new RowDefinition { Height = 25 });
                        OnePostLayout.RowDefinitions.Add(new RowDefinition { Height = 20 });
                        OnePostLayout.RowDefinitions.Add(new RowDefinition { Height = 10 });
                        OnePostLayout.Children.Add(commentariy, 0, row);
                        OnePostLayout.Children.Add(authorComment, 0, row + 1);
                        row += 3;
                        
                    }
                }

                // Инициализация остальных компонентов
                PostName.Text = answer.post.name;
                PostAuthor.Text = answer.post.user.username;
                PostContent.Text = answer.post.content;
                Comments.Text = $"{answer.comments.Count()} комментариев";
                InputComment.Placeholder = "Введите комментарий";
                SendButton.Text = "Отправить";
                CancelButton.Text = "Отмена";
                SendButton.IsVisible = true;
                CancelButton.IsVisible = true;
                InputComment.IsVisible = true;

                PostAuthor.GestureRecognizers.Add(new TapGestureRecognizer()
                {
                    Command = new Command(() =>
                    {
                        ViewProfile(answer.post.user.id);
                    })
                });

            }
            finally
            {
                await PopupNavigation.Instance.PopAsync();
            }

            InputComment.TextChanged += (sender, e) =>
            {
                string text = InputComment.Text;
                if (string.IsNullOrEmpty(text))
                {
                    SendButton.IsEnabled = false;
                    SendButton.Opacity = 0.5;
                }
                else
                {
                    SendButton.IsEnabled = true;
                    SendButton.Opacity = 1;
                }
            };
        }
        private async void ViewProfile(int id)
        {
            if (id != ClientInfo.Profile.id)
            {
                await PopupNavigation.Instance.PushAsync(popup);
                ApiRequest.Request.OtherProfile profile = await Request.GetInfoOtherProfile(id);
                var info = new Profile(ref profile);
                await Navigation.PushAsync(info);
                await PopupNavigation.Instance.PopAsync();

            }
        }
        async private void OnSendClicked(object sender, EventArgs e)
        {

            var CodeRequestAddComment = await ApiRequest.Request.PostAddComment(content: InputComment.Text, user: ClientInfo.Profile.id, post: PostID);

           
            switch (CodeRequestAddComment)
            {
                case 201:
                    await DisplayAlert("Успех", "Комментарий добавлен!", "OK");
                    for (int i = OnePostLayout.RowDefinitions.Count - 1; i >= 7; i--)
                    {
                        OnePostLayout.RowDefinitions.RemoveAt(i);

                        for (int j = OnePostLayout.ColumnDefinitions.Count - 1; j >= 0; j--)
                        {
                            View view = OnePostLayout.Children.FirstOrDefault(c => Grid.GetRow(c) == i && Grid.GetColumn(c) == j);
                            if (view != null)
                            {
                                OnePostLayout.Children.Remove(view);
                            }
                        }
                    }

                    row = 7;
                    OnAppearing();
                    OnCancelClicked(sender, e);
                    break;
                case 401:
                    await DisplayAlert("Ошибка", "Пожалуйста, авторизуйтесь!", "OK");
                    OnCancelClicked(sender,e);
                    break;
                default:
                    await DisplayAlert("Ошибка", "Что-то пошло не так...", "OK");
                    OnCancelClicked(sender, e);
                    break;
            }
        }

        private void OnCancelClicked(object sender, EventArgs e)
        {
            if(InputComment.Text != "")
            {
                InputComment.Text = "";
            }
        }

    }

}