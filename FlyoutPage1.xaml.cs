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
    public partial class FlyoutPage1 : FlyoutPage
    {
        public FlyoutPage1()
        {
            InitializeComponent();
            FlyoutPage.ListView.ItemSelected += ListView_ItemSelected;
        }

        private async void ListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            ((ListView)sender).SelectedItem = null;
            if (e.SelectedItem is FlyoutPage1FlyoutMenuItem menuItem)
            {
                // Проверьте, какой элемент меню был нажат
                if (menuItem.Id == 1)
                {
                    
                    await Navigation.PushAsync(new CreatePostPage());
                }
            }
        }
    }
}