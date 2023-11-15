using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace kursovaya
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FlyoutPage1Flyout : ContentPage
    {
        ListView listView;
        public ListView ListView { get { return listView; } }

        public FlyoutPage1Flyout()
        {
            InitializeComponent();

            BindingContext = new FlyoutPage1FlyoutViewModel();
            listView = MenuItemsListView;
            if (ClientInfo.Profile.id != -1)
                AppName.Text = ClientInfo.Profile.username;
            else
                AppName.Text = "Unknown";
            AppName.Text = AppName.Text.ToUpper();
        }
        private class FlyoutPage1FlyoutViewModel : INotifyPropertyChanged
        {
            public ObservableCollection<FlyoutPage1FlyoutMenuItem> MenuItems { get; set; }

            public FlyoutPage1FlyoutViewModel()
            {
                
                MenuItems = new ObservableCollection<FlyoutPage1FlyoutMenuItem>(new[]
                {
                    new FlyoutPage1FlyoutMenuItem { Id = 0, Title = "Профиль", Icon = "profile_icon.png" },
                    new FlyoutPage1FlyoutMenuItem { Id = 1, Title = "Создать пост", Icon = "pen_icon.png" },
                    new FlyoutPage1FlyoutMenuItem { Id = 2, Title = ClientInfo.Profile.id == -1 ? "Войти" : "Выйти", Icon = "door_icon.png" },
                });
                
            }

            #region INotifyPropertyChanged Implementation
            public event PropertyChangedEventHandler PropertyChanged;
            void OnPropertyChanged([CallerMemberName] string propertyName = "")
            {
                if (PropertyChanged == null)
                    return;

                PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
            #endregion
        }
    }
}
